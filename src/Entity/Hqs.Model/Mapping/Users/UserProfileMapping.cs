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
            builder.Property(p => p.CTime).HasComputedColumnSql("Ctime").IsRequired().HasColumnType("datetime2");
            builder.Property(p => p.MTime).HasComputedColumnSql("MTime").IsRequired().HasColumnType("datetime2");
            builder.Property(p => p.CreateBy).HasComputedColumnSql("CreateBy").IsRequired().HasColumnType("varchar").HasMaxLength(32);
            builder.Property(p => p.ModifyBy).HasComputedColumnSql("ModifyBy").IsRequired().HasColumnType("varchar").HasMaxLength(32);
            builder.Property(p => p.Name).HasComputedColumnSql("Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.NickName).HasComputedColumnSql("NickName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.Mobilephone).HasComputedColumnSql("Mobilephone").IsRequired().HasColumnType("varchar").HasMaxLength(11);
            builder.Property(p => p.Email).HasComputedColumnSql("Email").IsRequired().HasColumnType("varchar").HasMaxLength(30);
            builder.Property(p => p.Country).HasComputedColumnSql("Country").IsRequired(false).HasColumnType("varchar").HasMaxLength(10);
            builder.Property(p => p.Province).HasComputedColumnSql("Province").IsRequired(false).HasColumnType("varchar").HasMaxLength(10);
            builder.Property(p => p.City).HasComputedColumnSql("City").IsRequired(false).HasColumnType("varchar").HasMaxLength(10);
            builder.Property(p => p.HeadImgUrl).HasComputedColumnSql("HeadImgUrl").IsRequired(false).HasColumnType("varchar").HasMaxLength(32);
            builder.Property(p => p.Gender).HasComputedColumnSql("Gender").IsRequired().HasColumnType("int");
            builder.Property(p => p.Status).HasComputedColumnSql("Status").IsRequired().HasColumnType("int");
        }
    }
}
