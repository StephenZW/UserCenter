using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.Services.Entities
{
    public class T_UserGroup:BaseEntity
    {
        public long UserId { get; set; }

        public long GroupId { get; set; }

        public virtual T_User User { get; set; }

        public virtual T_Group Group { get; set; }


    }
}
