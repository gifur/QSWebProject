using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.Module.SharedAggregate;

namespace QS.DAL.EntityConfiguration
{
    class PhotoConfiguration : EntityTypeConfiguration<Photo>
    {
        public PhotoConfiguration()
        {
            this.HasKey(p => p.PhotoId);
            this.Property(p => p.PhotoName).HasMaxLength(255).IsOptional();
            this.Property(p => p.PhotoPath).HasMaxLength(255).IsRequired();
            this.Property(p => p.ThumbPath).HasMaxLength(255).IsRequired();
            this.Property(p => p.Remark).HasMaxLength(500).IsOptional();
            this.ToTable("Photo");
        }
    }
}
