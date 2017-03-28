﻿using IHK.DB;
using IHK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        //public async Task<LayoutTheme> GetLayoutThemeByName(string name)
        //{
        //    return await _db_LayoutTheme.SingleOrDefaultAsync(t => t.Name == name);
        //}

        //public async Task<LayoutTheme> GetLayoutThemeById(int id)
        //{
        //    return await _db_LayoutTheme.SingleOrDefaultAsync(t => t.Id == id);
        //}
    }
}