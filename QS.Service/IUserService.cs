
using System.Collections.Generic;
using QS.Common;
using QS.DTO.Module;

namespace QS.Service
{
    /// <summary>
    /// 接口允许你完全不用考虑数据库，促成更好的单元测试。
    /// </summary>
    public interface IUserService
    {
        UserDto GetUserById(int id);
        void AddUser(UserDto userDto);
        QsResult DeleteUser(int id);
        /// <summary>
        /// 修改现存的用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        OutMsg UpdateUserInformation(int id, UserDto userDto);
        void UpdateUserInformation(UserDto userInformation);

        QsResult CheckUserInLogin(string nameOrNumber, string password, bool isAdmin = false);
        List<UserDto> FindUsers(int pageIndex, int pageCount);

        List<UserDto> GetAllUsers();

        UserDto GetUserByStuNumber(string stuNumber);
        UserDto GetUserFindPassword(string realName, string email);

        OutMsg ExistsUserNickName(string name, int userId);
        int RecordUserLogin(int usersId, string userName, string ip, string computerName, string platform, string useragent, bool isLogin = true, bool isFront = true);
    }
}
