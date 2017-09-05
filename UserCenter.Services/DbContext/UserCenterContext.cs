using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Entities;

namespace UserCenter.Services
{
    public class UserCenterContext:DbContext
    {
        public UserCenterContext():base("default")
        {

        }

        public DbSet<T_User> Users { get; set; }
        public DbSet<T_AppInfo> AppInfos { get; set; }
        public DbSet<T_Group> Groups { get; set; }
        public DbSet<T_UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
