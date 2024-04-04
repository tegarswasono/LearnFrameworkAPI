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
        [MaxLength(512)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    public class UserUpdateModel : UserCreateModel
    {
        [RequiredGuid]
        public Guid Id { get; set; }
    }
    public class UserCreateModel1 : UserCreateModel
    {
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
