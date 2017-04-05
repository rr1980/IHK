using IHK.Models;
using IHK.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IHK.Services
{
    public static class ServiceExtensions
    {
        public static Mieter Map(this Mieter mieter, MieterItemViewModel m)
        {
            mieter.Anrede = m.Anrede;
            mieter.Name = m.Name;
            mieter.Telefon = m.Telefon;
            mieter.Vorname = m.Vorname;
            mieter.WbsNummer = m.WbsNummer;

            return mieter;
        }

        public static Wohnung Map(this Wohnung wohnung, WohnungItemViewModel w)
        {
            wohnung.Wohnungsnummer = w.Wohnungsnummer;
            wohnung.Etage = w.Etage;
            wohnung.Keller = w.Keller;
            wohnung.Garage = w.Garage;
            wohnung.Balkon = w.Balkon;
            wohnung.Garten = w.Garten;
            wohnung.Raeume = w.Raeume;
            wohnung.Qm = w.Qm;

            return wohnung;
        }

        public static Gebaeude Map(this Gebaeude gebaeude, GebaeudeItemViewModel g)
        {
            gebaeude.Etagen = g.Etagen;
            gebaeude.Gaerten = g.Gaerten;
            gebaeude.Wohnungen = g.Wohnungen.Count();

            return gebaeude;
        }

        public static Adresse Map(this Adresse adresse, AdressenItemViewModel a)
        {
            adresse.Postleitzahl = a.Postleitzahl;
            adresse.Stadt = a.Stadt;
            adresse.Strasse = a.Strasse;
            adresse.Hausnummer = a.Hausnummer;

            return adresse;
        }
    }
}
