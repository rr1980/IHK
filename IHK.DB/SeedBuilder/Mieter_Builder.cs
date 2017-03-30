using IHK.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IHK.DB.SeedBuilder
{
    public static class Mieter_Builder
    {
        static Random rnd;

        static Mieter_Builder()
        {
            rnd = new Random();
        }

        internal static void Create(DataContext context)
        {
            var wohsCount = context.Wohnung.Count();

            var directory = Directory.GetCurrentDirectory();

            string[] allLines = File.ReadAllLines(Path.Combine(directory, "../IHK.DB/Datas/PersonsData.csv"));

            var lines = allLines.Skip(1).ToList();

            List<Mieter> miets = new List<Mieter>();
            for (int i = 0; i < lines.Count(); i++)
            {
                var data = lines[i].Split(',');
                var wohId = rnd.Next(1, wohsCount);
                var woh = context.Wohnung.FirstOrDefault(g => g.Id == wohId);

                var mieter = new Mieter()
                {
                    Anrede = data[0] == "male" ? 0 : 1,
                    Name = data[1].Replace("\"", "").Trim(),
                    Vorname = data[2].Replace("\"", "").Trim(),
                    Wohnung = woh,
                    //Wohnung = context.Wohnung.FirstOrDefault(a=>a.Gebaeude.Adresse.Strasse== _getStrasse(data[3].Replace("\"", "").Trim())),
                    Telefon = data[7].Replace("\"", "").Trim(),
                    WbsNummer = data[8].Replace("\"", "").Trim()
                };

                miets.Add(mieter);
            }

            //var query = from line in lines
            //            let data = line.Split(',')
            //            select new Mieter()
            //            {
            //                Anrede = data[0] == "male" ? 0 : 1,
            //                Name = data[1].Replace("\"", "").Trim(),
            //                Vorname = data[2].Replace("\"", "").Trim(),
            //                //Strasse = _getStrasse(data[3].Replace("\"", "").Trim()),
            //                //Hausnummer = _getHnr(data[3].Replace("\"", "").Trim()),
            //                //Postleitzahl = data[5].Replace("\"", "").Trim(),
            //                //Stadt = data[6].Replace("\"", "").Trim(),
            //                //Wohnung = context.Gebaeude.FirstOrDefault(g=>g.Id == rnd.Next(0, gebsCount))
            //                //Wohnung = context.Wohnung.FirstOrDefault(a=>a.Gebaeude.Adresse.Strasse== _getStrasse(data[3].Replace("\"", "").Trim())),
            //                Telefon = data[7].Replace("\"", "").Trim(),
            //                WbsNummer = data[8].Replace("\"", "").Trim()
            //            };

            context.Mieters.AddRange(miets);
            context.SaveChanges();
        }


        private static string _getStrasse(string data)
        {
            var str = data.Split(' ');
            string result = "";

            for (int i = 0; i < str.Length - 1; i++)
            {
                result += " " + str[i];
            }

            return result.Trim();
        }
    }
}
