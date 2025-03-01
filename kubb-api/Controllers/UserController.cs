using KubbAdminAPI.Filters;
using KubbAdminAPI.Models.RequestModels.User;
using KubbAdminAPI.Models.ResponseModels.User;
using Microsoft.AspNetCore.Mvc;

namespace KubbAdminAPI.Controllers;

[ApiController, Route("[controller]/[action]"), AuthenticationFilter]
public class UserController : BaseController
{

    [HttpGet]
    public ActionResult GetProfile()
    {

        var user = CurrentUser()!;

        return Ok(new GetProfileResponse
        {
            Name = user.Name,
            Surname = user.Surname,
            EmailAddress = user.EmailAddress,
            Status = user.Status
        });

    }
    
    
}