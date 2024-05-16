using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Helpers;
using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Configuration
{
    public class MyProfileModel
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool Active { get; set; }
        public string ActiveInString { get { return Active.ActiveToString(); } }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public static MyProfileModel Dto(AppUser model)
        {
            return new MyProfileModel
            {
                Id = model.Id,
                Username = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Active = model.Active,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }
    }
    public class MyProfileModelUpdate
    {
        [RequiredGuid]
        public Guid Id { get; set; }
        [Required]
        public string FullName { get; set; } = string.Empty;
        [MaxLength(512)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
