using System.Data.Entity.ModelConfiguration;
using QS.Core.Module.ProfessionAggregate;

namespace QS.DAL.EntityConfiguration
{
    class ReservationConfiguration : EntityTypeConfiguration<Reservation>
    {
        public ReservationConfiguration()
        {
            this.HasKey(rt => rt.RId);
            this.Property(rt => rt.SubscriberName).HasMaxLength(32).IsRequired();
            this.Property(rt => rt.StuNumber).HasMaxLength(32).IsRequired();
            this.Property(rt => rt.Phone).HasMaxLength(32).IsRequired();
            this.Property(rt => rt.Dealtime).IsRequired();
            this.Property(rt => rt.Professional).HasMaxLength(64).IsRequired();

            this.Property(rt => rt.Email).IsOptional().HasMaxLength(64);
            this.Property(rt => rt.Past).IsOptional().HasMaxLength(128);
            this.Property(rt => rt.Experience).IsOptional().HasMaxLength(128);
            this.Property(rt => rt.Situation).IsOptional().HasMaxLength(2000);

            this.ToTable("Reservation");
        }
    }
}
