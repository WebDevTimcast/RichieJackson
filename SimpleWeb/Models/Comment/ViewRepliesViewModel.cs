using ON.Fragments.Comment;
using System.Collections.Generic;

namespace SimpleWeb.Models.Comment
{
    public class ViewRepliesViewModel
    {
        public CommentResponseRecord MainComment { get; set; }
        public ViewCommentsViewModel ViewComments { get; internal set; }
    }
}
