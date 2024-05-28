using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Helpers;
using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using LearnFrameworkApi.Module.Models.Common;
using Microsoft.AspNetCore.Http;
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
        public string ProfilePicture { get; set; } = string.Empty;
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
                ProfilePicture = model.ProfilePicture,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
        }
    }
    public class MyProfileModelUpdate
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [MaxLength(512)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
    public class MyProfileModelChangePassword
    {
        [Required]
        [MaxLength(512)]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        [MaxLength(512)]
        [ValidatePassword]
        public string NewPassword { get; set; } = string.Empty;
        [Required]
        [MaxLength(512)]
        [ValidatePassword]
        public string ConfirmPassword { get; set; } = string.Empty;

        public GeneralValidationModel IsValid()
        {
            if (NewPassword != ConfirmPassword)
            {
                return GeneralValidationModel.Dto(false, "NewPassword and ConfirmPassword should be same");
            }else if (CurrentPassword == NewPassword)
            {
                return GeneralValidationModel.Dto(false, "CurrentPassword and NewPassword should be different");
            }
            return GeneralValidationModel.Dto(true, "");
        }
    }
    public class MyProfileModelChangeProfilePicture
    {
        [MaxFileSize(4*1000*1000)]
        [AllowedExtension([".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".svg", ".webp", ".heic", ".ico"])]
        public IFormFile? File { get; set; }
    }
}
