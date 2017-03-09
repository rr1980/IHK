using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IHK.Services;
using Microsoft.AspNetCore.Http;
using IHK.ViewModels;
using System.Security.Claims;

namespace IHK.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AccountService _accountService;
        private readonly HttpContext _httpContext;

        public HomeController(AccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new HomeViewModel()
            {
                CurrentUser = await _accountService.GetById(id)
            });
        }



        public IActionResult Error()
        {
            return View();
        }
    }
}
