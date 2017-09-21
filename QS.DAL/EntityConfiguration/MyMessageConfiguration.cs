using QS.Core.Module;
using System.Data.Entity.ModelConfiguration;

namespace QS.DAL.EntityConfiguration
{
    class MyMessageConfiguration : EntityTypeConfiguration<MyMessage>
    {
        public MyMessageConfiguration()
        {
            HasKey(m => m.MyId);
            Property(m => m.Status).IsRequired();
            HasRequired(m => m.Message)
                .WithMany(m => m.MyMessages)
                .HasForeignKey(m => m.MId)
                .WillCascadeOnDelete(true);
            //configure table map
            this.ToTable("MyMessage");
        }
    }
}
