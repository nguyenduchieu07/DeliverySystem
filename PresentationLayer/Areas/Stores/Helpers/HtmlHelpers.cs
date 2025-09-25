using Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelpers
{
    public static string IsActive(this IHtmlHelper html,
                                  string controllers = null,
                                  string actions = null,
                                  string cssClass = "active")
    {
        var routeData = html.ViewContext.RouteData;

        var currentAction = routeData.Values["action"].ToString();
        var currentController = routeData.Values["controller"].ToString();

        if (string.IsNullOrEmpty(actions)) actions = currentAction;
        if (string.IsNullOrEmpty(controllers)) controllers = currentController;

        //var acceptedActions = actions.Split(',');
        var acceptedControllers = controllers.Split(',');

        return acceptedControllers.Contains(currentController) ? cssClass : string.Empty;
    }
}
