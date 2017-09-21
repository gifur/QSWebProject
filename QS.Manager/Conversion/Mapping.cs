using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.Module.UserAggregate;
using QS.DTO.Module;

namespace QS.Manager.Conversion
{
    public static class Mapping
    {
        public static UserDto UserToUserDto(User user)
        {
            UserDto objUserDto = new UserDto();
            objUserDto.UserId = user.UserId;
            objUserDto.UserName = user.UserName;
            objUserDto.Password = user.Password;
            objUserDto.RealName = user.RealName;
            objUserDto.StuNumber = user.StuNumber;
            objUserDto.Identification = user.Identification;
            objUserDto.Gender = user.Gender;
            objUserDto.Phone = user.Phone;
            objUserDto.Email = user.Email;
            objUserDto.PhotoUrl = user.PhotoUrl;
            objUserDto.About = user.About;
            objUserDto.PersonalPage = user.PersonalPage;
            objUserDto.State = user.State;
            return objUserDto;
        }
    }
}
