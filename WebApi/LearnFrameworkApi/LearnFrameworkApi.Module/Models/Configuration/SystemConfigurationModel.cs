using LearnFrameworkApi.Module.Datas.Entities.Configuration;
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
        public string ExampleSetting { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public static SystemConfigurationModel Dto(SystemConfiguration model)
        {
            return new SystemConfigurationModel()
            {
                ExampleSetting = model.ExampleSetting,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
            };
        }
    }
    public class SystemConfigurationModelCreateOrUpdate
    {
        [Required]
        [MaxLength(100)]
        public string ExampleSetting { get; set; } = string.Empty;
    }
}
