using System.Data.Entity.ModelConfiguration;
using QS.Core.Module.FeedbackAggregate;

namespace QS.DAL.EntityConfiguration
{
    class FeedbackConfiguration : EntityTypeConfiguration<Feedback>
    {
        public FeedbackConfiguration()
        {
            HasKey(f => f.FeedbackId);
            Property(f => f.FeedbackName).HasMaxLength(32).IsRequired();
            ToTable("Feedback");
        }
    }
}
