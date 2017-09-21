using System;
using System.Collections.Generic;
using QS.Common;
using QS.Core.IRepository;
using QS.Core.Module.ProfessionAggregate;
using QS.DTO.ProfessionModule;

namespace QS.Service.Effection
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService() { }

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public void AddReservation(ReservationDto reservationDto)
        {
            reservationDto.Createtime = DateTime.Now;
            _repository.Add(QsMapper.CreateMap<ReservationDto, Reservation>(reservationDto));
            _repository.UnitOfWork.Commit();
        }

        public void DeleteReservation(int reservationId)
        {
            var temp = _repository.Get(reservationId);
            if (temp != null)
            {
                _repository.Remove(temp);
                _repository.UnitOfWork.Commit();
            }
        }

        public ReservationDto GetReservationiById(int id)
        {
            return QsMapper.CreateMap<Reservation, ReservationDto>(_repository.Get(id));
        }

        public List<ReservationDto> GetReservations(int pageIndex, int pageCount)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReservationDto> GetAllReservations()
        {
            var temp = _repository.GetAllWithOrder(filter => filter.Createtime);
            if (temp == null) return null;
            return QsMapper.CreateMapIEnume<Reservation, ReservationDto>(temp);
        }

        public IEnumerable<ReservationDto> GetReservationsWithState(int state)
        {
            var temp = _repository.GetFiltered(ft => ft.State == state);
            if (temp == null) return null;
            return QsMapper.CreateMapIEnume<Reservation, ReservationDto>(temp);
        }

        public OutMsg AlterReservationState(int id, int state)
        {
            var message = new OutMsg { Status = false, Msg = "修改信息失败" };
            var current = _repository.Get(id);
            if (current == null)
            {
                message.Msg = string.Format("找不到编号为{0}的对象", id);
                return message;
            }
            var change = new Reservation(current) {State = state};
            _repository.Merge(current, change);
            _repository.UnitOfWork.Commit();
            message.Status = true;
            return message;

        }
    }
}
