using IHK.DB;
using IHK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IHK.Repositorys
{
    public class GebaeudeRepository
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Gebaeude> _db_Gebaeude;

        public GebaeudeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _db_Gebaeude = _dataContext.Set<Gebaeude>();
        }

        public async Task<List<Gebaeude>> GetAllGebaeude()
        {
            return await _db_Gebaeude.Include(g => g.Adresse).ToListAsync();
        }

        public async Task<Gebaeude> GetById(int id)
        {
            return await _db_Gebaeude.Include(g => g.Adresse).SingleOrDefaultAsync(m => m.Id == id);
        }

        //public async Task<List<Mieter>> GetMieterBy(Expression<Func<Mieter,bool>> pred)
        public async Task<List<Gebaeude>> GetGebaeudeBy(ICollection<string> datas, Expression<Func<Gebaeude, string[]>> pred)
        {
            return await _db_Gebaeude.Include(g => g.Adresse).MultiValueContainsAnyAll(datas, false, pred).ToListAsync();
        }

        public async Task<Adresse> GetAdresseById(int id)
        {
            return await _dataContext.Adresse.SingleOrDefaultAsync(w => w.Id == id);
        }

        public void AddGebaeude(Gebaeude gebaeude)
        {
            _dataContext.Gebaeude.Add(gebaeude);
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }
    }
}
