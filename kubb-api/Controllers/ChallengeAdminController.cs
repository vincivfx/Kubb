using Microsoft.AspNetCore.Mvc;

namespace KubbAdminAPI.Controllers;

[ApiController, Route("[controller]/[action]")]
public class ChallengeAdminController(DatabaseContext context) : BaseController
{
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

        var participations = context.Participations.Where(participation => participation.Challenge == challenge).Select(participation => new
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
    public void CreateChallenge()
    {
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