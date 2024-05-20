using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Common
{
    public class SendLinkResetPasswordModel
    {
        [Required]
        [ValidateEmail]
        public string Email { get; set; } = string.Empty;
    }
}
