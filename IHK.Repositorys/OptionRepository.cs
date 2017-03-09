using IHK.DB;
using IHK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHK.Repositorys
{
    public class OptionRepository
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<LayoutTheme> _db_LayoutTheme;

        public OptionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _db_LayoutTheme = _dataContext.Set<LayoutTheme>();
        }

        public async Task<List<LayoutTheme>> GetAllLayoutThemes()
        {
            return await _db_LayoutTheme.ToListAsync();
        }

        public async Task<LayoutTheme> GetLayoutThemeByName(string name)
        {
            return await _db_LayoutTheme.SingleOrDefaultAsync(t => t.Name == name);
        }

        public async Task<LayoutTheme> GetLayoutThemeById(int id)
        {
            return await _db_LayoutTheme.SingleOrDefaultAsync(t => t.Id == id);
        }
    }
}
