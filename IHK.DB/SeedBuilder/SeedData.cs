using IHK.Common;
using IHK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHK.DB.SeedBuilder
{
    public static class SeedData
    {
        public static void Seed(DataContext context, bool del, bool create)
        {
            if (del)
            {
                _del(context);
            }

            if (create)
            {
                if (_create(context))
                {
                    _build(context);
                }
            }
        }

        public static void Seed(DataContext context)
        {
            if (_create(context))
            {
                _build(context);
            }
        }

        internal static void _build(DataContext context)
        {
            Console.WriteLine("Erzeuge Daten...");

            LayoutTheme_Builder.Create(context);

            Roles_Builder.Create(context);

            Adresse_Builder.Create(context);
            Gebaeude_Builder.Create(context);
            //Wohnung_Builder.Create(context);

            Mieter_Builder.Create(context);

            Users_Builder.Create(context,
                        new UserRoleType[] { UserRoleType.Admin, UserRoleType.Default },
                        new User
                        {
                            Anrede = 0,
                            Adresse = context.Adresse.FirstOrDefault(a => a.Id == 2),
                            //Postleitzahl = "01983",
                            //Stadt = "Strausberg",
                            //Strasse = "Am Annatal 11",

                            Telefon = "12003",
                            Email = "rener1980@gmx.de",
                            Name = "Riesner",
                            Vorname = "Rene",
                            Username = "rr1980",
                            Password = "12003",
                            LayoutTheme = context.LayoutThemes.SingleOrDefault(lt => lt.Name == "default")
                        });

            Users_Builder.Create(context,
                        new UserRoleType[] { UserRoleType.Default },
                        new User
                        {
                            Anrede = 1,

                            Adresse = context.Adresse.FirstOrDefault(a => a.Id == 2),
                            //Postleitzahl = "15344",
                            //Stadt = "Großräschen",
                            //Strasse = "Rosa-Luxemburg Strasse 15",

                            Telefon = "12003",
                            Email = "rener1980@gmail.com",
                            Name = "Riesner",
                            Vorname = "Sven",
                            Username = "Oxi",
                            Password = "12003",
                            LayoutTheme = context.LayoutThemes.SingleOrDefault(lt => lt.Name == "slate")
                        });

            Users_Builder.Create(context,
                        new UserRoleType[] { UserRoleType.Default },
                        new User
                        {
                            Anrede = 1,

                            Adresse = context.Adresse.FirstOrDefault(a => a.Id == 2),
                            //Postleitzahl = "15344",
                            //Stadt = "Großräschen",
                            //Strasse = "Rosa-Luxemburg Strasse 15",

                            Telefon = "12003",
                            Email = "rener1980@gmail.com",
                            Name = "Riesner",
                            Vorname = "Test",
                            Username = "Test",
                            Password = "12003",
                            LayoutTheme = context.LayoutThemes.SingleOrDefault(lt => lt.Name == "slate")
                        });

        }

        private static void _del(DataContext context)
        {
            Console.WriteLine("Versuche Database zu löschen, einen Moment bitte...");
            if (context.Database.EnsureDeleted())
            {
                Console.WriteLine("Database gelöscht...");
            }
            else
            {
                Console.WriteLine("Keine Database gefunden...");
            }
        }

        private static bool _create(DataContext context)
        {
            Console.WriteLine("Versuche Database zu erzeugen, einen Moment bitte...");

            if (context.Database.EnsureCreated())
            {
                Console.WriteLine("Database erzeugt...");
                return true;
            }
            else
            {
                Console.WriteLine("Database existiert bereits...");
                return false;
            }
        }
    }
}
