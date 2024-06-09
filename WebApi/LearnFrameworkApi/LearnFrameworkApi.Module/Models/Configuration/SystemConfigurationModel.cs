using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Configuration
{
    public class SystemConfigurationModel
    {
        public string AppBaseUrl { get; set; } = string.Empty;
        public Guid? DefaultRoleId { get; set; }
        public string ExampleSetting { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public static SystemConfigurationModel Dto(SystemConfiguration model)
        {
            return new SystemConfigurationModel()
            {
                AppBaseUrl = model.AppBaseUrl,
                DefaultRoleId = model.DefaultRoleId,
                ExampleSetting = model.ExampleSetting,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
            };
        }
    }
    public class SystemConfigurationModelCreateOrUpdate
    {
        [Required]
        [MaxLength(50)]
        public string AppBaseUrl { get; set; } = string.Empty;
        [RequiredGuid]
        public Guid DefaultRoleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ExampleSetting { get; set; } = string.Empty;
    }
}
