using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Datas.Entities.Configuration
{
    [Index(nameof(OrderIndex))]
    //[Index("FirstColumn", "SecondColumn", IsUnique = true, Name = "My_Unique_Index")] //Constraint multiple column
    public class Menu : BaseEntity
    {
        public Guid? ParentId { get; set; }
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Url { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(50)]
        public string IconClass { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public bool Visible { get; set; }
        [MaxLength(50)]
        public string FunctionId { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Section {  get; set; } = string.Empty;
    }
}
