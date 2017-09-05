using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.Services.Entities
{
    public class T_Group:BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<T_UserGroup> UserGroup { get; set; }
      = new List<T_UserGroup>();
    }
}
