using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.IRepository;
using QS.Core.Module.ProfessionAggregate;
using QS.DAL;

namespace QS.Repository.Module
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }
    }
}
