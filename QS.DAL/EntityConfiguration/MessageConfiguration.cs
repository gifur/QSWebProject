using QS.Core.Module;
using System.Data.Entity.ModelConfiguration;

namespace QS.DAL.EntityConfiguration
{
    class MessageConfiguration : EntityTypeConfiguration<Core.Module.Message>
    {
        public MessageConfiguration()
        {
            HasKey(m => m.MId);
            Property(m => m.Appendix).IsOptional().HasMaxLength(50);
            Property(m => m.Title).HasMaxLength(225).IsRequired();
            Property(m => m.Context).IsRequired();
            //configure table map
            this.ToTable("Message");
        }
    }
}
