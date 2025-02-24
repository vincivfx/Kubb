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
    public ActionResult<OkResult> JoinChallenge([FromBody] JoinChallengeRequest request)
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
        var team = context.Teams.Include(team => team.Challenge).ThenInclude(challenge => challenge.Questions).Include(team => team.Administrator)
            .FirstOrDefault(team => team.TeamId == request.TeamId);

        var question = context.Questions.FirstOrDefault(question => question.QuestionId == request.QuestionId);
        
        if (team == null || question == null || !team.Challenge.Questions.Contains(question) || team.Administrator != CurrentUser())
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
            Question = question,
            Team = team
        };
        context.Answers.Add(answer);
        context.SaveChanges();

        return new SendAnswerResponse
        {
            Correctness = question.AnswerText.Equals(request.AnswerText)
        };
    }

    [HttpPut]
    public void EditResponse()
    {
    }

    [HttpDelete]
    public void DeleteResponse()
    {
    }

    [HttpPost]
    public ActionResult<OkResult> CreateTeam()
    {
        return new OkResult();
    }

    [HttpPut]
    public void UpdateTeam()
    {
    }

    [HttpDelete]
    public void DeleteTeam()
    {
    }
}