using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ON.Authentication;
using SimpleWeb.Models;
using SimpleWeb.Models.Auth;
using SimpleWeb.Models.CMS;
using SimpleWeb.Services;

namespace SimpleWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ContentService contentService;
        private readonly MainPaymentsService paymentsService;
        private readonly ONUserHelper userHelper;

        public HomeController(ILogger<HomeController> logger, ContentService contentService, MainPaymentsService paymentsService, ONUserHelper userHelper)
        {
            this.logger = logger;
            this.contentService = contentService;
            this.paymentsService = paymentsService;
            this.userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel((await contentService.GetAll()), userHelper.MyUser);

            if (userHelper.IsLoggedIn)
            {
                var res = await paymentsService.GetOwnOneTimeRecords();
                if (res != null)
                {
                    model.OneTimeRecords.AddRange(res.Stripe);
                }
            }

            ViewBag.IsHome = true;
            return View("Home", model);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            return View("Home", new HomeViewModel((await contentService.Search(query ?? "")), userHelper.MyUser));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
