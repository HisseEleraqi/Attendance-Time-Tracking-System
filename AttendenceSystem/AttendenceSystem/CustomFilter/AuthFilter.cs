using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace AttendenceSystem.CustomFilter
{
    public class AuthFilter:ActionFilterAttribute
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if(!filterContext.HttpContext.User.Identity.IsAuthenticated) 
            {
                filterContext.Result = new RedirectToActionResult("AccessError", "Account",null);
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated==false)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }

    }
}
