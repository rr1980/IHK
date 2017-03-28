using IHK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IHK.DB.SeedBuilder
{
    internal static class Gebaeude_Builder
    {
        internal static void Create(DataContext context)
        {
            List<Gebaeude> gebs = new List<Gebaeude>();

            for (int i = 1; i < 100; i++)
            {
                gebs.Add(new Gebaeude()
                {
                    Adresse = context.Adresse.FirstOrDefault(a => a.Id == i)
                });
            }

            context.Gebaeude.AddRange(gebs);
            context.SaveChanges();
        }
    }
}