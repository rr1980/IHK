using IHK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IHK.DB.SeedBuilder
{
    internal static class Wohnung_Builder
    {
        internal static void Create(DataContext context)
        {
            List<Wohnung> wohs = new List<Wohnung>();

            for (int i = 1; i < 100; i++)
            {
                wohs.Add(new Wohnung()
                {
                    Gebaeude = context.Gebaeude.FirstOrDefault(a => a.Id == i)
                });
            }

            context.Wohnung.AddRange(wohs);
            context.SaveChanges();
        }
    }
}