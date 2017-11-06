using System.Web;
using System.Web.Mvc;

public class SessionExpireFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        HttpContext ctx = HttpContext.Current;

        // check if session is supported
        if (HttpContext.Current.Session["User"] == null)
        {
            // check if a new session id was generated
            filterContext.Result = new RedirectResult("~/Account/Login");
            return;
        }

        base.OnActionExecuting(filterContext);
    }
}