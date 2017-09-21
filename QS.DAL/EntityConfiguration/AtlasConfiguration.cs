using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.Module.SharedAggregate;

namespace QS.DAL.EntityConfiguration
{
    class AtlasConfiguration : EntityTypeConfiguration<Atlas>
    {
        public AtlasConfiguration()
        {
            this.HasKey(p => p.AtlasId);
            this.Property(p => p.AtlasName).HasMaxLength(255).IsOptional();
            this.Property(p => p.AtlasPath).HasMaxLength(255).IsOptional();
            this.Property(p => p.ThumbPath).HasMaxLength(255).IsOptional();
            this.Property(p => p.Remark).HasMaxLength(500).IsOptional();
            this.ToTable("Atlas");
        }
    }
}
