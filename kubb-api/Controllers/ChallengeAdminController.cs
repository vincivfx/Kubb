using KubbAdminAPI.Filters;
using KubbAdminAPI.Models;
using KubbAdminAPI.Models.RequestModels;
using KubbAdminAPI.Models.RequestModels.ChallengeAdmin;
using KubbAdminAPI.Models.ResponseModels.ChallengeAdmin;
using Microsoft.AspNetCore.Mvc;
using Challenge = KubbAdminAPI.Models.Challenge;

namespace KubbAdminAPI.Controllers;

[ApiController, Route("[controller]/[action]"), AuthenticationFilter]
public class ChallengeAdminController(DatabaseContext context) : BaseController
{
    [HttpGet]
    public IActionResult All([FromQuery] Pagination pagination)
    {
        if (pagination.Limit > 100) return BadRequest();
        var user = CurrentUser();
        var challenges = context.Challenges.Where(challenge => challenge.Administrator == user)
            .OrderByDescending(challenge => challenge.EndTime).Select(challenge => new
            {
                challenge.ChallengeId,
                challenge.Name,
                challenge.StartTime,
                challenge.EndTime,
                challenge.RunningStatus,
                challenge.Status
            }).Skip(pagination.Offset * pagination.Limit)
            .Take(pagination.Limit).ToList();
        return Ok(challenges);
    }

    [HttpGet]
    public IActionResult ChallengeInfo([FromBody] Guid challengeId)
    {
        var current = CurrentUser()!;
        var challenge = context.Challenges.FirstOrDefault(challenge =>
            challenge.ChallengeId == challengeId && challenge.Administrator == current);

        if (challenge == null)
        {
            return Unauthorized();
        }

        var participations = context.Participations.Where(participation => participation.Challenge == challenge).Select(
            participation => new
            {
                participation.User.Name,
                participation.User.Surname,
                participation.User.EmailAddress,
                participation.Created
            }).ToList();

        var teams = context.Teams.Where(team => team.Challenge == challenge).ToList();

        return Ok(new
        {
            // TODO: response
        });
    }


    [HttpPost]
    public ActionResult<CreateChallengeResponse> CreateChallenge([FromBody] CreateChallengeRequest request)
    {
        var user = CurrentUser();
        if ((user.Status & UserStatus.ChallengeCreator) == 0) return Unauthorized("you are not challenge creator");

        var challenge = new Challenge
        {
            Name = request.ChallengeName,
            Administrator = user,
            Questions = []
        };

        context.Challenges.Add(challenge);
        context.SaveChanges();

        return Ok(new CreateChallengeResponse
        {
            ChallengeId = challenge.ChallengeId
        });
    }

    [HttpPut]
    public void UpdateChallenge()
    {
    }

    [HttpDelete]
    public void DeleteChallenge()
    {
    }
}