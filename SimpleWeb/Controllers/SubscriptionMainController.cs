﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ON.Settings;
using SimpleWeb.Models.Subscription.Main;
using SimpleWeb.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWeb.Controllers
{
    [Authorize]
    [Route("subscription")]
    public class SubscriptionMainController : Controller
    {
        private readonly ILogger<SubscriptionMainController> logger;
        private readonly UserService userService;
        private readonly MainPaymentsService payService;
        private readonly SubscriptionTierHelper subHelper;
        private readonly SettingsClient settingsClient;

        public SubscriptionMainController(ILogger<SubscriptionMainController> logger, UserService userService, MainPaymentsService payService, SubscriptionTierHelper subHelper, SettingsClient settingsClient)
        {
            this.logger = logger;
            this.userService = userService;
            this.payService = payService;
            this.subHelper = subHelper;
            this.settingsClient = settingsClient;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var rec = await payService.GetOwnSubscriptionRecord();
            var v = IndexViewModel.Create(subHelper, rec);

            return View(v);
        }

        [HttpGet("new/{amountCents}")]
        public async Task<IActionResult> New(uint amountCents)
        {
            var v = NewViewModel.Create(amountCents, settingsClient);

            var domainName = Request.Host.Host == "localhost" ? "http" : "https";
            domainName += "://" + Request.Host.Host;

            v.Methods = await payService.GetNewDetails(amountCents, domainName);

            return View(v);
        }

        [HttpGet("{subId}")]
        public async Task<IActionResult> Single(string subId)
        {
            var rec = await payService.GetOwnSubscriptionRecord();
            var v = IndexViewModel.Create(subHelper, rec);

            var v2 = v.Subscriptions.FirstOrDefault(s => s.SubscriptionId.ToString() == subId);

            return View(v2);
        }
    }
}
