using System.Web.Mvc;
using System.Web.Routing;

namespace QS.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Detail",
                url: "Admin/Feedback/{feedbackName}/{feedbackId}",
                defaults: new { controller = "Feedback", action = "Detail" },
                constraints: new { feedbackId = @"\d+" }
                );

            context.MapRoute(
                name: "User",
                url: "Admin/UserManage/{action}/{stuNumber}",
                defaults: new { controller = "UserManage", action = "DetailEdit" }
                );

            context.MapRoute(
                name: "Close",
                url: "Admin/Feedback/Close",
                defaults: new { controller = "Feedback", action = "Close", Area = "Admin" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
