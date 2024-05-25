using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using LearnFrameworkApi.Module.Models.Common;
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
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public static RoleModel Dto(AppRole model) 
        { 
            return new RoleModel() { Id = model.Id, Name = model.Name, CreatedAt = model.CreatedAt, UpdatedAt = model.UpdatedAt };
        }
    }
    public class RoleCreateModel
    {
        [Required]
        [MaxLength(512)]
        public string Name { get; set; } = string.Empty;
        public List<RoleFunctionModel> RoleFunctions { get; set; } = [];
    }
    public class RoleUpdateModel : RoleCreateModel
    {
        [RequiredGuid]
        public Guid Id { get; set; }
    }
}
