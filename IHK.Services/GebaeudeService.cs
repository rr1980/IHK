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
    public class GebaeudeService
    {
        private readonly GebaeudeRepository _gebaeudeRepository;

        public GebaeudeService(GebaeudeRepository gebaeudeRepository)
        {
            _gebaeudeRepository = gebaeudeRepository;
        }

        public async Task<List<GebaeudeItemViewModel>> GetAllGebaeude()
        {
            var result = await _gebaeudeRepository.GetAllGebaeude();

            return result.Select(m => _map(m)).ToList();
        }

        public async Task<GebaeudeItemViewModel> GetGebaeudeById(int id)
        {
            var result = await _gebaeudeRepository.GetById(id);
            return _map(result);
        }

        public async Task<List<GebaeudeItemViewModel>> SearchGebaeude(string datas)
        {
            ICollection<string> data = datas.Split(' ').Select(d => d.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            var result = await _gebaeudeRepository.GetGebaeudeBy(data, m => new[] {
                m.Adresse.Hausnummer,
                m.Adresse.Postleitzahl,
                m.Adresse.Stadt,
                m.Adresse.Strasse
            });

            return result.Select(m => _map(m)).ToList();
        }

        public async Task SaveGebaeude(GebaeudeItemViewModel gebaeude)
        {
            var m = await _gebaeudeRepository.GetById(gebaeude.Id);

            if (m == null)
            {
                m = new Gebaeude();
                _gebaeudeRepository.AddGebaeude(m);
            }
            m = m.Map(gebaeude);


            var aex = await _gebaeudeRepository.GetAdresseById(gebaeude.Adresse.Id);
            if (aex == null)
            {
                aex = new Adresse();
            }
            m.Adresse = aex.Map(gebaeude.Adresse);

            _gebaeudeRepository.SaveChanges();
        }

        private GebaeudeItemViewModel _map(Gebaeude m)
        {
            return new GebaeudeItemViewModel()
            {
                    Id = m.Id,
                    Etagen = m.Etagen,
                    Gaerten = m.Gaerten,

                    Adresse = new AdressenItemViewModel()
                    {
                        Id = m.Adresse.Id,
                        Postleitzahl = m.Adresse.Postleitzahl,
                        Stadt = m.Adresse.Stadt,
                        Strasse = m.Adresse.Strasse,
                        Hausnummer = m.Adresse.Hausnummer
                    }
            };
        }
    }
}
