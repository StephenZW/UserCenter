using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Entities;

namespace UserCenter.Services.Configs
{
    public class T_UserGroupConfig:EntityTypeConfiguration<T_UserGroup>
    {
        public T_UserGroupConfig()
        {
            this.HasRequired(ug => ug.User).WithMany(u => u.UserGroup).HasForeignKey(ug => ug.UserId).WillCascadeOnDelete(false);

            this.HasRequired(ug => ug.Group).WithMany(g => g.UserGroup).HasForeignKey(ug => ug.GroupId).WillCascadeOnDelete(false);
        }
    }
}
