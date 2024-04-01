using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas.Entities.Configuration
{
    [Index(nameof(Order), IsUnique = true)]
    public class ModuleFunction
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ModuleId { get; set; } = string.Empty;
        public Module Module { get; set; } = null!;
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }

        //audit
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        [MaxLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
