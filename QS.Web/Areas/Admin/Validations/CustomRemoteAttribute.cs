using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QS.Web.Areas.Admin.Validations
{
    public class CustomRemoteAttribute : RemoteAttribute
    {
        private string action { get; set; }
        private string controller { get; set; }
        private string area { get; set; }

        public CustomRemoteAttribute(string action, string controller, string area)
            : base(action, controller, area)
        {
            this.action = action;
            this.controller = controller;
            this.area = area;
            
            this.RouteData["area"] = area;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var additionalFields = this.AdditionalFields.Split(',');
            var propValues = new List<object>();
            propValues.Add(value);
            foreach (string additionalField in additionalFields)
            {
                var prop = validationContext.ObjectType.GetProperty(additionalField);
                if (prop != null)
                {
                    object propValue = prop.GetValue(validationContext.ObjectInstance, null);
                    propValues.Add(propValue);
                }
            }

            var controllerType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(d => d.Name.ToLower() == (this.controller + "Controller").ToLower());
            if (controllerType != null)
            {
                var instance = Activator.CreateInstance(controllerType);

                var method = controllerType.GetMethod(this.action);

                if (method != null)
                {
                    ActionResult response = (ActionResult)method.Invoke(instance, propValues.ToArray());

                    if (response is JsonResult)
                    {
                        bool isAvailable = false;
                        JsonResult json = (JsonResult)response;
                        string jsonData = Convert.ToString(json.Data);

                        bool.TryParse(jsonData, out isAvailable);

                        if (!isAvailable)
                        {
                            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                        }
                    }
                }
            }
            return null;
        }

    }
}