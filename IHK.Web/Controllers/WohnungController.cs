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
    public class WohnungController : Controller
    {
        private readonly AccountService _accountService;
        private readonly HttpContext _httpContext;
        private readonly WohnungService _wohnungService;

        public WohnungController(AccountService accountService, IHttpContextAccessor httpContextAccessor, WohnungService wohnungService)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
            _wohnungService = wohnungService;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index(int id)
        {
            var userId = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var wohnung = await _wohnungService.GetWohnungById(id);

            return View(new WohnungViewModel()
            {
                CurrentUser = await _accountService.GetById(userId),
                Wohnung = wohnung,
                Gebaeude = wohnung.Gebaeude,
                Adressen = wohnung.Gebaeude.Adresse
            });
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<List<WohnungItemViewModel>> SearchWohnung(string datas)
        {
            List<WohnungItemViewModel> wohnungen = new List<WohnungItemViewModel>();

            if (datas != null)
            {
                datas = datas.Trim();
            }

            if (string.IsNullOrEmpty(datas))
            {
                wohnungen = await _wohnungService.GetAllWohnungen();
            }
            else
            {
                wohnungen = await _wohnungService.SearchWohnung(datas);
            }

            return wohnungen;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<WohnungViewModel> SaveWohnung(WohnungItemViewModel wohnung)
        {

            if (!ModelState.IsValid)
            {
                return new WohnungViewModel()
                {
                    Errors = GetModelStateErrors(ModelState)
                };
            }
            else
            {
                _wohnungService.SaveWohnung(wohnung);
            }

            //ToDo SAVE

            return new WohnungViewModel();
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
