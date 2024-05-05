using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnFrameworkApi.Module.Helpers;

namespace LearnFrameworkApi.Module.Models.Configuration
{
    public class UserModel
    {
        public Guid Id {  get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public bool Active { get; set; }
        public string ActiveInString { get { return Active.ActiveToString(); } }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public static UserModel Dto(AppUser data)
        {
            return new UserModel
            {
                Id = data.Id,
                Username = data.UserName,
                Email = data.Email,
                FullName = data.FullName,
                Active = data.Active,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt
            };
        }
    }
    public class UserCreateModel
    {
        [Required]
        [MaxLength(512)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;
        public bool Active { get; set; }
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
