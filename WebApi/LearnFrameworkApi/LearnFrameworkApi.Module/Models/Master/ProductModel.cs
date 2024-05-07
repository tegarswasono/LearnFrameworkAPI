using LearnFrameworkApi.Module.Datas.Entities.Master;
using LearnFrameworkApi.Module.Helpers.CustomAttribute;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Configuration
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stok { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryModel Category { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public static ProductModel Dto(Product model)
        {
            return new ProductModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Stok = model.Stok,
                Price = model.Price,
                CategoryId = model.CategoryId,
                Category = CategoryModel.Dto(model.Category),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
            };
        }
    }

    public class ProductCreateModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Description { get; set; } = string.Empty;
        public int Stok { get; set; }
        [Precision(18, 2)]
        [Range(1, int.MaxValue, ErrorMessage = "Price " + ConstantString.ShouldBeBiggerThan0)]
        public decimal Price { get; set; }
        [RequiredGuid]
        public Guid CategoryId { get; set; }
    }
    public class ProductUpdateModel : ProductCreateModel
    {
        [RequiredGuid]
        public Guid Id { get; set; }
    }
}
