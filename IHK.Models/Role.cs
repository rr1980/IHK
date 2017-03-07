using IHK.Common;
using System.Collections.Generic;

namespace IHK.Models
{
    public class Role : BaseModel
    {
        public UserRoleType UserRoleType { get; set; }
        public virtual ICollection<RoleToUser> RoleToUsers { get; set; }
    }
}

