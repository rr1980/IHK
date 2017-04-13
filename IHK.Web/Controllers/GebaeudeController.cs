using IHK.Common;
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
    public class GebaeudeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly HttpContext _httpContext;
        private readonly GebaeudeService _gebaeudeService;

        public GebaeudeController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, GebaeudeService gebaeudeService)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
            _gebaeudeService = gebaeudeService;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index(int id)
        {
            var userId = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var gebaeude = await _gebaeudeService.GetGebaeudeById(id);

            return View(new GebaeudeViewModel()
            {
                CurrentUser = await _accountService.GetById(userId),
                Gebaeude = gebaeude
            });
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<List<GebaeudeItemViewModel>> SearchGebaeude(string datas)
        {
            List<GebaeudeItemViewModel> gebaeude = new List<GebaeudeItemViewModel>();

            if (datas != null)
            {
                datas = datas.Trim();
            }

            if (string.IsNullOrEmpty(datas))
            {
                gebaeude = await _gebaeudeService.GetAllGebaeude();
            }
            else
            {
                gebaeude = await _gebaeudeService.SearchGebaeude(datas);
            }

            return gebaeude;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<GebaeudeViewModel> SaveGebaeude(GebaeudeItemViewModel gebaeude)
        {

            if (!ModelState.IsValid)
            {
                return new GebaeudeViewModel()
                {
                    Errors = GetModelStateErrors(ModelState)
                };
            }
            else
            {
                _gebaeudeService.SaveGebaeude(gebaeude);
            }

            //ToDo SAVE

            return new GebaeudeViewModel();
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
