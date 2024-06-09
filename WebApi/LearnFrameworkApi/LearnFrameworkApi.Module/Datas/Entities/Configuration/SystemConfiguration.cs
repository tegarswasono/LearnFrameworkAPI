using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas.Entities.Configuration
{
    public class SystemConfiguration : BaseEntity
    {
        [MaxLength(50)]
        public string AppBaseUrl { get; set; } = string.Empty;
        [MaxLength(50)]
        public Guid DefaultRoleId {  get; set; }
        public AppRole DefaultRole { get; set; } = null!;

        [MaxLength(100)]
        public string ExampleSetting { get; set; } = string.Empty;
        public static SystemConfiguration GetInstance(AppDbContext context)
        {
            return context.SystemConfigurations.Include(x => x.DefaultRole).OrderBy(x => x.CreatedAt).FirstOrDefault()!;
        }
    }
}
