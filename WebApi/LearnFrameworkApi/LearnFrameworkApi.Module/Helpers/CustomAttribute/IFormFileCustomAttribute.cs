using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers.CustomAttribute
{
    public class RequiredIFormFileAttribute : ValidationAttribute
    {
        public RequiredIFormFileAttribute()
        {

        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string displayName = validationContext.DisplayName;
            string memberName = validationContext.MemberName ?? string.Empty;

            if (value is null)
            {
                return new ValidationResult($"The {displayName} field is required.", new[] { memberName });
            }
            return ValidationResult.Success;
        }
    }
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string displayName = validationContext.DisplayName;
            string memberName = validationContext.MemberName ?? string.Empty;

            if (value is IFormFile file && file.Length > _maxFileSize)
            {
                return new ValidationResult($"The Maximum file size of {displayName} field is {_maxFileSize} bytes.", new[] { memberName });
            }
            return ValidationResult.Success;
        }
    }
    public class AllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionAttribute(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string memberName = validationContext.MemberName ?? string.Empty;

            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult($"{extension} is not allowed.", new[] { memberName });
                }
            }
            return ValidationResult.Success;
        }
    }
}
