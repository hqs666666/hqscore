using Hqs.Model.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hqs.Model.Mapping.Users
{
    public class UserProfileMapping : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfile");
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().HasColumnType("varchar").HasMaxLength(32);
            builder.Property(p => p.CTime).HasColumnName("Ctime").IsRequired().HasColumnType("datetime2");
            builder.Property(p => p.MTime).HasColumnName("MTime").IsRequired().HasColumnType("datetime2");
            builder.Property(p => p.CreateBy).HasColumnName("CreateBy").IsRequired().HasColumnType("varchar").HasMaxLength(32);
            builder.Property(p => p.ModifyBy).HasColumnName("ModifyBy").IsRequired().HasColumnType("varchar").HasMaxLength(32);
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.NickName).HasColumnName("NickName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.Mobilephone).HasColumnName("Mobilephone").IsRequired().HasColumnType("varchar").HasMaxLength(11);
            builder.Property(p => p.Email).HasColumnName("Email").IsRequired().HasColumnType("varchar").HasMaxLength(30);
            builder.Property(p => p.Country).HasColumnName("Country").IsRequired(false).HasColumnType("varchar").HasMaxLength(10);
            builder.Property(p => p.Province).HasColumnName("Province").IsRequired(false).HasColumnType("varchar").HasMaxLength(10);
            builder.Property(p => p.City).HasColumnName("City").IsRequired(false).HasColumnType("varchar").HasMaxLength(10);
            builder.Property(p => p.HeadImgUrl).HasColumnName("HeadImgUrl").IsRequired(false).HasColumnType("varchar").HasMaxLength(1024);
            builder.Property(p => p.Gender).HasColumnName("Gender").IsRequired().HasColumnType("int");
            builder.Property(p => p.Status).HasColumnName("Status").IsRequired().HasColumnType("int");
        }
    }
}
