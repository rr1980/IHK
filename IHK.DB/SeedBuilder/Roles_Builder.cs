using IHK.Common;
using IHK.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.DB.SeedBuilder
{
    public static class Roles_Builder
    {
        internal static void Create(DataContext context)
        {
            var urts = (UserRoleType[])Enum.GetValues(typeof(UserRoleType));

            foreach (var urt in urts)
            {
                var r = new Role();
                r.UserRoleType = urt;
                context.Roles.Add(r);
            }

            context.SaveChanges();
        }
    }
}
