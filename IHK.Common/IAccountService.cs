using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHK.Common
{
    public interface IAccountService
    {
        Task<IUserItemViewModel> GetById(int id);
        Task<List<IUserItemViewModel>> GetAllUsers();
        Task AddOrUpdate(IUserItemViewModel user);
        Task ResetPassword(int id);
        Task RemoveUserById(int id);
        Task<IUserItemViewModel> GetByUserName(string username);
        Task<bool> HasRole(int id, UserRoleType urt);
    }
}
