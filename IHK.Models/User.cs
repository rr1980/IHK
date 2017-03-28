using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.Models
{
    public class User : Person
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Adresse Adresse { get; set; }
        public virtual LayoutTheme LayoutTheme { get; set; }
        public virtual ICollection<RoleToUser> RoleToUsers { get; set; }
    }
}

