using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Helpers
{
    public static class Utils
    {
        public static string ActiveToString(this bool active)
        {
            if (active) return "Active";
            else return "Not Active";
        }
    }
}
