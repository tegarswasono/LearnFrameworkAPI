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
        public static SystemConfigurationModel Dto(SystemConfiguration entity)
        {
            return new SystemConfigurationModel()
            {
                ExampleSetting = entity.ExampleSetting
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
