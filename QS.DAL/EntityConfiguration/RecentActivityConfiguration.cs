using System.Data.Entity.ModelConfiguration;
using QS.Core.Module;

namespace QS.DAL.EntityConfiguration
{
    class RecentActivityConfiguration : EntityTypeConfiguration<RecentActivity>
    {
        public RecentActivityConfiguration()
        {
            this.HasKey(n => n.Id);
            this.Property(n => n.Title).HasMaxLength(100).IsRequired();
            this.Property(n => n.Content).HasMaxLength(1000).IsRequired();
            this.ToTable("RecentActivity");
        }
    }
}
