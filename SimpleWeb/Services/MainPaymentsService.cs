﻿using Grpc.Core;
using Microsoft.Extensions.Logging;
using ON.Authentication;
using ON.Fragments.Authorization.Payment;
using ON.Fragments.Authorization.Payment.Fake;
using ON.Settings;
using System;
using System.Threading.Tasks;

namespace SimpleWeb.Services
{
    public class MainPaymentsService
    {
        private readonly ILogger logger;
        private readonly ServiceNameHelper nameHelper;
        public readonly ONUser User;

        public MainPaymentsService(ServiceNameHelper nameHelper, ONUserHelper userHelper, ILogger<MainPaymentsService> logger)
        {
            this.logger = logger;

            User = userHelper.MyUser;

            this.nameHelper = nameHelper;
        }

        public bool IsLoggedIn { get => User != null; }

        public async Task<GetNewDetailsResponse> GetNewDetails(uint level, string domainName)
        {
            if (!IsLoggedIn)
                return null;

            if (nameHelper.PaymentServiceChannel == null)
                return null;

            logger.LogWarning($"******Trying to hopefully connect to PaymentService at:({nameHelper.PaymentServiceChannel.Target})******");


            var client = new PaymentInterface.PaymentInterfaceClient(nameHelper.PaymentServiceChannel);
            var reply = await client.GetNewDetailsAsync(new GetNewDetailsRequest() { Level = level, DomainName = domainName }, GetMetadata());
            return reply;
        }

        public async Task<GetNewOneTimeDetailsResponse> GetNewOneTimeDetails(Guid contentId, string domainName, double oneTimeAmount)
        {
            if (!IsLoggedIn)
                return null;

            try
            {
                var cents = (uint)Math.Round(oneTimeAmount * 100.0);
                var client = new PaymentInterface.PaymentInterfaceClient(nameHelper.PaymentServiceChannel);
                var reply = await client.GetNewOneTimeDetailsAsync(new() { InternalId = contentId.ToString(), DomainName = domainName, DifferentPresetPriceCents = cents }, GetMetadata());
                return reply;
            }
            catch
            {
                return null;
            }
        }

        public async Task<GetOwnSubscriptionRecordsResponse> GetOwnSubscriptionRecord()
        {
            if (!IsLoggedIn)
                return null;

            if (nameHelper.PaymentServiceChannel == null)
                return null;

            logger.LogWarning($"******Trying to hopefully connect to PaymentService at:({nameHelper.PaymentServiceChannel.Target})******");


            var client = new PaymentInterface.PaymentInterfaceClient(nameHelper.PaymentServiceChannel);
            var reply = await client.GetOwnSubscriptionRecordsAsync(new(), GetMetadata());
            return reply;
        }

        public async Task<GetOwnOneTimeRecordsResponse> GetOwnOneTimeRecords()
        {
            if (!IsLoggedIn)
                return null;

            if (nameHelper.PaymentServiceChannel == null)
                return null;

            logger.LogWarning($"******Trying to hopefully connect to PaymentService at:({nameHelper.PaymentServiceChannel.Target})******");


            var client = new PaymentInterface.PaymentInterfaceClient(nameHelper.PaymentServiceChannel);
            var reply = await client.GetOwnOneTimeRecordsAsync(new(), GetMetadata());
            return reply;
        }

        private Metadata GetMetadata()
        {
            var data = new Metadata();
            if (User != null && !string.IsNullOrWhiteSpace(User.JwtToken))
                data.Add("Authorization", "Bearer " + User.JwtToken);

            return data;
        }
    }
}
