using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Configuration
{
    public class SmtpSettingModel
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public bool SmtpIsUseSsl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public static SmtpSettingModel Dto(SmtpSetting model)
        {
            return new SmtpSettingModel()
            {
                SmtpServer = model.SmtpServer,
                SmtpPort = model.SmtpPort,
                SmtpUser = model.SmtpUser,
                SmtpPassword = model.SmtpPassword,
                SmtpIsUseSsl = model.SmtpIsUseSsl
            };
        }
    }
    public class SmtpSettingModelCreateOrUpdate
    {
        [Required]
        [MaxLength(100)]
        public string SmtpServer { get; set; } = string.Empty;
        [Required]
        public int SmtpPort { get; set; }
        [Required]
        [MaxLength(100)]
        public string SmtpUser { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string SmtpPassword { get; set; } = string.Empty;
        public bool SmtpIsUseSsl { get; set; }
    }
}
