using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QS.Web.Areas.Admin.Validations
{
    public sealed class RemoteStuNumberAttribute : ValidationAttribute
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string ParameterName { get; set; }
        public string RouteName { get; set; }

        //如果该验证特性仅用于远程验证并且仅在创建对象时（而不是在编辑对象时）使用，
        //则重写的 IsValid 应返回 true。
        public override bool IsValid(object value)
        {
            return true;
        }
    }
}