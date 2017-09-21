using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.Module.SharedAggregate;

namespace QS.DAL.EntityConfiguration
{
    class NewsConfiguration : EntityTypeConfiguration<News>
    {
        public NewsConfiguration()
        {
            this.HasKey(n => n.NewsId);
            this.Property(n => n.NewsTitle).HasMaxLength(100).IsRequired();
            this.Property(n => n.NewsContent).IsRequired();
            this.ToTable("News");
        }
    }
}
