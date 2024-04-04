using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers.CustomAttribute
{
    public class RequiredGuidAttribute : ValidationAttribute
    {
        public RequiredGuidAttribute()
        {
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string displayName = validationContext.DisplayName;
            string memberName = validationContext.MemberName ?? string.Empty;

            if (value == null)
            {
                return new ValidationResult($"The {displayName} field is required.", new[] { memberName });
            }
            else
            {
                Guid userInput = (Guid)value;
                if (userInput == Guid.Empty)
                {
                    return new ValidationResult($"The {displayName} field is required.", new[] { memberName });
                }
            }
            return ValidationResult.Success;
        }
    }
}
