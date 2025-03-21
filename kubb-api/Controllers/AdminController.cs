using KubbAdminAPI.Models.RequestModels;
using KubbAdminAPI.Models.ResponseModels.Admin;
using Microsoft.AspNetCore.Mvc;
using KubbAdminAPI.Filters;
using KubbAdminAPI.Models.RequestModels.Admin;
using KubbAdminAPI.Models;
using KubbAdminAPI.Utils;
using KubbAdminAPI.Singletons;

namespace KubbAdminAPI.Controllers;

/**
 * MISSION: manage server administrator endpoints, allowing creation / edit / deletion of users
 */
[ApiController, Route("[controller]/[action]")]
[AuthenticationFilter, AdministratorFilter]
public class AdminController(DatabaseContext context, EmailTask emailTask, IConfiguration configuration) : BaseController
{
    [HttpGet]
    public ActionResult<UsersResponse> Users([FromQuery] Pagination pagination)
    {
        if (pagination.Limit > 100) return BadRequest();
        var users = context.Users.OrderBy(user => user.Created).Skip(pagination.Offset * pagination.Limit).Take(pagination.Limit).Select(user => new UsersResponse.User(user)).ToList();
        var userCount = context.Users.Count();

        return Ok(new UsersResponse(userCount, users));
    }

    [HttpGet]
    public ActionResult<GetUserResponse> GetUser([FromQuery] Guid UserId)
    {
        var user = context.Users.FirstOrDefault(user => user.UserId == UserId);
        if (user == null) return NotFound();
        return Ok(new GetUserResponse(user));
    }

    [HttpGet]
    public ActionResult<GetUserChallengesResponse> GetUserChallenges([FromQuery] Guid UserId)
    {
        var user = context.Users.FirstOrDefault(user => user.UserId == UserId);

        if (user == null) return NotFound();

        var challenges = context.Challenges.Where(challenge => challenge.Administrator == user).Select(challenge => new GetUserChallengesResponse.Challenge(challenge)).ToList();

        return Ok(new GetUserChallengesResponse(challenges));
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        if (request.Password == null && request.SendVerificationEmail == false) return BadRequest();

        if (context.Users.Any(user => user.EmailAddress == request.EmailAddress)) return BadRequest("mail registered");

        var user = new User
        {
            EmailAddress = request.EmailAddress,
            Name = request.Name,
            Surname = request.Surname,
            Status = request.Status
        };

        if (request.SendVerificationEmail)
        {
            var verificationToken = user.SetVerificationToken();
            var emailText = @"Hi, in order to complete your registration, please enter the following link: <br><br>" + configuration.GetValue<string>("SystemConfiguration:RootUrl") + "/auth/verify?email=" + Uri.EscapeDataString(user.EmailAddress) + "&token=" + verificationToken;
            if (request.SendRandomPassword)
            {
                var temporaryPassword = user.SetTemporaryPassword();
                emailText += "<br><br>Use this password for the first login: <b>" + temporaryPassword + "</b>";
                user.Status &= UserStatus.MustChangePassword;
            }
            emailText += "<br><br>best regards,<br>Kubb Contest Platform";
            emailTask.EnqueueEmail(EmailFactory.CreateEmailMessage(request.EmailAddress, "Verify your Account", emailText));
            user.Status |= UserStatus.NeedsVerification;
            user.SetPasswordHash(request.Password!);
        }
        else
        {
            user.SetPasswordHash(request.Password!);
        }

        context.Users.Add(user);
        context.SaveChanges();

        return Ok();
    }

    [HttpPut]
    public ActionResult UpdateUser([FromBody] UpdateUserRequest request)
    {
        var current = CurrentUser();
        var user = context.Users.FirstOrDefault(user => user.UserId == request.UserId);
        if (user == null) return BadRequest();
        
        // cannot set NeedsVerification = 1 if user is verified!
        if ((user.Status & UserStatus.NeedsVerification) == 0 && (request.Status & UserStatus.NeedsVerification) != 0) return BadRequest();
        
        // admin cannot disable theirselves.
        if (user == current && ((request.Status & UserStatus.Administrator) == 0 || (request.Status & UserStatus.Active) == 0)) return BadRequest();

        user.Name = request.Name;
        user.Status = request.Status;
        user.Surname = request.Surname;

        if (request.ResetPassword)
        {
            Console.WriteLine(request.Password!);
            user.SetPasswordHash(request.Password!);
        }

        context.SaveChanges();
        return Ok();
    }

    [HttpDelete]
    public ActionResult DeleteUser([FromBody] Guid UserId)
    {
        /* cannot delete the current user
        var currentUser = CurrentUser();
        if (currentUser.UserId == UserId) return BadRequest();

        var user = context.Users.FirstOrDefault(user => user.UserId == UserId);
        if (user == null) return BadRequest();

        var challenges = context.Challenges.Where(challenge => challenge.Administrator == user).ToList();
        */
        return NotFound();
    }
}