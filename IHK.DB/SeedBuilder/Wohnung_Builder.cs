using IHK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IHK.DB.SeedBuilder
{
    internal static class Wohnung_Builder
    {
        static Random rnd;

        static Wohnung_Builder()
        {
            rnd = new Random();
        }

        internal static void Create(DataContext context)
        {
            List<Wohnung> wohs = new List<Wohnung>();

            for (int i = 1; i < 100; i++)
            {
                var geb = context.Gebaeude.FirstOrDefault(a => a.Id == i);

                //wohs.Add();
            }

            context.Wohnung.AddRange(wohs);
            context.SaveChanges();
        }
    }
}


//rnd.Next(-100, 101)