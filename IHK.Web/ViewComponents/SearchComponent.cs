using IHK.Common;
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
    public class SearchComponent : ViewComponent
    {
        public SearchComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(EntityType entityType)
        {
            return View(new SearchComponentViewModel() {
                PanelHeaderText = entityType.ToString()
            });
        }
    }
}
