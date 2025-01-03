﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SimpleWeb.Models.Comment
{
    public class AddCommentViewModel
    {
        public string ContentID { get; set; }
        public string ParentCommentID { get; set; }

        [Display(Name = "Comment")]
        [Required]
        [StringLength(1000, ErrorMessage = "{0} length must be less than {1}.")]
        public string CommentText { get; set; }
    }
}
