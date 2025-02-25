using KubbAdminAPI.Models;
using KubbAdminAPI.Models.Cache;
using KubbAdminAPI.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace KubbAdminAPI.Controllers;

[ApiController, Route("[controller]/[action]")]
public class HomeController(DatabaseContext _context, IMemoryCache _cache) : BaseController
{
    [HttpGet]
    public ActionResult Challenges([FromQuery] Pagination pagination)
    {
        if (pagination.Limit > 100) return BadRequest("Limit can't be more than 100");
        var challenges =
            _context.Challenges.Where(challenge => challenge.RunningStatus == RunningChallengeStatus.Submitted)
                .Select(
                    item => new
                    {
                        item.ChallengeId,
                        item.Name,
                        item.StartTime,
                        item.EndTime,
                        item.Status
                    }).Skip(pagination.Offset * pagination.Limit).Take(pagination.Limit).ToList();
        return Json(challenges);
    }

    [HttpGet]
    public ActionResult ArchivedChallenges([FromQuery] Pagination pagination)
    {
        if (pagination.Limit > 100) return BadRequest("Limit can't be more than 100");
        var challenges =
            _context.Challenges.Where(challenge =>
                    challenge.RunningStatus == RunningChallengeStatus.Terminated ||
                    challenge.RunningStatus == RunningChallengeStatus.Frozen)
                .Select(
                    item => new
                    {
                        item.ChallengeId,
                        item.Name,
                        item.StartTime,
                        item.EndTime
                    }).Skip(pagination.Offset * pagination.Limit).Take(pagination.Limit).ToList();
        return Json(challenges);
    }

    [HttpGet]
    public ActionResult GetCache([FromQuery] string key)
    {
        var value = _cache.Get(key);
        if (value == null) return NotFound();
        return Ok(value);
    }
    
    #if DEBUG
    [HttpGet]
    public ActionResult SetCache([FromQuery] string key)
    {
	var c = "ciao";
	for (int i = 0; i < 10000; i += 1) c += "c";
        _cache.Set(key, c);
        return Ok();
    }
    #endif
}
