using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.Module.CommentAggregate;

namespace QS.DAL.EntityConfiguration
{
    class ArticleCommentConfiguration : EntityTypeConfiguration<ArticleComment>
    {
        public ArticleCommentConfiguration()
        {
            HasKey(nc => nc.CommentId);
            Property(nc => nc.Email).HasMaxLength(64);
            Property(nc => nc.NickName).HasMaxLength(32);
            Property(nc => nc.Content).HasMaxLength(1024).IsRequired();
            ToTable("ArticleComment");
        }
    }
}
