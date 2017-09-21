using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.DTO.Module;

namespace QS.Manager.IManager
{
    public interface IUserManager
    {
        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        List<UserDto> FindUsers(int pageIndex, int pageCount);

        /// <summary>
        /// 通过用户ID查找用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserDto FindUserById(int id);

        /// <summary>
        /// 通过用户ID删除用户
        /// </summary>
        /// <param name="userId"></param>
        void DeleteUser(int userId);

        /// <summary>
        /// 添加新的用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        void SaveUserInformation(UserDto userDto);

        /// <summary>
        /// 修改现存的用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        void UpdateUserInformation(int id, UserDto userDto);
    }
}
