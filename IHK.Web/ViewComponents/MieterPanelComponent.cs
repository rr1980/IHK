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
    public class MieterPanelComponent : ViewComponent
    {
        //private readonly AccountService _accountService;
        private readonly HttpContext _httpContext;

        public MieterPanelComponent(AccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            //_accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
