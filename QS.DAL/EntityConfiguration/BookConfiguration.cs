using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QS.Core.Module.SharedAggregate;

namespace QS.DAL.EntityConfiguration
{
    class BookConfiguration : EntityTypeConfiguration<Book>
    {
        public BookConfiguration()
        {
            HasKey(b => b.BookId);
            Property(b => b.BookName).HasMaxLength(50).IsRequired();
            ToTable("Book");
        }
    }
}
