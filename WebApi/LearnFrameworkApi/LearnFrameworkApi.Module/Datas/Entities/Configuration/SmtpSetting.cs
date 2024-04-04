using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas.Entities.Configuration
{
    public class SmtpSetting : BaseEntity
    {
        [MaxLength(100)]
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        [MaxLength(100)]
        public string SmtpUser { get; set; } = string.Empty;
        [MaxLength(100)]
        public string SmtpPassword { get; set; } = string.Empty;
        public bool SmtpIsUseSsl { get; set; }
        public static SmtpSetting GetInstance(AppDbContext context)
        {
            return context.SmtpSettings.OrderBy(x => x.CreatedAt).FirstOrDefault()!;
        }
    }
}
