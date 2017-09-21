using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Common;

namespace QS.Core.Module.ProfessionAggregate
{
    public sealed partial class Reservation : Entity, IValidatableObject
    {
        public Reservation() { }
        public Reservation(Reservation current)
        {
            this.RId = current.RId;
            this.SubscriberName = current.SubscriberName;
            this.StuNumber = current.StuNumber;
            this.Gender = current.Gender;
            this.Age = current.Age;
            this.Professional = current.Professional;
            this.Phone = current.Phone;
            this.Email = current.Email;
            this.Past = current.Past;
            this.Experience = current.Experience;
            this.Dealtime = current.Dealtime;
            this.Situation = current.Situation;
            this.Createtime = current.Createtime;
            this.State = current.State;
        }

        [Key]
        public int RId { get; set; }
        public string SubscriberName { get; set; }
        public string StuNumber { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string Professional { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Past { get; set; }
        public string Experience { get; set; }
        public DateTime Dealtime { get; set; }
        public string Situation { get; set; }
        public DateTime Createtime { get; set; }
        public int State { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            return validationResults;
        }
    }
}
