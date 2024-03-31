using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas.Entities.Master
{
    public class Category : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;
        public ICollection<Product> Products { get; } = new List<Product>(); 
    }
}
