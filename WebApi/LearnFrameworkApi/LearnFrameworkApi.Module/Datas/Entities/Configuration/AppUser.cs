using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas.Entities.Configuration
{
    public class AppUser : IdentityUser<Guid>
    {

        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        [MaxLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
