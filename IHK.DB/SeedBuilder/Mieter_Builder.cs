using IHK.Models;
using System.IO;
using System.Linq;

namespace IHK.DB.SeedBuilder
{
    public static class Mieter_Builder
    {
        internal static void Create(DataContext context)
        {
            var directory = Directory.GetCurrentDirectory();

            string[] allLines = File.ReadAllLines(Path.Combine(directory, "../IHK.DB/Datas/PersonsData.csv"));

            var lines = allLines.Skip(1);

            var query = from line in lines
                        let data = line.Split(',')
                        select new Mieter()
                        {
                            Anrede = data[0] == "male" ? 0 : 1,
                            Name = data[1].Replace("\"", "").Trim(),
                            Vorname = data[2].Replace("\"", "").Trim(),
                            Strasse = _getStrasse(data[3].Replace("\"", "").Trim()),
                            Hausnummer = _getHnr(data[3].Replace("\"", "").Trim()),
                            Postleitzahl = data[5].Replace("\"", "").Trim(),
                            Stadt = data[6].Replace("\"", "").Trim(),
                            Telefon = data[7].Replace("\"", "").Trim(),
                            WbsNummer = data[8].Replace("\"", "").Trim()
                        };

            context.Mieters.AddRange(query);
            context.SaveChanges();
        }

        private static string _getStrasse(string data)
        {
            var str = data.Split(' ');
            string result = "";

            for (int i = 0; i < str.Length-1; i++)
            {
                result += " " + str[i];
            }

            return result.Trim();
        }

        private static string _getHnr(string data)
        {
            var str = data.Split(' ');
            return str.Last().Trim();
        }
    }
}
