using IHK.DB.SeedBuilder;
using Microsoft.EntityFrameworkCore;
using System;

namespace IHK.DB
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsbuilder = new DbContextOptionsBuilder<DataContext>();
            optionsbuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Core-Test-V1;Trusted_Connection=True;MultipleActiveResultSets=true");
            var context = new DataContext(optionsbuilder.Options);

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            ConsoleKeyInfo cki;

            _outputMenue();

            do
            {
                cki = Console.ReadKey();

                switch (cki.KeyChar)
                {
                    case '1':
                        Console.WriteLine();
                        SeedData.Seed(context, true, true);
                        _outputMenue();
                        break;
                    case '2':
                        Console.WriteLine();
                        SeedData.Seed(context, true, false);
                        _outputMenue();
                        break;
                    case '3':
                        Console.WriteLine();
                        SeedData.Seed(context, false, true);
                        _outputMenue();
                        break;
                }

            } while (cki.Key != ConsoleKey.Escape);


            Console.WriteLine("Rdy");
            Console.ReadLine();
        }
        internal static void _outputMenue()
        {
            Console.WriteLine();
            Console.WriteLine("1 - Delete/Create");
            Console.WriteLine("2 - Delete");
            Console.WriteLine("3 - Create");
            Console.WriteLine("Esc - Exit");
        }
    }
}
