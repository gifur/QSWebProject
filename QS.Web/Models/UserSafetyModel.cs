using QS.DTO.Module;

namespace QS.Web.Models
{
    public class UserSafetyModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string StuNumber { get; set; }
        public string PhotoUrl { get; set; }
        public GenderType Gender { get; set; }
        public UserState State { get; set; }
        public bool RememberMe { get; set; }
        public string Roles { get; set; }
    }
}
