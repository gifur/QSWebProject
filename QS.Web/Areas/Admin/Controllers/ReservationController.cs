using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QS.Common;
using QS.DTO.ProfessionModule;
using QS.Service;
using QS.Web.Areas.Admin.Models;
using Webdiyer.WebControls.Mvc;

namespace QS.Web.Areas.Admin.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly IReservationService _reservationService;

        public ReservationController() { }

        public ReservationController(IReservationService service)
        {
            _reservationService = service;
        }

        public ActionResult Index(int plit = -1, int id = 1, string keyWord = null)
        {
            StateNumBindView();
            var query = plit == -1 ? _reservationService.GetAllReservations() : _reservationService.GetReservationsWithState(plit);
            var temp = QsMapper.CreateMapIEnume<ReservationDto, ReservationViewModel>(query);
            if (!String.IsNullOrWhiteSpace(keyWord))
                temp = temp.Where(ft => ft.SubscriberName.Contains(keyWord) || ft.StuNumber.Contains(keyWord));
            var model = temp.OrderByDescending(it => it.Createtime).ToPagedList(id, 8);
            return View(model);
        }

        public ActionResult Detail(int rid)
        {
            var model = _reservationService.GetReservationiById(rid);
            return View(model);
        }

        public ActionResult Finish(int rid)
        {
            var model = _reservationService.GetReservationiById(rid);
            var res = _reservationService.AlterReservationState(rid, (int)DealState.Treated);
            if (res.Status)
            {
                ViewData["Message"] = @"操作成功";
                model = _reservationService.GetReservationiById(rid);
                return View("Detail", model);
            }
            ViewData["Message"] = @"操作失败";
            return View("Detail", model);
        }

        public ActionResult Delete(int rid)
        {
            _reservationService.DeleteReservation(rid);
            ViewData["Message"] = @"删除成功";
            return RedirectToAction("Index");
        }

        #region 私有方法

        private void StateNumBindView()
        {
            var temp = _reservationService.GetAllReservations();
            ViewBag.StateNum = new ReservationStateNumModel(temp);
        }
        #endregion

    }
}
