using System.Collections.Generic;
using QS.DTO.FeedbackModule;

namespace QS.DTO.Module
{
    public enum GenderType
    {
        /// <summary>
        /// 男
        /// </summary>
        Male = 0,

        /// <summary>
        /// 女
        /// </summary>
        Female = 1,

        /// <summary>
        /// 保密
        /// </summary>
        Security = 2
    }

    public enum UserState
    {
        /// <summary>
        /// 未激活
        /// </summary>
        Nonactivated = 0,
        /// <summary>
        /// 激活
        /// </summary>
        Activated = 1,
        /// <summary>
        /// 退休
        /// </summary>
        Retire = 2
    }
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string StuNumber { get; set; }
        public string Identification { get; set; }
        public GenderType Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string About { get; set; }
        public string PersonalPage { get; set; }
        public UserState State { get; set; }
        public string Roles { get; set; }

        public List<FbDocumentDto> FbDocumentDtos { get; set; }
    }
}
