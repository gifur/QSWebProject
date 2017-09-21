using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QS.Web.Areas.Admin.Validations
{
    public class RemoteAttributeAdapter : DataAnnotationsModelValidator<RemoteStuNumberAttribute>
    {

        public RemoteAttributeAdapter(ModelMetadata metadata, ControllerContext context,
            RemoteStuNumberAttribute attribute) :
            base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                // Use the default DataAnnotationsModelValidator error message.
                // This error message will be overridden by the string returned by
                // IsUID_Available unless "FAIL"  or "OK" is returned in 
                // the Aim Controller.
                ErrorMessage = ErrorMessage,
                ValidationType = "remoteVal"
            };

            rule.ValidationParameters["url"] = GetUrl();
            rule.ValidationParameters["parameterName"] = Attribute.ParameterName;
            return new ModelClientValidationRule[] { rule };
        }

        private string GetUrl()
        {
            var rvd = new RouteValueDictionary() {
                { "controller", Attribute.Controller },
                { "action", Attribute.Action }
            };

            var virtualPath = RouteTable.Routes.GetVirtualPath(ControllerContext.RequestContext,
                Attribute.RouteName, rvd);
            if (virtualPath == null)
            {
                throw new InvalidOperationException("没有符合的路由");
            }

            return virtualPath.VirtualPath;
        }
    } 
}