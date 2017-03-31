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
    public class MieterController : Controller
    {
        private readonly AccountService _accountService;
        private readonly HttpContext _httpContext;
        private readonly MieterService _mieterService;

        public MieterController(AccountService accountService, IHttpContextAccessor httpContextAccessor, MieterService mieterService)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
            _mieterService = mieterService;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index(int id)
        {
            var userId = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var mieter = await _mieterService.GetMieterById(id);

            return View(new MieterViewModel() {
                CurrentUser = await _accountService.GetById(userId),
                Mieter = mieter
            });
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<List<MieterItemViewModel>> SearchMieter(string datas)
        {
            List<MieterItemViewModel> mieter = new List<MieterItemViewModel>();

            if (datas != null)
            {
                datas = datas.Trim();
            }

            if (string.IsNullOrEmpty(datas))
            {
                mieter = await _mieterService.GetAllMieter();
            }
            else
            {
                mieter = await _mieterService.SearchMieter(datas);
            }

            return mieter;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<MieterViewModel> SaveMieter(MieterItemViewModel mieter)
        {

            if (!ModelState.IsValid)
            {
                return new MieterViewModel()
                {
                    Errors = GetModelStateErrors(ModelState)
                };
            }

            //ToDo SAVE

            return new MieterViewModel();
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
