using LearnFrameworkApi.Module.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.WorkerService
{
    public class CurrentUserResolver : ICurrentUserResolver
    {
        public string CurrentId
        {
            get
            {
                return "";
            }
        }
        public string CurrentUsername
        {
            get
            {
                return "WorkerService";
            }
        }
        public string CurrentEmail
        {
            get
            {
                return "";
            }
        }
        public string CurrentFullname
        {
            get
            {
                return "";
            }
        }
        public string CurrentRoles
        {
            get
            {
                return "";
            }
        }
    }
}
