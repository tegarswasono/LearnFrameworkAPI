using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LearnFrameworkApi.Module.Helpers.CustomAttribute
{
    public class RequiredDateTimeAttribute : ValidationAttribute
    {
        public RequiredDateTimeAttribute()
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
                DateTime userInput = (DateTime)value;
                if (userInput == DateTime.MinValue)
                {
                    return new ValidationResult($"The {displayName} field is required.", new[] { memberName });
                }
            }
            return ValidationResult.Success;
        }
    }

    public class DateGreaterThanNowAttribute : ValidationAttribute
    {
        public DateGreaterThanNowAttribute()
        {
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime now = DateTime.Now;
                DateTime dateEntered = (DateTime)value;
                string displayName = validationContext.DisplayName;
                string memberName = validationContext.MemberName ?? string.Empty;

                if (dateEntered.Date <= now.Date)
                {
                    ErrorMessage = $"{displayName} must be greater than '{now.Date:yyyy-MM-dd}'.";
                    return new ValidationResult(ErrorMessage, new[] { memberName });
                }
            }
            return ValidationResult.Success;
        }
    }
    public class DateGreaterThanOrEqualsNowAttribute : ValidationAttribute
    {
        public DateGreaterThanOrEqualsNowAttribute()
        {
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime now = DateTime.Now;
                DateTime dateEntered = (DateTime)value;
                string displayName = validationContext.DisplayName;
                string memberName = validationContext.MemberName ?? string.Empty;

                if (dateEntered.Date < now.Date)
                {
                    ErrorMessage = $"{displayName} must be greater than or equals to '{now.Date:yyyy-MM-dd}'.";
                    return new ValidationResult(ErrorMessage, new[] { memberName });
                }
            }
            return ValidationResult.Success;
        }
    }
    public class DateLowerThanNowAttribute : ValidationAttribute
    {
        public DateLowerThanNowAttribute()
        {
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime now = DateTime.Now;
                DateTime dateEntered = (DateTime)value;
                string displayName = validationContext.DisplayName;
                string memberName = validationContext.MemberName ?? string.Empty;

                if (dateEntered.Date >= now.Date)
                {
                    ErrorMessage = $"{displayName} must be lower than '{now.Date:yyyy-MM-dd}'.";
                    return new ValidationResult(ErrorMessage, new[] { memberName });
                }
            }
            return ValidationResult.Success;
        }
    }
    public class DateLowerThanOrEqualsNowAttribute : ValidationAttribute
    {
        public DateLowerThanOrEqualsNowAttribute()
        {
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime now = DateTime.Now;
                DateTime dateEntered = (DateTime)value;
                string displayName = validationContext.DisplayName;
                string memberName = validationContext.MemberName ?? string.Empty;

                if (dateEntered.Date > now.Date)
                {
                    ErrorMessage = $"{displayName} must be lower than or equals to '{now.Date:yyyy-MM-dd}'.";
                    return new ValidationResult(ErrorMessage, new[] { memberName });
                }
            }
            return ValidationResult.Success;
        }
    }
}
