using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Configuration
{
    public class RoleModel
    {
        public Guid Id {  get; set; }
        public string? Name { get; set; }
    }
    public class RoleCreateModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
    public class RoleUpdateModel : RoleCreateModel
    {
        [RequiredGuid]
        public Guid Id { get; set; }
    }
}
