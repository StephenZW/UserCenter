using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Entities;

namespace UserCenter.Services.Configs
{
    public class T_UserConfig:EntityTypeConfiguration<T_User>
    {
        public T_UserConfig()
        {
            this.Property(p => p.PhoneNum).HasMaxLength(16).IsRequired();

            this.Property(p => p.NickName).HasMaxLength(64).IsRequired();
            this.Property(p => p.PasswordHash).HasMaxLength(256).IsUnicode(false).IsRequired();

            this.Property(p => p.PasswordSalt).IsRequired().HasMaxLength(32).IsUnicode(false);


        }
    }
}
