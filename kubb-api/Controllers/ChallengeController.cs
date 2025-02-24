using KubbAdminAPI.Models;
using KubbAdminAPI.Models.RequestModels.Challenge;
using KubbAdminAPI.Models.ResponseModels.Challenge;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KubbAdminAPI.Controllers;

[ApiController, Route("[controller]/[action]")]
public class ChallengeController(DatabaseContext context) : BaseController
{
    [HttpPost]
    public ActionResult JoinChallenge([FromBody] JoinChallengeRequest request)
    {
        var challenge = context.Challenges.FirstOrDefault(challenge => challenge.ChallengeId == request.ChallengeId);

        if (challenge == null || challenge.StartTime < DateTime.UtcNow ||
            (challenge.Status & ChallengeStatus.AllowAnonymousJoin) != ChallengeStatus.AllowAnonymousJoin)
        {
            return new UnauthorizedObjectResult("Something went wrong with your request");
        }

        var participation = new Participation
        {
            Challenge = challenge,
            User = CurrentUser()!
        };
        context.Participations.Add(participation);
        context.SaveChanges();

        return new OkResult();
    }

    [HttpPost]
    public ActionResult<SendAnswerResponse> SendAnswer([FromBody] SendAnswerRequest request)
    {
        var team = context.Teams.Include(team => team.Challenge).ThenInclude(challenge => challenge.Questions)
            .Include(team => team.Administrator)
            .FirstOrDefault(team => team.TeamId == request.TeamId);
        
        if (team == null || team.Challenge.Questions.Count() <= request.QuestionId ||
            team.Administrator != CurrentUser())
        {
            return BadRequest();
        }

        if (team.Challenge.EndTime < DateTime.UtcNow)
        {
            return Unauthorized("Challenge is finished");
        }

        var answer = new Answer
        {
            AnswerText = request.AnswerText,
            Question = request.QuestionId,
            Team = team
        };
        context.Answers.Add(answer);
        context.SaveChanges();

        return new SendAnswerResponse
        {
            Correctness = team.Challenge.Questions[request.QuestionId] == request.AnswerText,
        };
    }

    [HttpPut]
    public ActionResult EditAnswer([FromBody] EditAnswerRequest request)
    {
        var answer = context.Answers.Include(answer => answer.Team).ThenInclude(team => team.Challenge)
            .FirstOrDefault(answer => answer.AnswerId == request.AnswerId);

        if (answer == null || (answer.Team.Challenge.Status & ChallengeStatus.JoinersCanEditAnswer) !=
            ChallengeStatus.JoinersCanEditAnswer)
        {
            return BadRequest();
        }

        answer.AnswerText = request.AnswerText;
        context.SaveChanges();

        return Ok();
    }

    [HttpDelete]
    public ActionResult DeleteAnswer([FromBody] DeleteAnswerRequest request)
    {
        var answer = context.Answers.Include(answer => answer.Team).ThenInclude(team => team.Challenge)
            .FirstOrDefault(answer => answer.AnswerId == request.AnswerId);

        if (answer == null || answer.Team.TeamType == TeamType.JoinedTeam &&
            (answer.Team.Challenge.Status & ChallengeStatus.JoinersCanEditAnswer) !=
            ChallengeStatus.JoinersCanEditAnswer)
        {
            return BadRequest();
        }

        context.Answers.Remove(answer);
        context.SaveChanges();

        return Ok();
    }

    [HttpPost]
    public ActionResult<CreateTeamResponse> CreateTeam([FromBody] CreateTeamRequest request)
    {
        var challenge = context.Challenges.Include(challenge => challenge.Administrator)
            .FirstOrDefault(challenge => challenge.ChallengeId == request.ChallengeId);

        var user = CurrentUser();
        
        if (challenge == null || challenge.StartTime < DateTime.UtcNow ||
            challenge.Administrator != user && !context.Participations.Any(participation =>
                participation.User == user && participation.Challenge == challenge))
        {
            return Unauthorized();
        }

        var team = new Team
        {
            Administrator = user,
            TeamType = TeamType.JoinedTeam,
            TeamName = request.Name,
            Challenge = challenge
        };
        
        context.Teams.Add(team);
        context.SaveChanges();

        return new CreateTeamResponse
        {
            TeamId = team.TeamId
        };
    }

    [HttpDelete]
    public ActionResult DeleteTeam([FromBody] DeleteTeamRequest request)
    {
        var team = context.Teams.Include(team => team.Administrator).Include(team => team.Challenge).FirstOrDefault(team => team.TeamId == request.TeamId);
        if (team == null || team.Administrator != CurrentUser() || team.Challenge.StartTime < DateTime.UtcNow)
        {
            return Unauthorized();
        }

        context.Teams.Remove(team);
        context.SaveChanges();
        
        return Ok();
    }
}