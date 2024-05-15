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
        string CurrentId { get; }
        string CurrentUsername { get; }
        string CurrentEmail { get; }
        string CurrentFullname { get; }
        string CurrentRoles { get; }
    }
}
