using IHK.Common;
using IHK.Services;
using IHK.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.Web.ViewComponents
{
    public class SidebarComponent : ViewComponent
    {
        private readonly IAccountService _accountService;
        private readonly HttpContext _httpContext;

        public SidebarComponent(IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new SideBarViewModel()
            {

            });
        }
    }
}
