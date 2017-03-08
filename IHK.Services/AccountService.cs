using IHK.Models;
using IHK.Repositorys;
using IHK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHK.Services
{
    public class AccountService
    {
        private readonly UserRepository _userRepository;

        public AccountService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserViewModel> GetByUserName(string username)
        {
            User user = await _userRepository.GetByUserName(username);
            return _map(user);
        }

        private UserViewModel _map(User user)
        {
            if (user != null)
            {
                return new UserViewModel()
                {
                    UserId = user.Id,
                    Anrede = user.Anrede,
                    Username = user.Username,
                    ShowName = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                    Postleitzahl = user.Postleitzahl,
                    Stadt = user.Stadt,
                    Strasse = user.Strasse,
                    Telefon = user.Telefon,
                    Email = user.Email,
                    Roles = user.RoleToUsers.Select(r => r.Role).Select(r => (int)r.UserRoleType),
                    LayoutThemeViewModel = new LayoutThemeViewModel()
                    {
                        Id = user.LayoutTheme.Id,
                        Name = user.LayoutTheme.Name,
                        Link = user.LayoutTheme.Link
                    }
                };
            }
            else
            {
                return default(UserViewModel);
            }
        }
    }
}
