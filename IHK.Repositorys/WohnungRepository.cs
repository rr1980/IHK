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
    public class WohnungRepository
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Wohnung> _db_Wohnung;

        public WohnungRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _db_Wohnung = _dataContext.Set<Wohnung>();
        }

        public async Task<List<Wohnung>> GetAllWohnungen()
        {
            return await _db_Wohnung.Include(w => w.Gebaeude).ThenInclude(g => g.Adresse).ToListAsync();
        }

        public async Task<Wohnung> GetById(int id)
        {
            return await _db_Wohnung.Include(w => w.Gebaeude).ThenInclude(g => g.Adresse).SingleOrDefaultAsync(m => m.Id == id);
        }

        //public async Task<List<Mieter>> GetMieterBy(Expression<Func<Mieter,bool>> pred)
        public async Task<List<Wohnung>> GetWohnungBy(ICollection<string> datas, Expression<Func<Wohnung, string[]>> pred)
        {
            return await _db_Wohnung.Include(w => w.Gebaeude).ThenInclude(g => g.Adresse).MultiValueContainsAnyAll(datas, false, pred).ToListAsync();
        }

        public async Task<Gebaeude> GetGebaeudegById(int id)
        {
            return await _dataContext.Gebaeude.Include(w => w.Adresse).SingleOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Adresse> GetAdresseById(int id)
        {
            return await _dataContext.Adresse.SingleOrDefaultAsync(w => w.Id == id);
        }

        public void AddWohnung(Wohnung wohnung)
        {
            _dataContext.Wohnung.Add(wohnung);
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }
    }
}
