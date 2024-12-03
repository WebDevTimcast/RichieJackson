using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWeb.Models.Auth
{
    public class RegisterViewModel
    {
        [Required, DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public double? Amount { get; set; } = 0;

        public string ContentId { get; set; } = string.Empty;

        public string ErrorMessage { get; set; }
    }
}
