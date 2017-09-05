using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.Services.Entities
{
    public class T_User:BaseEntity
    {
        public string PhoneNum { get; set; }

        public string NickName { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public virtual ICollection<T_UserGroup> UserGroup { get; set; }
        = new List<T_UserGroup>();
    }
}
