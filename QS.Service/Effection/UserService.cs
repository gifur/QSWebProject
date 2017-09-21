using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using QS.Common;
using QS.Common.Validator;
using QS.Core.IRepository;
using QS.Core.Module;
using QS.DTO.Module;

namespace QS.Service.Effection
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public UserDto GetUserById(int id)
        {
            return QsMapper.CreateMap<User, UserDto>(_userRepository.Get(id));
        }

        public void AddUser(UserDto userDto)
        {
            User user = QsMapper.CreateMap<UserDto, User>(userDto);
            _userRepository.Add(user);
            _userRepository.UnitOfWork.Commit();
        }

        public QsResult DeleteUser(int id)
        {
            var result = new QsResult {Success = false};
            var currentUser = _userRepository.Get(id);
            if (currentUser == null)
            {
                result.Message = @"找不到对象";
                return result;
            }
            _userRepository.Remove(currentUser);
            _userRepository.UnitOfWork.Commit();
            result.Success = true;
            return result;
        }

        public QsResult CheckUserInLogin(string nameOrNumber, string password, bool isAdmin = false)
        {
            var result = new QsResult {Success = false};
            User model;
            if (isAdmin)
            {
                model =
                    _userRepository.FirstOrDefault(
                        user =>
                            user.UserName.Equals(nameOrNumber) &&
                            (user.Roles.Equals("Admin") || user.Roles.Equals("Editor")));
            }
            else
            {
                model = _userRepository.FirstOrDefault(user => user.UserName.Equals(nameOrNumber));
            }
            //后台的话就不允许使用学号登录
            if (!isAdmin && model == null && nameOrNumber.All(Char.IsNumber))
            {//如果用户不存在且用户名都是数字，则判断输入的是否为学号，根据学号找其数据

                model = _userRepository.FirstOrDefault(user => user.StuNumber.Equals(nameOrNumber));
                
                if (model == null)
                {
                    result.Message = @"该用户不存在";
                    return result;
                }
            }
            if (model == null)
            {
                result.Message = @"该用户不存在";
                return result;
            }
            if (model.Password.Equals(password))
            {
                result.Success = true;
                result.Message = model.UserId.ToString(CultureInfo.InvariantCulture);
                return result;
            }
            result.Message = @"密码输入错误";
            return result;
        }

        public OutMsg UpdateUserInformation(int id, UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentException(Message.warning_CannotAddUserWithNullInformation);

            var currentUser = _userRepository.Get(id);
            var updateUser = QsMapper.CreateMap<UserDto, User>(userDto);

            return this.UpdateUser(currentUser, updateUser);
        }

        public void UpdateUserInformation(UserDto userInformation)
        {
            if (userInformation == null) return;
            var currentUser = _userRepository.Get(userInformation.UserId);
            if (currentUser == null) return;
            var updatedUser = QsMapper.CreateMap<UserDto, User>(userInformation);
            _userRepository.Merge(currentUser, updatedUser);
            _userRepository.UnitOfWork.Commit();
        }

        OutMsg UpdateUser(User currentUser, User updatedUser)
        {
            var message = new OutMsg {Status = false, Msg = "修改信息失败"};
            var entityValidator = EntityValidatorFactory.CreateValidator();
            if (entityValidator.IsValid(updatedUser))
            {
                _userRepository.Merge(currentUser, updatedUser);
                _userRepository.UnitOfWork.Commit();
                message.Msg = "成功修改信息";
                message.Status = true;
                return message;
            }
            else
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(updatedUser));
        }

        public List<UserDto> FindUsers(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentNullException(Message.warning_InvalidArgumentForFindUsers);
            var users = _userRepository.GetPaged<string>(pageIndex, pageCount, o => o.StuNumber, false).ToList();
            if (!users.Any()) return new List<UserDto>();
            var lstUserDto = QsMapper.CreateMapList<User, UserDto>(users);
            //var lstUserDto = QsMapper.CreateMap<List<User>, List<UserDto>>(users);
            return lstUserDto;
        }

        public List<UserDto> GetAllUsers()
        {
            var users = _userRepository.GetAll().ToList();

            if (!users.Any()) return new List<UserDto>();
            var lstUserDto = QsMapper.CreateMapList<User, UserDto>(users);
            return lstUserDto;
        }

        public UserDto GetUserByStuNumber(string stuNumber)
        {
            if (String.IsNullOrWhiteSpace(stuNumber))
            {
                throw new ArgumentNullException(Message.warning_InvalidArgumentForFindUsers);
            }
            var currentUser = _userRepository.FirstOrDefault(e => e.StuNumber.Equals(stuNumber));
            return currentUser == null ? null : QsMapper.CreateMap<User, UserDto>(currentUser);
        }

        public UserDto GetUserFindPassword(string realName, string email)
        {
            if (string.IsNullOrEmpty(realName) || string.IsNullOrEmpty(email)) return null;
            var user = _userRepository.FirstOrDefault(m => m.RealName.Equals(realName) && m.Email.Equals(email));
            return QsMapper.CreateMap<User, UserDto>(user);
        }

        public OutMsg ExistsUserNickName(string name, int userId)
        {
            var message = new OutMsg
            {
                Status = false,
                Msg = @"不存在该用户"
            };
            var currentUser = _userRepository.FirstOrDefault(e => e.UserName.Equals(name));
            if (currentUser != null && currentUser.UserId != userId)
            {
                message.Status = true;
                message.Msg = @"该用户名已被占用";
            }

            return message;
        }

        public int RecordUserLogin(int usersId, string userName, string ip, string computerName, string platform, string useragent, bool isLogin = true, bool isFront = true)
        {
            var entrance = isLogin ? (isFront ? @"前台登录" : @"后台登录") : (isFront ? @"前台退出" : @"后台退出");
            return _userRepository.ExecuteCommand(String.Format(
                "INSERT INTO LoginLog([UserId], [UserName], [IP], [ComputerName], [LoginTime], [Platform], [UserAgent], [Type]) VALUES ({0}, '{1}', '{2}', '{3}', GETDATE(), '{4}', '{5}', '{6}')",
                usersId, userName, ip, computerName, platform, useragent, entrance));
        }
    }
}
