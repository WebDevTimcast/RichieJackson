using ON.Fragments.Content;
using ON.Fragments.Content.Stats;
using SimpleWeb.Models.Comment;

namespace SimpleWeb.Models.CMS
{
    public class ViewContentViewModel
    {
        public ContentPublicRecord Record { get; set; }
        public ViewCommentsViewModel Comments { get; set; }
        public GetContentStatsResponse Stats { get; set; }
    }
}
