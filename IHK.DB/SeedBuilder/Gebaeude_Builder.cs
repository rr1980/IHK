using IHK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IHK.DB.SeedBuilder
{
    internal static class Gebaeude_Builder
    {
        static Random rnd;

        static Gebaeude_Builder()
        {
            rnd = new Random();
        }

        internal static void Create(DataContext context)
        {
            List<Gebaeude> gebs = new List<Gebaeude>();
            for (int i = 1; i < 100; i++)
            {
                var etagen = rnd.Next(1, 10);

                var geb = new Gebaeude()
                {
                    Adresse = context.Adresse.FirstOrDefault(a => a.Id == i),
                    Etagen = etagen,
                    Gaerten = (int)Math.Floor((decimal)(etagen / 2)),
                    Wohnungen = etagen
                };

                List<Wohnung> wohs = new List<Wohnung>();
                for (int o = 1; o < etagen; o++)
                {
                    var r = rnd.Next(1, 5);

                    var woh = new Wohnung()
                    {
                        Gebaeude = geb,
                        Wohnungsnummer = "00" + o,
                        Etage = o,
                        Keller = rnd.Next(0, 2) == 1 ? true : false,
                        Garage = rnd.Next(0, 2) == 1 ? true : false,
                        Balkon = rnd.Next(0, 2) == 1 ? true : false,
                        Garten = rnd.Next(0, 2) == 1 ? true : false,
                        Raeume = r,
                        Qm = r * 25
                    };

                    wohs.Add(woh);
                }

                context.Wohnung.AddRange(wohs);
                gebs.Add(geb);
            }

            context.Gebaeude.AddRange(gebs);
            context.SaveChanges();
        }
    }
}