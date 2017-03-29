using IHK.DB;
using IHK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IHK.Repositorys
{
    public class MieterRepository
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Mieter> _db_Mieter;

        public MieterRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _db_Mieter = _dataContext.Set<Mieter>();
        }

        public async Task<List<Mieter>> GetAllMieter()
        {
            return await _db_Mieter.Include(m=>m.Wohnung).ThenInclude(w=>w.Gebaeude).ThenInclude(g=>g.Adresse).ToListAsync();
        }

        public async Task<Mieter> GetById(int id)
        {
            return await _db_Mieter.Include(m => m.Wohnung).ThenInclude(w => w.Gebaeude).ThenInclude(g => g.Adresse).SingleOrDefaultAsync(m=>m.Id == id);
        }

        //public async Task<List<Mieter>> GetMieterBy(Expression<Func<Mieter,bool>> pred)
        public async Task<List<Mieter>> GetMieterBy(ICollection<string> datas, Expression<Func<Mieter, string[]>> pred)
        {
            return await _db_Mieter.Include(m => m.Wohnung).ThenInclude(w => w.Gebaeude).ThenInclude(g => g.Adresse).MultiValueContainsAnyAll(datas,false,pred).ToListAsync();
        }
    }
}
