using IHK.Common;
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
        private readonly OptionRepository _optionRepository;

        public AccountService(UserRepository userRepository, OptionRepository optionRepository)
        {
            _userRepository = userRepository;
            _optionRepository = optionRepository;
        }
        
        public async Task AddOrUpdate(UserViewModel user)
        {
            var ex = await _userRepository.GetById(user.UserId);
            if (ex == null)
            {
                var usr = new User()
                {
                    Anrede = (int)user.Anrede,
                    Username = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                    Postleitzahl = user.Postleitzahl,
                    Stadt = user.Stadt,
                    Strasse = user.Strasse,
                    Telefon = user.Telefon,
                    Email = user.Email,
                    //LayoutTheme = await _context.LayoutThemes.SingleOrDefaultAsync(lt => lt.Name == "default")
                    LayoutTheme = await _optionRepository.GetLayoutThemeByName("default")
                };

                List<RoleToUser> rtus = new List<RoleToUser>();
                foreach (var role in user.Roles)
                {
                    var _rtu = new RoleToUser()
                    {
                        //Role = role != -1 ? _context.Roles.First(r => r.UserRoleType == (UserRoleType)role) : _context.Roles.First(r => r.UserRoleType == UserRoleType.Default),
                        Role = role != -1 ? await _userRepository.GetRoleByType((UserRoleType)role) : await _userRepository.GetRoleByType(UserRoleType.Default),
                        User = usr
                    };
                    rtus.Add(_rtu);
                    _userRepository.AddRoleToUsers(_rtu);
                }
                usr.RoleToUsers = rtus;
            }
            else
            {
                _delRoles(ex);
                ex.Anrede = user.Anrede;
                ex.Name = user.Name;
                ex.Vorname = user.Vorname;
                ex.Username = user.Username;
                //ex.LayoutTheme = await _optionRepository.LayoutThemes.SingleOrDefaultAsync(lt => lt.Id == user.LayoutThemeViewModel.Id);
                ex.LayoutTheme = await _optionRepository.GetLayoutThemeById(user.LayoutThemeViewModel.Id);
                ex.Password = user.Password;
                ex.Postleitzahl = user.Postleitzahl;
                ex.Stadt = user.Stadt;
                ex.Strasse = user.Strasse;
                ex.Telefon = user.Telefon;
                ex.Email = user.Email;

                List<RoleToUser> rtus = new List<RoleToUser>();
                foreach (var role in user.Roles)
                {
                    var _rtu = new RoleToUser()
                    {
                        Role = role != -1 ? await _userRepository.GetRoleByType((UserRoleType)role) : await _userRepository.GetRoleByType(UserRoleType.Default),
                        User = ex
                    };
                    rtus.Add(_rtu);
                    _userRepository.AddRoleToUsers(_rtu);
                }
                ex.RoleToUsers = rtus;
            }

            try
            {
                _userRepository.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error...:");
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task ResetPassword(int id)
        {
            await _userRepository.ResetPassword(id);
        }

        public async Task RemoveUserById(int id)
        {
            await _userRepository.RemoveUserById(id);
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            List<User> users = await _userRepository.GetAllUsers();
            return  users.Select(u => _map(u)).ToList();
        }

        public async Task<UserViewModel> GetByUserName(string username)
        {
            User user = await _userRepository.GetByUserName(username);
            return _map(user);
        }


        public async Task<UserViewModel> GetById(int id)
        {
            User user = await _userRepository.GetById(id);
            return _map(user);
        }

        public async Task<bool> HasRole(int id, UserRoleType urt)
        {
            User user = await _userRepository.GetById(id);
            return user.RoleToUsers.Any(rtu => rtu.Role.UserRoleType == urt);
        }

        private UserViewModel _map(User user)
        {
            if (user != null)
            {
                var default_theme = _optionRepository.GetLayoutThemeByName("default").Result;

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
                        Id = user.LayoutTheme?.Id ?? default_theme.Id,
                        Name = user.LayoutTheme?.Name ?? default_theme.Name,
                        Link = user.LayoutTheme?.Link ?? default_theme.Link,
                    }
                };
            }
            else
            {
                return default(UserViewModel);
            }
        }

        private User _delRoles(User user)
        {
            var rtus = _userRepository.GetManyRoleToUser(rtu => rtu.UserId == user.Id);
            _userRepository.RemoveRoleToUserRange(rtus);
            _userRepository.SaveChanges();
            return user;
        }
    }
}
