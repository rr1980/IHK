using IHK.DB;
using IHK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IHK.Repositorys
{
    public class AdresseRepository
    {
        private readonly DataContext _dataContext;
        private readonly DbSet<Adresse> _db_Adresse;

        public AdresseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            _db_Adresse = _dataContext.Set<Adresse>();
        }

        public async Task<List<Adresse>> GetAllAdressen()
        {
            return await _db_Adresse.ToListAsync();
        }

        public async Task<Adresse> GetById(int id)
        {
            return await _db_Adresse.SingleOrDefaultAsync(m => m.Id == id);
        }

        //public async Task<List<Mieter>> GetMieterBy(Expression<Func<Mieter,bool>> pred)
        public async Task<List<Adresse>> GetAdresseBy(ICollection<string> datas, Expression<Func<Adresse, string[]>> pred)
        {
            return await _db_Adresse.MultiValueContainsAnyAll(datas, false, pred).ToListAsync();
        }

        public void AddAdresse(Adresse adresse)
        {
            _dataContext.Adresse.Add(adresse);
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }
    }
}
