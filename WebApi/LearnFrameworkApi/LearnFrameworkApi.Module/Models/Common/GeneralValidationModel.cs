using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Common
{
    public class GeneralValidationModel
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public static GeneralValidationModel Dto(bool isValid, string message)
        {
            return new GeneralValidationModel { IsValid = isValid, Message = message };
        }
    }
}
