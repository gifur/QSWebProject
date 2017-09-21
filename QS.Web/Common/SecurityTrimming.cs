using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace QS.Web.Common
{
    /// <summary>
    /// refrence:http://stackoverflow.com/questions/2721869/security-aware-action-link
    /// time:2014-9-17
    /// </summary>
    public static class SecurityTrimmingExtensions
    {

        public static bool HasActionPermission(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            //if the controller name is empty the ASP.NET convention is:
            //"we are linking to a different controller
            ControllerBase controllerToLinkTo = string.IsNullOrEmpty(controllerName)
                                                    ? htmlHelper.ViewContext.Controller
                                                    : GetControllerByName(htmlHelper, controllerName);

            var controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerToLinkTo);

            var controllerDescriptor = new ReflectedControllerDescriptor(controllerToLinkTo.GetType());

            var actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);

            return ActionIsAuthorized(controllerContext, actionDescriptor);
        }


        private static bool ActionIsAuthorized(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
                return false; // action does not exist so say yes - should we authorise this?!

            var authContext = new AuthorizationContext(controllerContext);

            // run each auth filter until on fails
            // performance could be improved by some caching
            foreach (var filter in FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor))
            {
                var authFilter = filter.Instance as IAuthorizationFilter;
                if (authFilter != null) authFilter.OnAuthorization(authContext);

                if (authContext.Result != null)
                    return false;
            }

            return true;
        }

        private static ControllerBase GetControllerByName(HtmlHelper helper, string controllerName)
        {
            // Instantiate the controller and call Execute
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();

            IController controller = factory.CreateController(helper.ViewContext.RequestContext, controllerName);

            if (controller == null)
            {
                throw new InvalidOperationException(

                    String.Format(
                        CultureInfo.CurrentUICulture,
                        "Controller factory {0} controller {1} returned null",
                        factory.GetType(),
                        controllerName));

            }

            return (ControllerBase)controller;
        }

    }

    public static class SecurityTrimmedLink
    {
        public static MvcHtmlString SecurityTrimmedActionLink(this HtmlHelper htmlHelper, string linkName, string actionName)
        {
            return htmlHelper.HasActionPermission(actionName, "")
                       ? htmlHelper.ActionLink(linkName, actionName)
                       : MvcHtmlString.Create("");
        }

        public static MvcHtmlString SecurityTrimmedActionLink(this HtmlHelper htmlHelper, string linkName, string actionName, RouteValueDictionary routeValueDictionary)
        {
            return htmlHelper.HasActionPermission(actionName, "")
                       ? htmlHelper.ActionLink(linkName, actionName, routeValueDictionary)
                       : MvcHtmlString.Create("");
        }

        public static MvcHtmlString SecurityTrimmedActionLink(this HtmlHelper htmlHelper, string linkName, string actionName, object routeValues, object htmlAttributes)
        {
            return htmlHelper.HasActionPermission(actionName, "")
                       ? htmlHelper.ActionLink(linkName, actionName, routeValues, htmlAttributes)
                       : MvcHtmlString.Create("");
        }

        public static MvcHtmlString SecurityTrimmedActionLink(this HtmlHelper htmlHelper, string linkName, string actionName, string controllerName)
        {
            return htmlHelper.HasActionPermission(actionName, controllerName)
                       ? htmlHelper.ActionLink(linkName, actionName, controllerName)
                       : MvcHtmlString.Create("");
        }
    }
}