using KubbAdminAPI.Models;
using KubbAdminAPI.Models.Cache;
using KubbAdminAPI.Models.Configuration;
using KubbAdminAPI.Models.RequestModels;
using KubbAdminAPI.Models.ResponseModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace KubbAdminAPI.Controllers;

[ApiController, Route("[controller]/[action]")]
public class HomeController(DatabaseContext _context, IMemoryCache _cache, IConfiguration _configuration) : BaseController
{
    [HttpGet]
    public ActionResult Challenges([FromQuery] Pagination pagination)
    {
        if (pagination.Limit > 100) return BadRequest("Limit can't be more than 100");
        var challenges =
            _context.Challenges.Where(challenge => (challenge.RunningStatus == RunningChallengeStatus.Submitted || challenge.RunningStatus == RunningChallengeStatus.Running) && (challenge.Status & ChallengeStatus.Visible) != 0)
                .OrderBy(challenge => challenge.StartTime).Skip(pagination.Offset * pagination.Limit).Take(pagination.Limit).Select(challenge => new ChallengesResponse.Challenge(challenge)).ToList();
        var totalCount = _context.Challenges.Count(challenge => (challenge.RunningStatus == RunningChallengeStatus.Submitted || challenge.RunningStatus == RunningChallengeStatus.Running) && (challenge.Status & ChallengeStatus.Visible) != 0);
        return Ok(new ChallengesResponse(challenges, totalCount));
    }

    [HttpGet]
    public ActionResult Archived([FromQuery] Pagination pagination)
    {
        if (pagination.Limit > 100) return BadRequest("Limit can't be more than 100");
        var challenges =
            _context.Challenges.Where(challenge => (challenge.RunningStatus == RunningChallengeStatus.Frozen || challenge.RunningStatus == RunningChallengeStatus.Terminated) && (challenge.Status & ChallengeStatus.Visible) != 0)
                .Select(challenge => new ChallengesResponse.Challenge(challenge)).OrderByDescending(challenge => challenge.StartTime).Skip(pagination.Offset * pagination.Limit).Take(pagination.Limit).ToList();
        var totalCount = _context.Challenges.Count(challenge => (challenge.RunningStatus == RunningChallengeStatus.Frozen || challenge.RunningStatus == RunningChallengeStatus.Terminated) && (challenge.Status & ChallengeStatus.Visible) != 0);
        return Ok(new ChallengesResponse(challenges, totalCount));
    }

    [HttpGet]
    public ActionResult GetCache([FromQuery] string key)
    {
        var value = _cache.Get(key);
        if (value == null) return NotFound();
        return Ok(value);
    }

    [HttpGet]
    public ActionResult<ChallengeSetupResponse> ChallengeSetup([FromQuery] Guid challengeId)
    {
        var challenge = _context.Challenges.Where(challenge => challenge.ChallengeId == challengeId)
            .Select(ch => new ChallengeSetupResponse(ch)).FirstOrDefault();
        if (challenge == null) return NotFound();
        return Ok(challenge);
    }

    [HttpGet]
    public ActionResult SystemConfiguration() {
        var configSection = _configuration.GetSection("SystemConfiguration");
        var config = configSection.Get<SystemConfiguration>();
        return Ok(config);
    }
}