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
    public class AdminComponent : ViewComponent
    {
        private readonly AccountService _accountService;
        private readonly OptionService _optionService;
        private readonly HttpContext _httpContext;

        public AdminComponent(AccountService accountService, OptionService optionService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _optionService = optionService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var result = await _accountService.GetAllUsers();
            result.Insert(0, new UserItemViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return View(new AdminViewModel()
            {
                Users = result,
                CurrentUser = await _accountService.GetById(id)
            });
        }
    }
}
