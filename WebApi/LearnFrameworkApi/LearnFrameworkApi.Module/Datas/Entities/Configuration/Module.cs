using LearnFrameworkApi.Module.Datas.Entities.Master;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas.Entities.Configuration
{
    public class Module
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;
        public ICollection<ModuleFunction> ModuleFunctions { get; } = new List<ModuleFunction>();

        //audit
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        [MaxLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
