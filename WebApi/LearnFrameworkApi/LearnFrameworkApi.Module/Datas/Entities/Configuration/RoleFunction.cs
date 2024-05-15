using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas.Entities.Configuration
{
    [Index(nameof(RoleId))]
    [Index(nameof(FunctionId))]
    public class RoleFunction : BaseEntity
    {
        public Guid RoleId { get; set; }
        public AppRole Role { get; set; } = null!;
        public string FunctionId { get; set; } = string.Empty;
    }
}
