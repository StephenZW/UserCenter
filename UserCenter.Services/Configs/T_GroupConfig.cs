using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Entities;

namespace UserCenter.Services.Configs
{
    public class T_GroupConfig:EntityTypeConfiguration<T_Group>
    {
        public T_GroupConfig()
        {
            this.ToTable(nameof(T_Group) + "s");
            this.Property(p => p.Name).HasMaxLength(32).IsRequired();
        }
    }
}
