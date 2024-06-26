﻿using LearnFrameworkApi.Module.Helpers.CustomAttribute;
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
    public class IsValidResetTokenModel
    {
        [Required]
        public string ResetToken { get; set; } = string.Empty;
    }
    public class ResetPasswordModel
    {
        [Required]
        public string ResetToken { get; set; } = string.Empty;
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
            }
            return GeneralValidationModel.Dto(true, "");
        }
    }
    public class SendLinkSignUpModel
    {
        [Required]
        [ValidateEmail]
        public string Email { get; set; } = string.Empty;
    }
    public class IsValidRegistrationFormModel
    {
        [Required]
        public string RegistrationToken { get; set; } = string.Empty;
    }
    public class IsValidRegistrationFormModelResponse
    {
        public string RegistrationToken { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    public class SubmitRegistrationFormModel
    {
        [Required]
        public string RegistrationToken { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;
        [MaxLength(512)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [ValidatePassword]
        public string Password { get; set; } = string.Empty;
        [Required]
        [MaxLength(512)]
        [ValidatePassword]
        public string ConfirmPassword { get; set; } = string.Empty;

        public GeneralValidationModel IsValid()
        {
            if (Password != ConfirmPassword)
            {
                return GeneralValidationModel.Dto(false, "Password and ConfirmPassword should be same");
            }
            return GeneralValidationModel.Dto(true, "");
        }
    }
}
