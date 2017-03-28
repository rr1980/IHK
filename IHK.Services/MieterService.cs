using IHK.Models;
using IHK.Repositorys;
using IHK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Wohnungsnummer = m.Wohnung.Wohnungsnummer,
                Postleitzahl = m.Wohnung.Gebaeude.Adresse.Postleitzahl,
                Stadt = m.Wohnung.Gebaeude.Adresse.Stadt,
                Strasse = m.Wohnung.Gebaeude.Adresse.Strasse,
                Hausnummer = m.Wohnung.Gebaeude.Adresse.Hausnummer
            };
        }
    }
}
