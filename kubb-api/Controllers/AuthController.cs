using KubbAdminAPI.Models;
using KubbAdminAPI.Models.RequestModels.Auth;
using KubbAdminAPI.Models.ResponseModels.Auth;
using KubbAdminAPI.Utils;
using KubbAdminAPI.Workers;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = KubbAdminAPI.Models.RequestModels.Auth.LoginRequest;
using RegisterRequest = KubbAdminAPI.Models.RequestModels.Auth.RegisterRequest;

namespace KubbAdminAPI.Controllers;

[ApiController, Route("[controller]/[action]")]
public class AuthController(DatabaseContext context, EmailSenderWorker emailSenderWorker) : BaseController
{
    [HttpPost]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        
        var user = context.Users.FirstOrDefault(user => user.EmailAddress == request.EmailAddress);

        // if user not found or user not active return error 401 
        if (user == null || (user.Status & UserStatus.Active) != UserStatus.Active)
        {
            return new UnauthorizedResult();
        }

        var login = new Login
        {
            User = user
        };

        var loginToken = login.SetToken();

        context.Logins.Add(login);
        context.SaveChanges();

        return new LoginResponse
        {
            Success = true,
            Token = loginToken,
        };

    }

    [HttpPost]
    public ActionResult Register([FromBody] RegisterRequest request)
    {
        // check if email address is registered yet
        if (context.Users.Any(user => user.EmailAddress == request.EmailAddress))
        {
            return new BadRequestObjectResult("Email address already exists");
        }

        var user = new User
        {
            EmailAddress = request.EmailAddress,
            Name = request.Name,
            Surname = request.Surname,
            Status = UserStatus.NeedsVerification
        };
        
        user.SetPasswordHash(request.Password);
        var verificationToken = user.SetVerificationToken();
        
        context.Users.Add(user);
        context.SaveChanges();
        
        var messageHtml = @"Hi, in order to complete your registration, please enter the following link: <br><br>https://localhost:8080/auth/verify?token=" + verificationToken + "<br><br>best regards,<br>Kubb Contest Platform";

        var message = EmailFactory.CreateEmailMessage(user.EmailAddress, "Complete Registration", messageHtml);
        
        Task.Run(() => emailSenderWorker.QueueBackgroundWorkItem(message));

        
        return new OkResult();
    }

    [HttpPost]
    public ActionResult RecoverPassword([FromBody] RecoverPasswordRequest request)
    {
        
        var user = context.Users.FirstOrDefault(user => user.EmailAddress == request.EmailAddress);

        if (user == null || (user.Status & UserStatus.Active) != UserStatus.Active)
        {
            return new UnauthorizedResult();
        }

        var temporaryPassword = user.SetTemporaryPassword();

        var messageHtml = @"Hi, in order to reset your password, please use this temporary one: <br><br>" + temporaryPassword + "<br><br>best regards,<br>Kubb Contest Platform";
        
        var message = EmailFactory.CreateEmailMessage(user.EmailAddress, "Reset Password", messageHtml);
        
        Task.Run(() => emailSenderWorker.QueueBackgroundWorkItem(message));
        
        return new OkResult();
    }

    [HttpPost]
    public ActionResult Logout()
    {

        context.Logins.Remove(CurrentUserLogin());
        context.SaveChanges();

        return new OkResult();

    }

    [HttpPost]
    public ActionResult CompleteRegistration([FromBody] CompleteRegistrationRequest request)
    {
        var user = context.Users.FirstOrDefault(user => user.EmailAddress == request.EmailAddress);
        
        if (user == null || (user.Status & UserStatus.NeedsVerification) != UserStatus.NeedsVerification)
        {
            return BadRequest();
        }
        
        user.Status &= ~UserStatus.NeedsVerification;
        user.Status &= UserStatus.Active;
        context.Users.Update(user);
        context.SaveChanges();

        return new OkResult();


    }
}