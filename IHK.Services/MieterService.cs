using IHK.Models;
using IHK.Repositorys;
using IHK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IHK.Services
{
    public class MieterService
    {
        private readonly MieterRepository _mieterRepository;

        public MieterService(MieterRepository mieterRepository)
        {
            _mieterRepository = mieterRepository;
        }

        public async Task<List<MieterItemViewModel>> GetAllMieter()
        {
            var mieter = await _mieterRepository.GetAllMieter();

            return mieter.Select(m => _map(m)).ToList();
        }

        public async Task<MieterItemViewModel> GetMieterById(int id)
        {
            var mieter = await _mieterRepository.GetById(id);
            return _map(mieter);
        }

        public async Task<List<MieterItemViewModel>> SearchMieter(string datas)
        {
            ICollection<string> data = datas.Split(' ').Select(d => d.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            var mieter = await _mieterRepository.GetMieterBy(data, m => new[] {
                m.Name,
                m.Vorname,
                m.Telefon,
                m.WbsNummer,
                m.Wohnung.Wohnungsnummer,
                m.Wohnung.Gebaeude.Adresse.Hausnummer,
                m.Wohnung.Gebaeude.Adresse.Postleitzahl,
                m.Wohnung.Gebaeude.Adresse.Stadt,
                m.Wohnung.Gebaeude.Adresse.Strasse
            });

            return mieter.Select(m => _map(m)).ToList();
        }

        public async Task SaveMieter(MieterItemViewModel mieter)
        {
            var m = await _mieterRepository.GetById(mieter.Id);

            if (m == null)
            {
                m = new Mieter();
                _mieterRepository.AddMieter(m);
            }
            m = m.Map(mieter);

            var wex = await _mieterRepository.GetWohnungById(mieter.Wohnung.Id);
            if (wex == null)
            {
                wex = new Wohnung();
            }
            m.Wohnung = wex.Map(mieter.Wohnung);

            var gex = await _mieterRepository.GetGebaeudegById(mieter.Wohnung.Gebaeude.Id);
            if (gex == null)
            {
                gex = new Gebaeude();
            }
            m.Wohnung.Gebaeude = gex.Map(mieter.Wohnung.Gebaeude);

            var aex = await _mieterRepository.GetAdresseById(mieter.Wohnung.Gebaeude.Adresse.Id);
            if (aex == null)
            {
                aex = new Adresse();
            }
            m.Wohnung.Gebaeude.Adresse = aex.Map(mieter.Wohnung.Gebaeude.Adresse);

            _mieterRepository.SaveChanges();
        }

        private MieterItemViewModel _map(Mieter m)
        {
            return new MieterItemViewModel()
            {
                Id = m.Id,
                Anrede = (int)m.Anrede,
                Name = m.Name,
                Vorname = m.Vorname,
                Telefon = m.Telefon,
                WbsNummer = m.WbsNummer,
                Wohnung = new WohnungItemViewModel()
                {
                    Id = m.Wohnung.Id,
                    Wohnungsnummer = m.Wohnung.Wohnungsnummer,
                    Etage = m.Wohnung.Etage,
                    Keller = m.Wohnung.Keller,
                    Garage = m.Wohnung.Garage,
                    Balkon = m.Wohnung.Balkon,
                    Garten = m.Wohnung.Garten,
                    Raeume = m.Wohnung.Raeume,
                    Qm = m.Wohnung.Qm,

                    Gebaeude = new GebaeudeItemViewModel()
                    {
                        Id = m.Wohnung.Gebaeude.Id,
                        Etagen = m.Wohnung.Gebaeude.Etagen,
                        Gaerten = m.Wohnung.Gebaeude.Gaerten,

                        Adresse = new AdressenItemViewModel()
                        {
                            Id = m.Wohnung.Gebaeude.Adresse.Id,
                            Postleitzahl = m.Wohnung.Gebaeude.Adresse.Postleitzahl,
                            Stadt = m.Wohnung.Gebaeude.Adresse.Stadt,
                            Strasse = m.Wohnung.Gebaeude.Adresse.Strasse,
                            Hausnummer = m.Wohnung.Gebaeude.Adresse.Hausnummer
                        }
                    }
                }
            };
        }
    }
}
