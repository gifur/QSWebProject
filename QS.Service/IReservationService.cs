using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;
using QS.DTO.ProfessionModule;

namespace QS.Service
{
    public interface IReservationService
    {
        void AddReservation(ReservationDto reservationDto);
        void DeleteReservation(int reservationId);
        ReservationDto GetReservationiById(int id);
        List<ReservationDto> GetReservations(int pageIndex, int pageCount);
        IEnumerable<ReservationDto> GetAllReservations();
        IEnumerable<ReservationDto> GetReservationsWithState(int state);
        OutMsg AlterReservationState(int id, int state);
    }
}
