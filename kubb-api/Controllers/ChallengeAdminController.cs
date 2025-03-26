using System.ComponentModel.DataAnnotations;
using KubbAdminAPI.Filters;
using KubbAdminAPI.Models;
using KubbAdminAPI.Models.RequestModels;
using KubbAdminAPI.Models.RequestModels.ChallengeAdmin;
using KubbAdminAPI.Models.ResponseModels.ChallengeAdmin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge = KubbAdminAPI.Models.Challenge;

namespace KubbAdminAPI.Controllers;

/**
 * MISSION: allowing challenge administrators to create / edit / manage their challenges 
 */
[ApiController, Route("[controller]/[action]"), AuthenticationFilter]
public class ChallengeAdminController(DatabaseContext context) : BaseController
{
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

        var teams = context.Teams.Include(team => team.Administrator).Where(team => team.Challenge == challenge)
            .Select(team => new ChallengeInfoResponse.Team(team, current)).ToList();

        return Ok(new ChallengeInfoResponse(challenge, participations, teams));
    }


    [HttpPost]
    public ActionResult<CreateChallengeResponse> CreateChallenge([FromBody] CreateChallengeRequest request)
    {
        var user = CurrentUser();
        if ((user.Status & UserStatus.ChallengeCreator) == 0) return Unauthorized("you are not challenge creator");

        // TODO: check startTime > today
        if (request.StartTime < DateTime.UtcNow) return BadRequest();

        var challenge = new Challenge
        {
            Name = request.ChallengeName,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
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
            (challenge.RunningStatus == RunningChallengeStatus.Draft || challenge.RunningStatus == RunningChallengeStatus.Submitted));

        if (challenge == null) return Unauthorized();

        // allow only setting status to submitted
        if (request.RunningStatus != RunningChallengeStatus.Submitted && request.RunningStatus != RunningChallengeStatus.Draft)
        {
            return Unauthorized();
        }

        if (challenge.RunningStatus == RunningChallengeStatus.Draft)
        {
            challenge.Name = request.Name;
            challenge.StartTime = request.StartTime;
            challenge.EndTime = request.EndTime;
            challenge.RunningStatus = request.RunningStatus;
            challenge.Status = request.Status;
        }
        challenge.Questions = request.Questions;
        challenge.MaxTeamPerUser = request.MaxTeamPerUser;
        challenge.BasePoints = request.BasePoints;
        challenge.AlgorithmSettings = request.AlgorithmSettings;


        context.SaveChanges();

        return Ok();
    }


    [HttpPost]
    public ActionResult DeleteChallenge([FromBody] GenericChallengeRequest request)
    {

        var currentUser = CurrentUser();
        var challenge = context.Challenges.FirstOrDefault(challenge => challenge.ChallengeId == request.ChallengeId && challenge.Administrator == currentUser);

        if (challenge == null) return Unauthorized();

        // Stuff to delete
        var teams = context.Teams.Where(team => team.Challenge == challenge).ToList();
        var answers = context.Answers.Where(answer => teams.Contains(answer.Team)).ToList();
        var participations = context.Participations.Where(participation => participation.Challenge == challenge).ToList();

        context.Answers.RemoveRange(answers);
        context.Participations.RemoveRange(participations);
        context.Teams.RemoveRange(teams);
        context.Challenges.Remove(challenge);

        context.SaveChanges();

        return Ok();

    }

    [HttpPost]
    public ActionResult ManualStart([FromBody] GenericChallengeRequest request)
    {
        var currentUser = CurrentUser();
        var challenge = context.Challenges.FirstOrDefault(challenge => challenge.ChallengeId == request.ChallengeId && challenge.Administrator == currentUser);

        if (challenge == null || (challenge.Status & ChallengeStatus.StartEnabled) != 0) return Unauthorized();

        var timeDifference = challenge.EndTime.Subtract(challenge.StartTime).TotalSeconds;

        challenge.StartTime = DateTime.UtcNow;
        challenge.EndTime = DateTime.UtcNow.AddSeconds(timeDifference);
        challenge.RunningStatus = RunningChallengeStatus.Running;

        context.SaveChanges();

        return Ok();

    }
}