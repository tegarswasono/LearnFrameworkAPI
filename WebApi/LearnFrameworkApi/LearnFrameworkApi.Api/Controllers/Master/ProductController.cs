using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Datas.Entities.Master;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/master/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<ProductModel>> Index()
        {
            try
            {
                var products = _context.Products
                    .Include(x => x.Category)
                    .OrderBy(x => x.Name)
                    .Select(x => ProductModel.Dto(x))
                    .ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                Log.Error($"ProductController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ProductModel> GetById(Guid id)
        {
            try
            {
                var product = _context.Products
                    .Include(x => x.Category)
                    .Where(x => x.Id == id)
                    .Select(x => ProductModel.Dto(x))
                    .FirstOrDefault() ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Product"));
                return Ok(product);
            }
            catch (Exception ex)
            {
                Log.Error($"ProductController.GetById | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("Create")]
        public ActionResult<GeneralResponseMessage> Create(ProductCreateModel model)
        {
            try
            {
                var exist = _context.Products.FirstOrDefault(x => x.Name == model.Name);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Name));
                }

                var product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Stok = model.Stok,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                };
                _context.Products.Add(product);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"ProductController.Create | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPut("Update")]
        public ActionResult<GeneralResponseMessage> Update(ProductUpdateModel model)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(x => x.Id == model.Id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Product"));
                var exist = _context.Products.FirstOrDefault(x => x.Id != model.Id && x.Name == model.Name);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Name));
                }

                product.Name = model.Name;
                product.Description = model.Description;
                product.Stok = model.Stok;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                _context.Products.Update(product);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"ProductController.Update | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<GeneralResponseMessage> Delete(Guid id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(x => x.Id == id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Product"));

                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.DeleteSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"ProductController.Delete | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
