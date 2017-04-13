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
    public class AccountService: IAccountService
    {
        private readonly UserRepository _userRepository;
        private readonly OptionRepository _optionRepository;

        public AccountService(UserRepository userRepository, OptionRepository optionRepository)
        {
            _userRepository = userRepository;
            _optionRepository = optionRepository;
        }

        public async Task AddOrUpdate(IUserItemViewModel user)
        {
            var ex = await _userRepository.GetById(user.UserId);
            if (ex == null)
            {
                Adresse adr = await _userRepository.GetAdresse(user.Postleitzahl, user.Stadt, user.Strasse, user.Hausnummer);
                if (adr == null)
                {
                    adr = new Adresse()
                    {
                        Postleitzahl = user.Postleitzahl,
                        Stadt = user.Stadt,
                        Strasse = user.Strasse,
                        Hausnummer = user.Hausnummer
                    };
                }


                var usr = new User()
                {
                    Anrede = (int)user.Anrede,
                    Username = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                    Adresse = adr,
                    Telefon = user.Telefon,
                    Email = user.Email,
                    //LayoutTheme = await _context.LayoutThemes.SingleOrDefaultAsync(lt => lt.Name == "default")
                    LayoutTheme = await _optionRepository.GetLayoutThemeByName("default")
                };

                List<RoleToUser> rtus = new List<RoleToUser>();

                if (user.Roles != null)
                {
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
                }
                usr.RoleToUsers = rtus;
            }
            else
            {
                _delRoles(ex);

                Adresse adr = await _userRepository.GetAdresse(user.Postleitzahl, user.Stadt, user.Strasse, user.Hausnummer);
                if (adr == null)
                {
                    adr = new Adresse()
                    {
                        Postleitzahl = user.Postleitzahl,
                        Stadt = user.Stadt,
                        Strasse = user.Strasse,
                        Hausnummer = user.Hausnummer
                    };
                }

                ex.Anrede = user.Anrede;
                ex.Name = user.Name;
                ex.Vorname = user.Vorname;
                ex.Username = user.Username;
                //ex.LayoutTheme = await _optionRepository.LayoutThemes.SingleOrDefaultAsync(lt => lt.Id == user.LayoutThemeViewModel.Id);
                ex.LayoutTheme = await _optionRepository.GetLayoutThemeById(user.LayoutThemeViewModel.Id);
                ex.Password = user.Password;
                ex.Adresse = adr;
                ex.Telefon = user.Telefon;
                ex.Email = user.Email;

                List<RoleToUser> rtus = new List<RoleToUser>();

                if (user.Roles != null)
                {
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

        public async Task<List<IUserItemViewModel>> GetAllUsers()
        {
            List<User> users = await _userRepository.GetAllUsers();
            return users.Select(u => _map(u)).ToList();
        }

        public async Task<IUserItemViewModel> GetByUserName(string username)
        {
            User user = await _userRepository.GetByUserName(username);
            return _map(user);
        }


        public async Task<IUserItemViewModel> GetById(int id)
        {
            User user = await _userRepository.GetById(id);
            return _map(user);
        }

        public async Task<bool> HasRole(int id, UserRoleType urt)
        {
            User user = await _userRepository.GetById(id);
            return user.RoleToUsers.Any(rtu => rtu.Role.UserRoleType == urt);
        }

        private IUserItemViewModel _map(User user)
        {
            if (user != null)
            {
                var default_theme = _optionRepository.GetLayoutThemeByName("default").Result;

                return new UserItemViewModel()
                {
                    UserId = user.Id,
                    Anrede = user.Anrede,
                    Username = user.Username,
                    ShowName = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                    Postleitzahl = user.Adresse.Postleitzahl,
                    Stadt = user.Adresse.Stadt,
                    Strasse = user.Adresse.Strasse,
                    Hausnummer = user.Adresse.Hausnummer,
                    Telefon = user.Telefon,
                    Email = user.Email,
                    Roles = user.RoleToUsers.Select(r => r.Role).Select(r => (int)r.UserRoleType),
                    LayoutThemeViewModel = new LayoutThemeItemViewModel()
                    {
                        Id = user.LayoutTheme?.Id ?? default_theme.Id,
                        Name = user.LayoutTheme?.Name ?? default_theme.Name,
                        Link = user.LayoutTheme?.Link ?? default_theme.Link,
                    }
                };
            }
            else
            {
                return default(UserItemViewModel);
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
