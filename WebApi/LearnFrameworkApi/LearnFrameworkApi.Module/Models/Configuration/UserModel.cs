using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Configuration
{
    public class UserModel
    {
        public Guid Id {  get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    public class UserCreateModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
    public class UserUpdateModel : UserCreateModel
    {
        [RequiredGuid]
        public Guid Id { get; set; }
    }
}
