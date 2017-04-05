using IHK.Services;
using IHK.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IHK.Web.Controllers
{
    public class AdresseController : Controller
    {
        private readonly AccountService _accountService;
        private readonly HttpContext _httpContext;
        private readonly AdresseService _adresseService;

        public AdresseController(AccountService accountService, IHttpContextAccessor httpContextAccessor, AdresseService adresseService)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
            _adresseService = adresseService;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index(int id)
        {
            var userId = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var adressen = await _adresseService.GetAdresseById(id);

            return View(new AdresseViewModel()
            {
                CurrentUser = await _accountService.GetById(userId),
                Adresse = adressen
            });
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<List<AdressenItemViewModel>> SearchAdresse(string datas)
        {
            List<AdressenItemViewModel> gebaeude = new List<AdressenItemViewModel>();

            if (datas != null)
            {
                datas = datas.Trim();
            }

            if (string.IsNullOrEmpty(datas))
            {
                gebaeude = await _adresseService.GetAllAdresse();
            }
            else
            {
                gebaeude = await _adresseService.SearchAdresse(datas);
            }

            return gebaeude;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<AdresseViewModel> SaveAdresse(AdressenItemViewModel adresse)
        {

            if (!ModelState.IsValid)
            {
                return new AdresseViewModel()
                {
                    Errors = GetModelStateErrors(ModelState)
                };
            }
            else
            {
                _adresseService.SaveAdresse(adresse);
            }

            //ToDo SAVE

            return new AdresseViewModel();
        }

        private List<string> GetModelStateErrors(ModelStateDictionary ModelState)
        {
            List<string> errorMessages = new List<string>();

            var validationErrors = ModelState.Values.Select(x => x.Errors);
            validationErrors.ToList().ForEach(ve =>
            {
                var errorStrings = ve.Select(x => x.ErrorMessage);
                errorStrings.ToList().ForEach(em =>
                {
                    errorMessages.Add(em);
                });
            });

            return errorMessages;
        }
    }
}
