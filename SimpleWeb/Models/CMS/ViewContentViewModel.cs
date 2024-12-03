using ON.Authentication;
using ON.Fragments.Content;
using ON.Fragments.Content.Stats;
using SimpleWeb.Models.Comment;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SimpleWeb.Models.CMS
{
    public class ViewContentViewModel
    {
        public ContentPublicRecord Record { get; set; }
        public ViewCommentsViewModel Comments { get; set; }
        public GetContentStatsResponse Stats { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public double OneTimeAmount { get; set; } = 0;

        public bool CanShowContent(ONUser user)
        {
            if ((user?.IsWriterOrHigher ?? false))
                return true;

            if (Record.Data.SubscriptionLevel > 0)
            {
                if (Record.Data.SubscriptionLevel <= (user?.SubscriptionLevel ?? 0))
                    return true;
            }


            if (Record.Data.OneTimeAmountCents > 0)
            {
                if ((Record.Data?.Video?.RumbleVideoId ?? "") != "")
                    return true;
                if ((Record.Data?.Video?.YoutubeVideoId ?? "") != "")
                    return true;
                if ((Record.Data?.Written?.HtmlBody ?? "") != "")
                    return true;
            }

            return false;
        }
    }
}
