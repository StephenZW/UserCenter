using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Entities;

namespace UserCenter.Services.Configs
{
    public class T_AppInfoConfig:EntityTypeConfiguration<T_AppInfo>
    {
        public T_AppInfoConfig()
        {
            this.HasKey(t => t.Id);
            this.Property(p => p.Name).HasMaxLength(128).IsRequired();

            this.Property(p => p.AppKey).HasMaxLength(256).IsUnicode(false).IsRequired();

            this.Property(p => p.AppSecret).HasMaxLength(256).IsUnicode(false).IsRequired();

            this.Property(p => p.CreateDate).HasColumnType("datetime").IsRequired();

        }
    }
}
