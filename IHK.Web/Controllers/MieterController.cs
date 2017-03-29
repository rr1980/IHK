using IHK.Services;
using IHK.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.Web.Controllers
{
    public class MieterController : Controller
    {
        private readonly HttpContext _httpContext;
        private readonly MieterService _mieterService;

        public MieterController(IHttpContextAccessor httpContextAccessor, MieterService mieterService)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _mieterService = mieterService;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index(int id)
        {
            var mieter = await _mieterService.GetMieterById(id);

            return View(new MieterViewModel() {
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
    }
}
