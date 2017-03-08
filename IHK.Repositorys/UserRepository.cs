using IHK.DB;
using IHK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHK.Repositorys
{
    public class UserRepository
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<User> _db;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _db = _dataContext.Set<User>();
        }

        public async Task<User> GetByUserName(string username)
        {
            return await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Username == username);
        }
    }
}
