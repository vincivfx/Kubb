using KubbAdminAPI.Filters;
using KubbAdminAPI.Models;
using KubbAdminAPI.Models.RequestModels;
using KubbAdminAPI.Models.RequestModels.Challenge;
using KubbAdminAPI.Models.ResponseModels.Challenge;
using KubbAdminAPI.Models.ResponseModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KubbAdminAPI.Controllers;

[ApiController, Route("[controller]/[action]"), AuthenticationFilter]
public class ChallengeController(DatabaseContext context) : BaseController
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
    public ActionResult<GetInfoResponse> GetInfo([FromQuery] Guid ChallengeId)
    {
        var challenge = context.Challenges.Include(challenge => challenge.Administrator).FirstOrDefault(challenge => challenge.ChallengeId == ChallengeId);
        if (challenge == null) return NotFound();

        var currentUser = CurrentUser();

        var teams = context.Teams.Where(team => team.Challenge == challenge && team.Administrator == currentUser).ToList();

        if (challenge.Administrator != currentUser && context.Participations.Any(p => p.Challenge == challenge && p.User == currentUser)) return Unauthorized();

        return Ok(new GetInfoResponse(teams, challenge));
    }

    /**
     * 
     */
    [HttpGet]
    public ActionResult<GetCurrentParticipationsResponse> GetCurrentParticipations([FromQuery] Pagination pagination)
    {
        if (pagination.Limit > 100) return BadRequest("Limit can't be more than 100");
        var currentUser = CurrentUser()!;
        var participations = context.Participations.Include(participation => participation.Challenge).Where(participation => participation.User == currentUser)
            .OrderByDescending(participation => participation.Challenge.StartTime).Skip(pagination.Limit * pagination.Offset).Take(pagination.Limit).Select(participation => new GetCurrentParticipationsResponse.Challenge(participation.Challenge))
            .ToList();
        var totalCount = context.Participations.Count(participation => participation.User == currentUser);
        return Ok(new GetCurrentParticipationsResponse(totalCount, participations));
    }


    [HttpGet]
    [AuthenticationFilter]
    public ActionResult<ChallengesResponse> Available([FromQuery] Pagination pagination)
    {
        if (pagination.Limit > 100) return BadRequest("Limit can't be more than 100");
        var challenges =
            context.Challenges.Where(challenge => (challenge.RunningStatus == RunningChallengeStatus.Submitted) && (challenge.Status & ChallengeStatus.AllowAnonymousJoin) != 0)
                .OrderBy(challenge => challenge.StartTime).Skip(pagination.Offset * pagination.Limit).Take(pagination.Limit).Select(challenge => new ChallengesResponse.Challenge(challenge)).ToList();
        var totalCount = context.Challenges.Count(challenge => (challenge.RunningStatus == RunningChallengeStatus.Submitted) && (challenge.Status & ChallengeStatus.AllowAnonymousJoin) != 0);
        return Ok(new ChallengesResponse(challenges, totalCount));
    }

    [HttpPost]
    public ActionResult JoinChallenge([FromBody] JoinChallengeRequest request)
    {
        var challenge = context.Challenges.FirstOrDefault(challenge => challenge.ChallengeId == request.ChallengeId);

        if (challenge is not { RunningStatus: RunningChallengeStatus.Submitted } ||
            (challenge.Status & ChallengeStatus.AllowAnonymousJoin) != ChallengeStatus.AllowAnonymousJoin)
        {
            return Unauthorized("Something went wrong with your request");
        }

        var user = CurrentUser()!;

        var existingParticipation = context.Participations.FirstOrDefault(part => part.Challenge == challenge);

        if (challenge.Administrator == user || existingParticipation != null) return Ok(); // even if it is not created

        var participation = new Participation
        {
            Challenge = challenge,
            User = user
        };
        context.Participations.Add(participation);
        context.SaveChanges();

        return new OkResult();
    }

    [HttpGet]
    public ActionResult<GetAnswersResponse> GetAnswers([FromQuery] Guid ChallengeId)
    {
        var challenge = context.Challenges.Include(challenge => challenge.Administrator)
                                        .FirstOrDefault(challenge => challenge.ChallengeId == ChallengeId);

        if (challenge == null) return NotFound();

        var currentUser = CurrentUser();

        // if user is administrator return all answers
        if (challenge.Administrator == currentUser)
        {
            var adminQuery = from team in context.Teams
                             from answer in context.Answers
                             where team.Challenge == challenge && answer.Team == team
                             group answer by answer.Team into answerList
                             select new GetAnswersResponse.TeamAnswer(answerList.Key, answerList.ToList(), challenge);

            var adminAnswers = adminQuery.ToList();

            return Ok(new GetAnswersResponse(adminAnswers));
        }

        // user has a participation
        var query = from team in context.Teams
                    from participation in context.Participations
                    from answer in context.Answers
                    where participation.Challenge == challenge && participation.User == currentUser && team.Challenge == challenge && answer.Team == team
                    group answer by answer.Team into answerList
                    select new GetAnswersResponse.TeamAnswer(answerList.Key, answerList.ToList(), challenge);

        var answers = query.ToList();

        return Ok(new GetAnswersResponse(answers));

    }


    [HttpPost]
    public ActionResult<SendAnswerResponse> SendAnswer([FromBody] SendAnswerRequest request)
    {
        var team = context.Teams.Include(team => team.Challenge)
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

    [HttpPost]
    public ActionResult SendJolly()
    {
        return BadRequest();
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

        if (challenge == null || challenge.StartTime != null && challenge.StartTime < DateTime.UtcNow ||
            challenge.Administrator != user && !context.Participations.Any(participation =>
                participation.User == user && participation.Challenge == challenge))
        {
            return Unauthorized();
        }

        if (challenge.Administrator != user && context.Teams.Count(team => team.Administrator == user && team.Challenge == challenge) == challenge.MaxTeamPerUser) return Unauthorized();

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
        var team = context.Teams.Include(team => team.Administrator).Include(team => team.Challenge)
            .FirstOrDefault(team => team.TeamId == request.TeamId);
        if (team == null || team.Administrator != CurrentUser() || team.Challenge.StartTime < DateTime.UtcNow)
        {
            return Unauthorized();
        }

        context.Teams.Remove(team);
        context.SaveChanges();

        return Ok();
    }
}