using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnFrameworkApi.Module.Helpers;
using System.Data;

namespace LearnFrameworkApi.Module.Models.Configuration
{
    public class UserModel
    {
        public Guid Id {  get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public bool Active { get; set; }
        public string ActiveInString { get { return Active.ActiveToString(); } }
        public List<RoleModel> Roles { get; set; } = [];
        public string RolesInString { get { return string.Join(", ", Roles.OrderBy(x => x.Name).Select(x => x.Name)); } }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class UserCreateModel
    {
        [Required]
        [MaxLength(512)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;
        [MaxLength(512)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool Active { get; set; }
        public List<RoleModel> Roles { get; set; } = [];
    }
    public class UserUpdateModel : UserCreateModel
    {
        [RequiredGuid]
        public Guid Id { get; set; }
    }
    public class UserCreateModel1 : UserCreateModel
    {
        [Required]
        [ValidatePassword]
        public string Password { get; set; } = string.Empty;
    }
}
