using KubbAdminAPI.Filters;
using KubbAdminAPI.Models;
using KubbAdminAPI.Models.RequestModels;
using KubbAdminAPI.Models.RequestModels.ChallengeAdmin;
using KubbAdminAPI.Models.ResponseModels.ChallengeAdmin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public ActionResult<ChallengeInfoResponse> ChallengeInfo([FromQuery] Guid challengeId)
    {
        var current = CurrentUser()!;
        var challenge = context.Challenges.FirstOrDefault(challenge =>
            challenge.ChallengeId == challengeId && challenge.Administrator == current);

        if (challenge == null)
        {
            return Unauthorized();
        }

        var participations = context.Participations.Where(participation => participation.Challenge == challenge).Select(
            participation => new ChallengeInfoResponse.Participation(participation)).ToList();

        var teams = context.Teams.Where(team => team.Challenge == challenge)
            .Select(team => new ChallengeInfoResponse.Team(team)).ToList();

        return Ok(ChallengeInfoResponse.Create(challenge, participations, teams));
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
    public IActionResult UpdateChallenge([FromBody] UpdateChallengeRequest request)
    {
        var user = CurrentUser();
        // Challenge should be set in "DRAFT" status to be modified
        var challenge = context.Challenges.Include(challenge => challenge.Administrator).FirstOrDefault(challenge =>
            challenge.ChallengeId == request.ChallengeId && challenge.Administrator == user &&
            challenge.RunningStatus == RunningChallengeStatus.Draft);

        if (challenge == null) return Unauthorized();

        // allow only setting status to submitted
        if (request.RunningStatus != RunningChallengeStatus.Submitted && request.RunningStatus != RunningChallengeStatus.Draft) {
            return Unauthorized();
        }

        // TODO: Check statuses
        
        challenge.Name = request.Name;
        challenge.StartTime = request.StartTime;
        challenge.EndTime = request.EndTime;
        challenge.Status = request.Status;
        challenge.RunningStatus = request.RunningStatus;
        context.SaveChanges();

        return Ok();
    }


    [HttpDelete]
    public void DeleteChallenge()
    {
    }
    
}