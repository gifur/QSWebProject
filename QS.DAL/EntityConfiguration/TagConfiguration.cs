using QS.Core.Module;
using System.Data.Entity.ModelConfiguration;

namespace QS.DAL.EntityConfiguration
{
    class TagConfiguration : EntityTypeConfiguration<Tag>
    {
        public TagConfiguration()
        {
            HasKey(t => t.TagId);
            Property(t => t.TagName).HasMaxLength(50).IsRequired();
            Property(t => t.TagDescription).HasMaxLength(500).IsOptional();
            ToTable("Tag");
        }
    }
}
