using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

public static class MvcExtensions
{
    public static string ActiveClass(this IHtmlHelper htmlHelper, string controllers = null, string actions = null, string cssClass = "active")
    {
        var currentController = htmlHelper?.ViewContext.RouteData.Values["controller"] as string;
        var currentAction = htmlHelper?.ViewContext.RouteData.Values["action"] as string;

        var acceptedControllers = (controllers ?? currentController ?? "").Split(',');
        var acceptedActions = (actions ?? currentAction ?? "").Split(',');

        return acceptedControllers.Contains(currentController) && acceptedActions.Contains(currentAction)
            ? cssClass
            : "";
    }
}