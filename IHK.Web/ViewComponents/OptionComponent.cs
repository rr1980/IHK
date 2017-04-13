using IHK.Common;
using IHK.Services;
using IHK.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IHK.Web.ViewComponents
{
    public class OptionComponent : ViewComponent
    {
        private readonly IAccountService _accountService;
        private readonly OptionService _optionService;
        private readonly HttpContext _httpContext;

        public OptionComponent(IAccountService accountService, OptionService optionService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _optionService = optionService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new OptionViewModel()
            {
                CurrentUser = await _accountService.GetById(id),
                LayoutThemeViewModels = await _optionService.GetAllThemes()
            });
        }
    }
}
