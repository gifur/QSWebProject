using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace QS.Web.Models
{
    public class IdentificationErrorModel
    {
        [Required(ErrorMessage = @"请输入描述内容")]
        [MaxLength(1000, ErrorMessage=@"输入的描述内容超过字数限制的1000字符长度")]
        public string Description { get; set; }
    }
}