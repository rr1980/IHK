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
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly HttpContext _httpContext;

        public AdminController(IAccountService accountService, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContext = httpContextAccessor.HttpContext;
        }

        //[Authorize(Policy = "AdminPolicy")]
        //public async Task<IActionResult> Index()
        //{
        //    var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

        //    var result = await _accountService.GetAllUsers();
        //    result.Insert(0, new UserViewModel()
        //    {
        //        UserId = -1,
        //        ShowName = "Neu...",
        //        Roles = new int[] { -1 }
        //    });

        //    return View(new AdminViewModel()
        //    {
        //        Users = result,
        //        CurrentUser = await _accountService.GetById(id)
        //    });
        //}

        [Authorize(Policy = "AdminPolicy")]
        public async Task<AdminViewModel> SaveUser(IUserItemViewModel user)
        {
            List<IUserItemViewModel> result;

            if (!ModelState.IsValid)
            {
                result = await _accountService.GetAllUsers();
                result.Insert(0, new UserItemViewModel()
                {
                    UserId = -1,
                    ShowName = "Neu...",
                    Roles = new int[] { -1 }
                });

                return new AdminViewModel()
                {
                    Users = result,
                    Errors = GetModelStateErrors(ModelState)
                };
            }

            await _accountService.AddOrUpdate(user);
            result = await _accountService.GetAllUsers();
            result.Insert(0, new UserItemViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return new AdminViewModel()
            {
                Users = result,
            };
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task ResetPassord(IUserItemViewModel user)
        {
            await _accountService.ResetPassword(user.UserId);
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<AdminViewModel> DelUser(IUserItemViewModel user)
        {
            await _accountService.RemoveUserById(user.UserId);
            var result = await _accountService.GetAllUsers();
            result.Insert(0, new UserItemViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return new AdminViewModel()
            {
                Users = result,
            };
        }

        [Authorize]
        public IActionResult Error()
        {
            return View();
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



//_logger.LogWarning("loulou");
//_logger.LogError("loulou");
//_logger.LogWarning(LoggingEvents.GET_ITEM, "Getting item {ID}", 1);