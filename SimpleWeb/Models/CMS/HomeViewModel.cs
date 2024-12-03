using ON.Authentication;
using ON.Fragments.Authentication;
using ON.Fragments.Authorization.Payment;
using ON.Fragments.Authorization.Payment.Stripe;
using ON.Fragments.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWeb.Models.CMS
{
    public class HomeViewModel
    {
        public HomeViewModel() { }

        public HomeViewModel(IEnumerable<ContentListRecord> records, ONUser user)
        {
            Records.AddRange(records);

            ShowLockStatus = !(user?.IsWriterOrHigher ?? false);
            UserSubscriptionLevel = user?.SubscriptionLevel ?? 0;
        }

        public bool ShowLockStatus { get; set; }

        public uint UserSubscriptionLevel { get; set; } = 0;

        public List<ContentListRecord> Records { get; } = new List<ContentListRecord>();
        public List<StripeOneTimePaymentRecord> OneTimeRecords { get; internal set; } = new();

        public bool HasPaidForContent(Guid contentId)
        {
            return OneTimeRecords.Any(r => r.InternalID == contentId.ToString());
        }

        public bool IsWritterOrHigher(ONUserHelper userHelper)
        {
            return userHelper.MyUser == null ? false : userHelper.MyUser.IsWriterOrHigher;
        }
    }
}
