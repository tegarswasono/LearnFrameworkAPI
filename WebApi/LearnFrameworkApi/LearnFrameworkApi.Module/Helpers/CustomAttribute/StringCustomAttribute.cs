using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers.CustomAttribute
{
    public class ValidatePasswordAttribute : ValidationAttribute
    {
        public ValidatePasswordAttribute()
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
                string value1 = value.ToString()!;
                if (value1.Length < 8)
                {
                    return new ValidationResult($"The {displayName} should be a minimum of 8 characters");
                }

                bool hasUppercase = false;
                bool hasLowercase = false;
                bool hasDigitOrSymbol = false;
                foreach (char c in value1)
                {
                    if (char.IsUpper(c))
                    {
                        hasUppercase = true;
                    }
                    else if (char.IsLower(c))
                    {
                        hasLowercase = true;
                    }
                    else if (char.IsDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c))
                    {
                        hasDigitOrSymbol = true;
                    }
                }
                if (!hasUppercase || !hasLowercase || !hasDigitOrSymbol)
                {
                    return new ValidationResult($"The {displayName} should be contain 1 uppercase letter, 1 lowercase letter, and a number or symbol");
                }
            }
            return ValidationResult.Success;
        }
    }
}
