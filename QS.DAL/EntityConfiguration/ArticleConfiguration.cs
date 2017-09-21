using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.Module.SharedAggregate;

namespace QS.DAL.EntityConfiguration
{
    class ArticleConfiguration : EntityTypeConfiguration<Article>
    {
        public ArticleConfiguration()
        {
            this.HasKey(n => n.ArticleId);
            this.Property(n => n.ArticleTitle).HasMaxLength(100).IsRequired();
            this.Property(n => n.ArticleContent).IsRequired();
            this.ToTable("Article");
        }
    }
}
