using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace KubbAdminAPI.Filters;

public class AuthenticationFilter : ActionFilterAttribute, IActionFilter
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var dbContext = context.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();
        
        var loginId = context.HttpContext.Request.Headers["X-AuthorizationId"];
        var loginToken = context.HttpContext.Request.Headers["X-AuthorizationToken"];

        if (loginId == string.Empty || loginToken == string.Empty)
        {
            context.Result = new BadRequestObjectResult("Missing Authorization header");
            return;
        }
        
        var login = dbContext.Logins.Include(login => login.User).FirstOrDefault(login => login.LoginId == loginId);

        if (login == null || !login.ValidateToken(loginToken!))
        {
            context.Result = new UnauthorizedObjectResult("Invalid login");
            return;
        }
        
        login.Expiration = DateTime.UtcNow.AddMinutes(30);
        
        context.RouteData.Values["login"] = login;
        
        base.OnActionExecuting(context);
    }
    
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);
    }
}