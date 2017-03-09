using IHK.DB;
using IHK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IHK.Common;
using System.Linq;

namespace IHK.Repositorys
{
    public class UserRepository
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<User> _db_User;
        private readonly DbSet<Role> _db_Role;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _db_User = _dataContext.Set<User>();
            _db_Role = _dataContext.Set<Role>();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _db_User.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).ToListAsync();
        }

        public async Task<User> GetByUserName(string username)
        {
            return await _db_User.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetById(int id)
        {
            return await _db_User.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Role> GetRoleByType(UserRoleType role)
        {
            return await _db_Role.SingleOrDefaultAsync(r => r.UserRoleType == (UserRoleType)role);
        }

        public void AddRoleToUsers(RoleToUser rtu)
        {
            _dataContext.RoleToUsers.Add(rtu);
        }

        public  List<RoleToUser> GetManyRoleToUser(Func<RoleToUser, bool> p)
        {
            return _dataContext.RoleToUsers.Where(p).ToList();
        }

        public void RemoveRoleToUserRange(List<RoleToUser> rtus)
        {
            _dataContext.RoleToUsers.RemoveRange(rtus);
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }

        public async Task RemoveUserById(int id)
        {
            _db_User.Remove(await _db_User.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Id == id));
            _dataContext.SaveChanges();
        }

        public async Task ResetPassword(int id)
        {
            var ex = await _db_User.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Id == id);
            if (ex == null)
            {
                return;
            }
            else
            {
                ex.Password = "";
                _dataContext.SaveChanges();
            }
        }
    }
}
