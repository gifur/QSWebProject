using System;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.Module;
using QS.Service;
using QS.Web.Models;
using QS.Web.Tools;

namespace QS.Web.Areas.Admin.Controllers
{
    public class OAuthController : BaseController
    {
        private readonly IUserService _userService;
        public OAuthController(){}
        public OAuthController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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
            var result = _userService.CheckUserInLogin(model.NameOrNumber, Utilities.MD5(model.Password), true);
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
            //记录入登录日志中
            var ip = QsRequest.GetIp();
            var computerName = QsRequest.GetHost();
            var platform = Request.Browser.Platform;
            var userAgent = Request.UserAgent;
            _userService.RecordUserLogin(temp.UserId, temp.UserName, ip, computerName, platform, userAgent, true, false);
            //结束记录
            SetAuthCookie(temp);
            if (user.State == UserState.Nonactivated)
            {
                ModelState.AddModelError("NameOrNumber", @"该用户状态出现异常，请联系管理员");
            }

            if (!String.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(HttpUtility.UrlDecode(model.ReturnUrl));
            }
            return RedirectToAction("Index", "OAuth");
        }
        public ActionResult LoginOut()
        {
            var temp = GetUserInCookie();
            SafeOutAuthCookie();
            //记录入登录日志中
            var ip = QsRequest.GetIp();
            var computerName = QsRequest.GetHost();
            var platform = Request.Browser.Platform;
            var userAgent = Request.UserAgent;
            _userService.RecordUserLogin(temp.UserId, temp.UserName, ip, computerName, platform, userAgent, false, false);
            //结束记录
            return RedirectToAction("Login", "OAuth");
        }

        public ActionResult _UserProfile()
        {
            var model = GetUserInCookie();
            return PartialView(model);
        }
        [AllowAnonymous]
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
    }
}
