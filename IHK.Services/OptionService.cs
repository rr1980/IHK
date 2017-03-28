using IHK.Repositorys;
using System;
using System.Collections.Generic;
using System.Text;
using IHK.ViewModels;
using System.Threading.Tasks;
using IHK.Models;
using System.Linq;

namespace IHK.Services
{
    public class OptionService
    {
        private readonly OptionRepository _optionRepository;

        public OptionService(OptionRepository optionRepository)
        {
            _optionRepository = optionRepository;
        }

        public async Task<List<LayoutThemeItemViewModel>> GetAllThemes()
        {
            var themes = await _optionRepository.GetAllLayoutThemes();
            return themes.Select(t => _map(t)).ToList();
        }

        private LayoutThemeItemViewModel _map(LayoutTheme theme)
        {
            return new LayoutThemeItemViewModel()
            {
                Id = theme.Id,
                Name = theme.Name,
                Link = theme.Link
            };
        }
    }
}
