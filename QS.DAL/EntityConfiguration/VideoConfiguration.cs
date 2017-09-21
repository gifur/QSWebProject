using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.Module.SharedAggregate;

namespace QS.DAL.EntityConfiguration
{
    class VideoConfiguration : EntityTypeConfiguration<Video>
    {
        public VideoConfiguration()
        {
            this.HasKey(v => v.VideoId);
            this.Property(v => v.VideoName).HasMaxLength(50).IsRequired();
            this.Property(v => v.Remark).HasMaxLength(500).IsOptional();
            this.Property(v => v.ComesFrom).HasMaxLength(100).IsRequired();
            this.ToTable("Video");
        }
    }
}
