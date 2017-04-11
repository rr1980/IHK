using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.Web.Controllers
{
    public class DebugController:Controller
    {
        public DebugController()
        {

        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {


            return View();
        }
    }
}
