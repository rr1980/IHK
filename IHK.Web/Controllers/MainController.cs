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
using IHK.Common;

namespace IHK.Web.Controllers
{
    public class MainController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly HttpContext _httpContext;

        public MainController(IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new MainViewModel()
            {
                CurrentUser = await _accountService.GetById(id)
            });
        }

        [Authorize(Policy = "DefaultPolicy")]
        public  IActionResult GetUser(int id)
        {
            return View($"<h1>Yehaaa_{id}</h1>");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
