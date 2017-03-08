using IHK.Common;
using Microsoft.AspNetCore.Authorization;

namespace IHK.Web.Authorization
{
    public class AuthPolicyRequirement : IAuthorizationRequirement
    {
        public UserRoleType UserRoleType;

        public AuthPolicyRequirement(UserRoleType type)
        {
            this.UserRoleType = type;
        }
    }
}
