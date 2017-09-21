using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.DTO.Module;
using QS.Service;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Areas.Admin.Controllers
{
    public class InforManageController : BaseController
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        public InforManageController() { }

        public InforManageController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public ActionResult Index(int id = 1)
        {
            int count;
            const int pageSize = 8;
            var result = _messageService.GetAllMessages();
            var model = result.ToPagedList(id, pageSize);
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new MessageDto();
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(MessageDto model)
        {
            if (!ModelState.IsValid) return View(model);
            _messageService.AddMessage(model);
            return RedirectToAction("Index");
        }

        public ActionResult Detail(Int64 id)
        {
            var model = _messageService.GetMessageById(id);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detail(MessageDto model)
        {
            if (!ModelState.IsValid) return View(model);
            model.EditTime = DateTime.Now;
            _messageService.ChangeMessageDescription(model.MId, model);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Delete(Int64 id)
        {
            _messageService.DeleteMessage(id);
            return Content("true");
        }
    }
}
