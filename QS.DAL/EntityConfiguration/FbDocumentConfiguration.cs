using System.Data.Entity.ModelConfiguration;
using QS.Core.Module.FeedbackAggregate;

namespace QS.DAL.EntityConfiguration
{
    class FbDocumentConfiguration : EntityTypeConfiguration<FbDocument>
    {
        public FbDocumentConfiguration()
        {
            HasKey(fd => fd.DocumentId);
            //1 ... *
            HasRequired(fd => fd.Feedback)
                .WithMany(fd => fd.FbDocuments)
                .HasForeignKey(fd => fd.FeedbackId)
                .WillCascadeOnDelete(false);

            //1 ... *
            HasRequired(fd => fd.Uploader)
                .WithMany(fd => fd.FbDocuments)
                .HasForeignKey(fd => fd.UploaderId)
                .WillCascadeOnDelete(false);

            ToTable("FbDocument");
        }
    }
}
