using KubbAdminAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace KubbAdminAPI.Filters;

public class AdministratorFilter : ActionFilterAttribute, IActionFilter
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var login = context.RouteData.Values["login"] as Login;
        var user = login!.User;

        if ((user.Status & UserStatus.Administrator) == 0) {
            context.Result = new BadRequestObjectResult("You are not an administrator");
            return;
        }

        base.OnActionExecuting(context);
    }
    
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);
    }
}