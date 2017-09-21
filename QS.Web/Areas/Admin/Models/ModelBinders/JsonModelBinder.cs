using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QS.Web.Areas.Admin.Models.ModelBinders
{
    public class JsonModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var json = string.Empty;

            var provider = bindingContext.ValueProvider;

            var providerValue = provider.GetValue(bindingContext.ModelName);

            if (providerValue != null)
                json = providerValue.AttemptedValue;

            // 基本表达式确保字符串以JSON对象（{}）或数组字符（[]）表示
            if (Regex.IsMatch(json, @"^(\[.*\]|{.*})$"))
            {
                return new JavaScriptSerializer().Deserialize(json, bindingContext.ModelType);
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum |
                 AttributeTargets.Interface | AttributeTargets.Parameter |
                 AttributeTargets.Struct | AttributeTargets.Property,
                 AllowMultiple = false, Inherited = false)]
    public class JsonModelBinderAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new JsonModelBinder();
        }
    }

    public class JsonBinder<TEntity> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //从请求中获取提交的参数数据
            var json = controllerContext.HttpContext.Request.Form[0] as string;
            //提交参数是对象
            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                var jsonBody = JObject.Parse(json);
                var js = new JsonSerializer();
                var obj = js.Deserialize(jsonBody.CreateReader(), typeof(TEntity));
                var list = new List<TEntity> { (TEntity)obj };
                return list;

            }
            return null;
        }
    }
}