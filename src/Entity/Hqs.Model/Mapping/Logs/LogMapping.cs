using System;
using Hqs.Model.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hqs.Model.Mapping.Logs
{
    public class LogMapping :  IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("LogApi");
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().HasColumnType("int");
            builder.Property(p => p.CTime).HasColumnName("CTime").IsRequired().HasColumnType("datetime2");
            builder.Property(p => p.CreateBy).HasColumnName("CreateBy").IsRequired().HasColumnType("varchar").HasMaxLength(32);
            builder.Property(p => p.AccessUrl).HasColumnName("AccessUrl").IsRequired().HasColumnType("varchar").HasMaxLength(1024);
            builder.Property(p => p.LogType).HasColumnName("LogType").IsRequired().HasColumnType("int");
            builder.Property(p => p.LogTypeName).HasColumnName("LogTypeName").IsRequired().HasColumnType("nvarchar").HasMaxLength(20);
            builder.Property(p => p.ModuleName).HasColumnName("ModuleName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.ActionName).HasColumnName("ActionName").IsRequired().HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(p => p.IpAddress).HasColumnName("IpAddress").IsRequired().HasColumnType("varchar").HasMaxLength(30);
            builder.Property(p => p.Description).HasColumnName("Description").IsRequired().HasColumnType("nvarchar").HasMaxLength(Int32.MaxValue);
            builder.Property(p => p.BeforeChange).HasColumnName("BeforeChange").IsRequired(false).HasColumnType("nvarchar").HasMaxLength(Int32.MaxValue);
            builder.Property(p => p.AfterChange).HasColumnName("AfterChange").IsRequired(false).HasColumnType("nvarchar").HasMaxLength(Int32.MaxValue);
            builder.Property(p => p.Remarks).HasColumnName("Remarks").IsRequired(false).HasColumnType("nvarchar").HasMaxLength(Int32.MaxValue);
        }
    }
}
