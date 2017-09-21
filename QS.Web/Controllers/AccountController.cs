using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using QS.Web.Tools;
using Webdiyer.WebControls.Mvc;
using Insus.NET;
using log4net;
using QS.Common;
using QS.DTO.Module;
using QS.Service;
using QS.Web.Common;
using QS.Web.Models;

namespace QS.Web.Controllers
{
    public class AccountController : DefaultController
    {
        private static readonly ILog Log = LogManager.GetLogger("Loggering");

        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IMyMessageService _myMessageService;

        public AccountController(){}

        public AccountController(IUserService userService, IMessageService messageService, IMyMessageService myMessageService)
        {
            _userService = userService;
            _messageService = messageService;
            _myMessageService = myMessageService;
        }

        #region 登入登出

        //用户登陆
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.returnUrl = "";
            if (Request["ReturnUrl"] != null)
            {
                ViewBag.returnUrl = Request["ReturnUrl"];
            }
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            model.ValidateCode = model.ValidateCode.ToLower();
            if (Session["ValidateCode"].ToString() != model.ValidateCode)
            {
                ModelState.AddModelError("ValidateCode", @"验证码错误");
                model.Password = String.Empty;
                model.ValidateCode = String.Empty;
                return View(model);
            }
            var result = _userService.CheckUserInLogin(model.NameOrNumber, Utilities.MD5(model.Password));
            if (!result.Success)
            {
                ModelState.AddModelError(result.Message.Contains("用户") ? "NameOrNumber" : "Password", result.Message);
                model.Password = String.Empty;
                model.ValidateCode = String.Empty;
                return View(model);
            }
            var user = _userService.GetUserById(Convert.ToInt32(result.Message));
            var temp = QsMapper.CreateMap<UserDto, UserSafetyModel>(user);
            temp.RememberMe = model.RememberMe;
            //首先去除先前的登陆信息
            SafeOutAuthCookie();
            SetAuthCookie(temp);

            //记录入登录日志中

            var ip = QsRequest.GetIp();
            var computerName = QsRequest.GetDnsSafeHost();
            var platform = Request.Browser.Platform;
            var userAgent = Request.UserAgent;
            _userService.RecordUserLogin(temp.UserId, temp.UserName, ip, computerName, platform, userAgent);
            //结束记录

            if (user.State == UserState.Nonactivated)
            {
                return RedirectToAction("Confirmation");
            }
            //被限制要登录的页面会在url上带上上一访问的页面
            //FormsAuthentication.RedirectFromLoginPage(user.UserName, model.RememberMe);
            //return new EmptyResult();

            if (!String.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(HttpUtility.UrlDecode(model.ReturnUrl));
            }
            return RedirectToAction("Index", "Home");

        }

        public ActionResult GetValidateCode()
        {
            //var vCode = new ValidateCodeModel();
            //string code = vCode.CreateValidateCode(5);
            //Session["ValidateCode"] = code;
            //byte[] bytes = vCode.CreateValidateGraphic(code);
            //return File(bytes, @"image/jpeg");

            var validateCodeType = new ValidateCode2Model();
            string code;
            var bytes = validateCodeType.CreateImage(out code);
            Session["ValidateCode"] = code;

            return File(bytes, @"image/jpeg");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            var model = new ForgotPasswordModel();
            return View(model);
        }

        public ActionResult IdentificationError()
        {
            var model = new IdentificationErrorModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult IdentificationError(IdentificationErrorModel model)
        {
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _userService.GetUserFindPassword(model.RealName, model.Email);
            if (user == null)
            {
                ModelState.AddModelError("duplicate", @"信息无效，未能匹配相关用户");
                return View(model);
            }
            model.CreateTime = DateTime.Now;
            var mailBody = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/ForgotPasswordEmailTemplate.htm"));
            mailBody = mailBody.Replace("{{RealName}}", model.RealName);
            mailBody = mailBody.Replace("{{CreateTime}}", model.CreateTime.ToString(CultureInfo.InvariantCulture));
            mailBody = mailBody.Replace("{{Email}}", model.Email);
            mailBody = mailBody.Replace("{{Identification}}", user.Identification);
            mailBody = mailBody.Replace("{{Phone}}", user.Phone);
            try
            {
                QsMail.SendMail("smtp.163.com", "xxxxx@163.com", "password", "passwordSender",
                    "xxxxx@163.com", "to@domain.com", @"[求索工作室网站]用户密码找回请求", mailBody);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Content("<script>alert('已将在[忘记密码操作下]填写的信息发送至管理员邮箱，索子会尽快尽快进行密码重置并联系您');window.location='/Home/Index'</script>");
        }

        //注销
        public ActionResult LoginOut()
        {
            var temp = GetUserInCookie();
            SafeOutAuthCookie();
            //记录入登录日志中
            var ip = QsRequest.GetIp();
            var computerName = QsRequest.GetDnsSafeHost();
            var platform = Request.Browser.Platform;
            var userAgent = Request.UserAgent;
            _userService.RecordUserLogin(temp.UserId, temp.UserName, ip, computerName, platform, userAgent, false);
            //结束记录
            return RedirectToAction("Index", "Home");
        }

        #endregion

        [CustomAuthorize(Roles = "", Required = false)]
        public ActionResult Confirmation()
        {
            var userDto = _userService.GetUserById(GetUserInCookie().UserId);

            if (userDto.State == UserState.Activated || userDto.State == UserState.Retire)
            {
                var log = LogManager.GetLogger("用户相关");
                log.Info(String.Format("[编号：{0}]{1}尝试访问Comfirmation页面", userDto.UserId, userDto.UserName));
                return Content("<script type='text/javascript'>alert('抱歉，无阅读此页面的权限！');history.go(-1);</script>");
            }

            BindSelectListDataSource((int)userDto.Gender);

            //为页面验证消息而进行的处理
            var userAcnt = QsMapper.CreateMap<UserDto, AccountModel>(userDto);

            return View(userAcnt);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "", Required = false)]
        public ActionResult Confirmation(AccountModel model)
        {
            var userDto = CustomAuthorizeAttribute.GetUser();
            if (ModelState.IsValid)
            {
                var initUser = _userService.GetUserById(userDto.UserId);
                if (TryUpdateModel(initUser, null, null, new[] {"UserId", "RealName", "StuNumber", "Identification", "State", "PhotoUrl", "Roles"}))
                {
                    if (initUser.State == UserState.Activated || initUser.State == UserState.Retire)
                    {
                        ModelState.AddModelError("duplicate", @"用户的状态出现错误：" + initUser.State);
                    }
                    else
                    {
                        //将用户状态设为激活状态，此种情况下才能执行查看其他页面
                        initUser.State = UserState.Activated;
                        if (!String.IsNullOrEmpty(model.Password))
                        {
                            initUser.Password = Utilities.MD5(model.Password);
                        }
                        _userService.UpdateUserInformation(initUser);
                        SafeOutAuthCookie();
                        SetAuthCookie(QsMapper.CreateMap<UserDto, UserSafetyModel>(initUser));
                        return Content("<script>alert('填写成功！');window.location='/Account/ProfileDetail'</script>");
                    }
                }
            }
            BindSelectListDataSource((int)userDto.Gender);
            return View(model);
        }
       

        #region 个人中心

        [CustomAuthorize]
        [ChildActionOnly]
        public ActionResult _Personal()
        {
            var user = CustomAuthorizeAttribute.GetUser();
            BindSelectListDataSource((int)user.Gender);
            var result = _userService.GetUserById(user.UserId);
            return PartialView(QsMapper.CreateMap<UserDto, AcntWithoutPsw>(result));
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult Personal([Bind(Exclude = "RealName, StuNumber, Identification")]AcntWithoutPsw userAcnt)
        {
            var result = new QsResult {Success = false};
            if (ModelState.IsValid)
            {
                var userInCookie = CustomAuthorizeAttribute.GetUser();
                if (userAcnt.UserId != userInCookie.UserId)
                {
                    result.Message = @"请不要尝试修改您不允许改动的内容";
                    return Json(result);
                }
                var original = _userService.GetUserById(userInCookie.UserId);
                if (TryUpdateModel(original, null, null, new [] { "RealName", "StuNumber", "Identification","Roles"}))
                {
                    _userService.UpdateUserInformation(original);
                    SafeOutAuthCookie();
                    SetAuthCookie(QsMapper.CreateMap<UserDto, UserSafetyModel>(original));
                    result.Success = true;
                    return Json(result);
                }
            }
            return Json(result);
        }

        [CustomAuthorize]
        public ActionResult ProfileDetail()
        {
            var user = CustomAuthorizeAttribute.GetUser();
            ViewBag.Photo = user.PhotoUrl;
            return View();
        }

        [HttpPost]
        [CustomAuthorize]
        public ActionResult ChangeImage()
        {
            var result = new QsResult {Success = false};
            var user = _userService.GetUserById(GetUserInCookie().UserId);
            var file = Request.Files["ImageInput"];
            if (file != null && file.ContentLength > 0)
            {
                var fileName = file.FileName;
                var fileExtension = fileName.Substring(fileName.IndexOf('.'), fileName.Length - fileName.IndexOf('.'));
                if (!String.IsNullOrWhiteSpace(user.PhotoUrl))
                {
                    if (!user.PhotoUrl.Equals("no-image.png"))
                    {
                        var deleteFileName = Server.MapPath("~/Profiles/HeadImage/" + user.PhotoUrl);
                        System.IO.File.Delete(deleteFileName);
                    }
                    //物理存储头像
                    result.Message = user.PhotoUrl = user.StuNumber + "_" + DateTime.Now.Ticks + fileExtension;
                    file.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Profiles/HeadImage"), user.PhotoUrl));
                    result.Message = "/Profiles/HeadImage/" + result.Message;
                    //修改用户信息

                    _userService.UpdateUserInformation(user);
                    //更新Cookie中用户的信息
                    SafeOutAuthCookie();
                    user = _userService.GetUserById(user.UserId);
                    SetAuthCookie(QsMapper.CreateMap<UserDto, UserSafetyModel>(user));
                    //更新完成
                    result.Success = true;
                    return Json(result);
                }
            }
            result.Message = @"请选择想要上传的头像";
            ModelState.AddModelError("PhotoUrl", @"请选择想要上传的头像");
            return Json(result);

        }

        public ActionResult _ChangePassword()
        {
            var model = new ProfileChangePassword();
            return PartialView(model);
        }
        [HttpPost]
        [CustomAuthorize]
        public ActionResult ChangePassword(ProfileChangePassword model)
        {
            model.Trim();
            var result = new QsResult {Success = false};
            if (!ModelState.IsValid)
            {
                return Json(result);
            }
            var user = _userService.GetUserById(GetUserInCookie().UserId);
            if (!user.Password.Equals(Utilities.MD5(model.CurrentPassword)))
            {//验证原始密码
                result.Message = @"原始密码输入错误";
                ModelState.AddModelError("CurrentPassword", result.Message);
                return Json(result);
            }
            if (model.CurrentPassword.Equals(model.NewPassword))
            {//判断到新旧密码一致
                result.Message = @"新旧密码一致，未进行修改操作";
                return Json(result);
            }
            user.Password = Utilities.MD5(model.NewPassword);
            _userService.UpdateUserInformation(user);
            SafeOutAuthCookie();
            result.Success = true;
            return Json(result);
        }

        #endregion

        [CustomAuthorize]
        public ActionResult Message(string type = "unread", int pageIndex = 1)
        {
            var user = CustomAuthorizeAttribute.GetUser();
            IEnumerable<MyMessageDto> result;
            var temp = (List<MessageDto>) _messageService.GetAllMessages();
            IList<MessageDto> models = new List<MessageDto>();
            if (type.Equals("unread") || !type.Equals("read"))
            {
                result = _myMessageService.GetMyMessagesWithStatus(user.UserId, false);
                ViewBag.Type = "unread";
                if (result == null) return View((new List<MessageDto>()).ToPagedList(pageIndex, 5));
                foreach (var item in result)
                {
                    var model = temp.Find(it => it.MId == item.MId);
                    model.Appendix = item.MyId.ToString(CultureInfo.InvariantCulture);
                    models.Add(model);
                }
            }
            else
            {
                result = _myMessageService.GetMyMessagesWithStatus(user.UserId, true);
                ViewBag.Type = "read";
                if (result == null) return View((new List<MessageDto>()).ToPagedList(pageIndex, 5));
                foreach (var item in result)
                {
                    var model = temp.Find(it => it.MId == item.MId);
                    model.Appendix = item.MyId.ToString(CultureInfo.InvariantCulture);
                    models.Add(model);
                }
            }
            return View(models.ToPagedList(pageIndex, 5));
        }

        [HttpPost]
        public ActionResult _NewMessageNum()
        {
            var user = CustomAuthorizeAttribute.GetUser();
            var newNum = _myMessageService.GetUnreadMessage(user.UserId);
            return Json(new {num = newNum});
        }

        [HttpPost]
        public ActionResult Delete(Int64 id, bool type = true)
        {
            var user = CustomAuthorizeAttribute.GetUser();
            var model = _myMessageService.GetMyMessageById(id);
            if (type)
            {
                _myMessageService.DeleteMyMessage(id);
                return Content("true");
            }
            model.Status = true;
            var temp = _myMessageService.ChangeMyMessageDescription(id, model);
            return Content(temp ? "true" : "false");
        }

        

        [HttpGet]
        public JsonResult IsUserNameAvailable(string userName)
        {
            var user = CustomAuthorizeAttribute.GetUser();
            var message = _userService.ExistsUserNickName(userName, user.UserId);
            return !message.Status ? Json(true, JsonRequestBehavior.AllowGet) : Json("该用户名已存在", JsonRequestBehavior.AllowGet);
        }

        #region 私有方法

        /// <summary>
        /// 绑定页面上的数据
        /// </summary>
        /// <param name="selectedIndex"></param>
        private void BindSelectListDataSource(int selectedIndex)
        {
            var gender = new List<SelectListItem>
                {
                    (new SelectListItem {Text = @"男", Value = "0", Selected = false}),
                    (new SelectListItem {Text = @"女", Value = "1", Selected = false}),
                    (new SelectListItem {Text = @"保密", Value = "2", Selected = false})
                };
            gender[selectedIndex].Selected = true;
            ViewData["gender"] = gender;
        }

        #endregion
    }
}
