using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHK.Common
{
    public interface IUserItemViewModel
    {
        int UserId { get; set; }
        int Anrede { get; set; }

        string Name { get; set; }
        string Vorname { get; set; }

        string Username { get; set; }
        string ShowName { get; set; }
        string Password { get; set; }
        string Postleitzahl { get; set; }
        string Stadt { get; set; }
        string Strasse { get; set; }
        string Hausnummer { get; set; }
        string Telefon { get; set; }
        string Email { get; set; }

        ILayoutThemeItemViewModel LayoutThemeViewModel { get; set; }
        IEnumerable<int> Roles { get; set; }
    }

    public interface ILayoutThemeItemViewModel
    {
        int Id { get; set; }
        string Name { get; set; }
        string Link { get; set; }
    }

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
