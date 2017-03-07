using IHK.Common;
using IHK.Models;
using System.Linq;

namespace IHK.DB.SeedBuilder
{
    public static class Users_Builder
    {
        internal static void Create(DataContext context, UserRoleType[] roles, User user)
        {
            //user.Roles = context.Roles.Where(r => roles.Contains(r.UserRoleType)).ToList();

            foreach (var role in roles)
            {
                var rtu = new RoleToUser();

                var ro = context.Roles.First(r => r.UserRoleType == role);

                rtu.Role = ro;
                rtu.User = user;

                context.RoleToUsers.Add(rtu);
            }

            context.SaveChanges();
        }
    }
}
