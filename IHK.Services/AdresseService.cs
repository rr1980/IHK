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
    public class AdresseService
    {
        private readonly AdresseRepository _adresseRepository;

        public AdresseService(AdresseRepository adresseRepository)
        {
            _adresseRepository = adresseRepository;
        }

        public async Task<List<AdressenItemViewModel>> GetAllAdresse()
        {
            var result = await _adresseRepository.GetAllAdressen();

            return result.Select(m => _map(m)).ToList();
        }

        public async Task<AdressenItemViewModel> GetAdresseById(int id)
        {
            var result = await _adresseRepository.GetById(id);
            return _map(result);
        }

        public async Task<List<AdressenItemViewModel>> SearchAdresse(string datas)
        {
            ICollection<string> data = datas.Split(' ').Select(d => d.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            var result = await _adresseRepository.GetAdresseBy(data, m => new[] {
                m.Hausnummer,
                m.Postleitzahl,
                m.Stadt,
                m.Strasse
            });

            return result.Select(m => _map(m)).ToList();
        }

        public async Task SaveAdresse(AdressenItemViewModel adresse)
        {
            var m = await _adresseRepository.GetById(adresse.Id);

            if (m == null)
            {
                m = new Adresse();
                _adresseRepository.AddAdresse(m);
            }
            m = m.Map(adresse);

            _adresseRepository.SaveChanges();
        }

        private AdressenItemViewModel _map(Adresse m)
        {
            return  new AdressenItemViewModel()
                    {
                        Id = m.Id,
                        Postleitzahl = m.Postleitzahl,
                        Stadt = m.Stadt,
                        Strasse = m.Strasse,
                        Hausnummer = m.Hausnummer
            };
        }
    }
}
