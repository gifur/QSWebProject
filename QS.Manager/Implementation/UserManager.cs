using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.Common.Logging;
using QS.Common.Validator;
using QS.Core.Module.UserAggregate;
using QS.DTO.Module;
using QS.Manager.IManager;
using QS.Repository.Module;

namespace QS.Manager.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly UserRepository _userRepository;

        public UserManager(UserRepository userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException("userRepository");
            _userRepository = userRepository;
        }

        public List<UserDto> FindUsers(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
                throw new ArgumentNullException(Message.warning_InvalidArgumentForFindUsers);
            var users = _userRepository.GetPaged<string>(pageIndex, pageCount, o => o.StuNumber, false);
            if (users != null && users.Any())
            {
                List<UserDto> lstUserDto = new List<UserDto>();
                foreach (var usr in users)
                {
                    lstUserDto.Add(Conversion.Mapping.UserToUserDto(usr));
                }
                return lstUserDto;
            }
            else
                return new List<UserDto>();
        }

        public void DeleteUser(int UserId)
        {
            var user = _userRepository.Get(UserId);
            if (user != null)
            {
                _userRepository.Remove(user);
                //提交确认
                _userRepository.UnitOfWork.Commit();
            }
            else
                LoggerFactory.CreateLog().LogWarning(Message.warning_CannotRemoveNonExistingUser);
        }

        public UserDto FindUserById(int id)
        {
            var usr = _userRepository.Get(id);
            if (usr != null)
            {
                return Conversion.Mapping.UserToUserDto(usr);
            }
            else
                return new UserDto();
        }

        public void SaveUserInformation(UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentException(Message.warning_CannotAddUserWithNullInformation);
            var newUser = UserFactory.CreateUser(userDto.UserId, userDto.RealName, userDto.StuNumber, userDto.Identification,
                userDto.UserName, userDto.Password, userDto.Gender, userDto.Phone, userDto.Email, userDto.PhotoUrl,
                userDto.About, userDto.PersonalPage, userDto.State);

            newUser = SaveUser(newUser);
        }

        public void UpdateUserInformation(int id, UserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentException(Message.warning_CannotAddUserWithNullInformation);

            var currentUser = _userRepository.Get(id);
            var updateUser = new User
            {
                UserId = id,
                UserName = userDto.UserName,
                Password = userDto.Password,
                Identification = userDto.Identification,
                StuNumber = userDto.StuNumber,
                RealName = userDto.RealName,
                Gender = userDto.Gender,
                Phone = userDto.Phone,
                Email = userDto.Email,
                PhotoUrl = userDto.PhotoUrl,
                About = userDto.About,
                PersonalPage = userDto.PersonalPage,
                State = userDto.State
            };

            updateUser = this.UpdateUser(currentUser, updateUser);
        }

        User SaveUser(User usr)
        {
            var entityValidator = EntityValidatorFactory.CreateValidator();
            if (entityValidator.IsValid(usr))
            {
                _userRepository.Add(usr);
                _userRepository.UnitOfWork.Commit();
                return usr;
            }
            else
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(usr));
        }

        OutMsg UpdateUser(User currentUser, User updatedUser)
        {
            var message = new OutMsg();
            message.Status = false;
            var entityValidator = EntityValidatorFactory.CreateValidator();
            if (entityValidator.IsValid(updatedUser))
            {
                _userRepository.Merge(currentUser, updatedUser);
                _userRepository.UnitOfWork.Commit();
                message.Status = true;
                message.Msg = "已成功修改信息";
                return message;
            }
            else
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(updatedUser));
        }

    }
}
