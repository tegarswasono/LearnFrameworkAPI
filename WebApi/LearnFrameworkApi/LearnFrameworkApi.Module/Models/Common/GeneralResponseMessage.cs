using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Common
{
    public class GeneralResponseMessage
    {
        public string Message { get; set; } = string.Empty;
        public static GeneralResponseMessage Dto(string message)
        {
            return new GeneralResponseMessage { Message = message };
        }
        public static GeneralResponseMessage ProcessSuccessfully()
        {
            return new GeneralResponseMessage { Message = "Process Successfully" };
        }
        public static GeneralResponseMessage DeleteSuccessfully()
        {
            return new GeneralResponseMessage { Message = "Delete Successfully" };
        }
    }
}
