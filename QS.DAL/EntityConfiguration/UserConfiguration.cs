using QS.Core.Module;
using System.Data.Entity.ModelConfiguration;

namespace QS.DAL.EntityConfiguration
{
    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.HasKey(u => u.UserId);
            this.Property(u => u.UserName).IsOptional().HasMaxLength(32);
            this.Property(u => u.Password).HasMaxLength(32).IsRequired();
            this.Property(u => u.RealName).HasMaxLength(32).IsRequired();
            this.Property(u => u.StuNumber).HasMaxLength(32).IsRequired();
            this.Property(u => u.Identification).HasMaxLength(64).IsRequired();
            this.Property(u => u.Gender).IsOptional();
            this.Property(u => u.Phone).IsOptional().HasMaxLength(32);
            this.Property(u => u.Email).IsOptional().HasMaxLength(64);
            this.Property(u => u.PhotoUrl).IsOptional().HasMaxLength(96);
            this.Property(u => u.About).IsOptional().HasMaxLength(140);
            this.Property(u => u.PersonalPage).IsOptional().HasMaxLength(64);
            this.Property(u => u.State).IsOptional();
            //configure table map
            this.ToTable("User");
        }
    }
}
