using IHK.Services;
using IHK.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IHK.Web.ViewComponents
{
    public class NavbarComponent : ViewComponent
    {
        private readonly AccountService _accountService;
        private readonly HttpContext _httpContext;

        public NavbarComponent(AccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool showSidebar = false, string tab = "Suche")
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new NavbarViewModel()
            {
                UserViewModel = await _accountService.GetById(id),
                ShowSidebar = showSidebar,
                activeTab = tab
            });
        }
    }
}
