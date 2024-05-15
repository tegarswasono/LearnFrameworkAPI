using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Services.Configuration
{
    public interface ICurrentUserResolver
    {
        string CurrentUsername { get; }
        string CurrentAppRole { get; }
    }
}
