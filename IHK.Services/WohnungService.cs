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
    public class WohnungService
    {
        private readonly WohnungRepository _wohnungRepository;

        public WohnungService(WohnungRepository wohnungRepository)
        {
            _wohnungRepository = wohnungRepository;
        }

        public async Task<List<WohnungItemViewModel>> GetAllWohnungen()
        {
            var result = await _wohnungRepository.GetAllWohnungen();

            return result.Select(m => _map(m)).ToList();
        }

        public async Task<WohnungItemViewModel> GetWohnungById(int id)
        {
            var result = await _wohnungRepository.GetById(id);
            return _map(result);
        }

        public async Task<List<WohnungItemViewModel>> SearchWohnung(string datas)
        {
            ICollection<string> data = datas.Split(' ').Select(d => d.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            var result = await _wohnungRepository.GetWohnungBy(data, m => new[] {
                m.Wohnungsnummer,
                m.Gebaeude.Adresse.Hausnummer,
                m.Gebaeude.Adresse.Postleitzahl,
                m.Gebaeude.Adresse.Stadt,
                m.Gebaeude.Adresse.Strasse
            });

            return result.Select(m => _map(m)).ToList();
        }

        public async Task SaveWohnung(WohnungItemViewModel wohnung)
        {
            var m = await _wohnungRepository.GetById(wohnung.Id);

            if (m == null)
            {
                m = new Wohnung();
                _wohnungRepository.AddWohnung(m);
            }
            m = m.Map(wohnung);

            var gex = await _wohnungRepository.GetGebaeudegById(wohnung.Gebaeude.Id);
            if (gex == null)
            {
                gex = new Gebaeude();
            }
            m.Gebaeude = gex.Map(wohnung.Gebaeude);

            var aex = await _wohnungRepository.GetAdresseById(wohnung.Gebaeude.Adresse.Id);
            if (aex == null)
            {
                aex = new Adresse();
            }
            m.Gebaeude.Adresse = aex.Map(wohnung.Gebaeude.Adresse);

            _wohnungRepository.SaveChanges();
        }

        private WohnungItemViewModel _map(Wohnung m)
        {
            return new WohnungItemViewModel()
            {
                Id = m.Id,
                Wohnungsnummer = m.Wohnungsnummer,
                Etage = m.Etage,
                Keller = m.Keller,
                Garage = m.Garage,
                Balkon = m.Balkon,
                Garten = m.Garten,
                Raeume = m.Raeume,
                Qm = m.Qm,

                Gebaeude = new GebaeudeItemViewModel()
                {
                    Id = m.Gebaeude.Id,
                    Etagen = m.Gebaeude.Etagen,
                    Gaerten = m.Gebaeude.Gaerten,

                    Adresse = new AdressenItemViewModel()
                    {
                        Id = m.Gebaeude.Adresse.Id,
                        Postleitzahl = m.Gebaeude.Adresse.Postleitzahl,
                        Stadt = m.Gebaeude.Adresse.Stadt,
                        Strasse = m.Gebaeude.Adresse.Strasse,
                        Hausnummer = m.Gebaeude.Adresse.Hausnummer
                    }
                }
            };
        }
    }
}
