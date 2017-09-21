using System;
using System.Globalization;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.Module;
using QS.Service;
using QS.Web.Areas.Admin.Models;

namespace QS.Web.Areas.Admin.Controllers
{
    public class UserManageController : BaseController
    {
        private readonly IUserService _userService;

        public UserManageController(){}

        public UserManageController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        public ActionResult DetailEdit(int? uid, string stuNumber, string message)
        {
            UserDto user = null;
            if (uid.HasValue)
            {
                 user = _userService.GetUserById((int)uid);
            }
            else if(!String.IsNullOrWhiteSpace(stuNumber))
            {
                user = _userService.GetUserByStuNumber(stuNumber);
            }
            if (!String.IsNullOrWhiteSpace(message))
            {
                ViewData["Message"] = message;
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult SaveEdit(UserDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.State == UserState.Nonactivated)
                {
                    model = InitUserDtoInfor(model);
                }
                var msg = _userService.UpdateUserInformation(model.UserId, model);
                var user = _userService.GetUserById(model.UserId);
                if (msg.Status)
                {
                    return RedirectToAction("DetailEdit", new { stuNumber = user.StuNumber, message = "修改成功" });
                }
                return RedirectToAction("DetailEdit", new { stuNumber = user.StuNumber, message = "修改失败" });
            }
            return View("DetailEdit", model);
        }

        public ActionResult Create()
        {
            var viewModel = new UserCreateModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(UserCreateModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            if (_userService.GetUserByStuNumber(model.StuNumber) != null)
            {
                ModelState.AddModelError("StuNumber", @"该学号已存在");
                return View(model);
            }
            var userDto = QsMapper.CreateMap<UserCreateModel, UserDto>(model);
            userDto = InitUserDtoInfor(userDto);
            _userService.AddUser(userDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult IsStuNumberAvailable(string stuNumber)
        {
            if (_userService.GetUserByStuNumber(stuNumber) == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            var temp = String.Format(CultureInfo.InvariantCulture,
                "{0} 已存在", stuNumber);

            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ResetPassword(int id)
        {
            var result = new QsResult();
            if (Request.IsAjaxRequest())
            {
                var model = _userService.GetUserById(id);
                if (model == null)
                {
                    result.Success = false;
                    result.Message = @"找不到对象";
                    return Json(result);
                }
                model.Password = Utilities.MD5(model.StuNumber);
                _userService.UpdateUserInformation(model);
                return Json(result);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!Request.IsAjaxRequest()) return RedirectToAction("Index");
            var result = _userService.DeleteUser(id);
            return Json(result);
        }


        private static UserDto InitUserDtoInfor(UserDto model)
        {
            model.UserName = model.StuNumber;
            model.Gender = GenderType.Security;
            model.Phone = model.Email = String.Empty;
            model.Password = Utilities.MD5(model.StuNumber);
            model.PersonalPage = "华农的某个角落";
            model.About = "这人太懒了，什么都没留下…… ";
            model.PhotoUrl = "no-image.png";
            model.Roles = "Normal";
            return model;
        }
    }
}
