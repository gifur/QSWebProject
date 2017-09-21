using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QS.DTO.FeedbackModule;

namespace QS.DTO.Module
{
    public enum TagBelongType
    {
        All = 0,
        News,
        Article,
        Gallery,
        Video,
        Book

    }

    public class TagDto
    {
        public int TagId { get; set; }
        [Required(ErrorMessage = @"请填写标签名称")]
        [StringLength(100, ErrorMessage = @"标签名称必须至少包含 {2} 个字符。", MinimumLength = 2)]
        public string TagName { get; set; }
        [Range(0, 2, ErrorMessage = @"请选择所属类别")]
        public TagBelongType Belong { get; set; }
        [Required(ErrorMessage = @"请填写标签搜索时所需的英文名称")]
        public string TagEnglish { get; set; }
        [MaxLength(500, ErrorMessage=@"已超过500字符字数限制")]
        public string TagDescription { get; set; }
        public Int64 TagSum { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
