using KubbAdminAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KubbAdminAPI.Controllers;

public class BaseController : Controller
{

    protected Login? CurrentUserLoginNullable()
    {
        return RouteData.Values["login"] as Login;
    }
    
    protected User CurrentUser()
    {
        return (RouteData.Values["login"] as Login)!.User;
    }

    protected Login CurrentUserLogin()
    {
        return (RouteData.Values["login"] as Login)!;
    }

}
