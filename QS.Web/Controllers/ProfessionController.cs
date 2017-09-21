using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.ProfessionModule;
using QS.Service;
using QS.Web.Models;

namespace QS.Web.Controllers
{
    public class ProfessionController : Controller
    {
        private readonly IReservationService _reservationService;

        public ProfessionController() { }

        public ProfessionController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        //
        // GET: /Profession/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Preregistration()
        {
            var model = new ReserveModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Preregistration(ReserveModel model)
        {
            if (ModelState.IsValid)
            {
                _reservationService.AddReservation(QsMapper.CreateMap<ReserveModel, ReservationDto>(model));
                TempData["Message"] = "您的预约信息已成功提交，索子会在2到3个工作日内处理，接下来将电话邀约核实，期间请保持通讯工具畅通";
                return RedirectToAction("Preregistration");
            }
            return View(model);
        }

    }
}
