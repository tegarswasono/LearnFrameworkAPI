﻿using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Datas.Entities.Master;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using System.Linq.Dynamic.Core;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/master/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AppAuthorize(AvailableModuleFunction.CategoryView)]
        public ActionResult<GeneralDatatableResponseModel<CategoryModel>> Index([FromQuery] GeneralDatatableRequestModel model)
        {
            try
            {
                var orderType = model.Descending ? "desc" : "asc";
                string OrderBy = "Name " + orderType;
                if (!string.IsNullOrEmpty(model.SortBy) && model.SortBy != "null")
                {
                    OrderBy = $"{model.SortBy} {orderType}";
                }
                int total = _context.Categories.Count();
                var categories = _context.Categories
                    .OrderBy(OrderBy)
                    .Skip((model.Page - 1) * model.RowsPerPage).Take(model.RowsPerPage)
                    .Select(x => CategoryModel.Dto(x))
                    .ToList();

                var result = GeneralDatatableResponseModel<CategoryModel>.Dto(categories, model, total);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"CategoryController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [AppAuthorize(AvailableModuleFunction.CategoryView)]
        public ActionResult<CategoryModel> GetById(Guid id)
        {
            try
            {
                var category = _context.Categories
                    .Where(x => x.Id == id)
                    .Select(x => CategoryModel.Dto(x))
                    .FirstOrDefault() ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Category"));
                return Ok(category);
            }
            catch (Exception ex)
            {
                Log.Error($"CategoryController.GetById | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("Create")]
        [AppAuthorize(AvailableModuleFunction.CategoryAdd)]
        public ActionResult<GeneralResponseMessage> Create(CategoryCreateModel model)
        {
            try
            {
                var exist = _context.Categories.FirstOrDefault(x => x.Name == model.Name);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Name));
                }

                var category = new Category()
                {
                    Name = model.Name,
                    Description = model.Description
                };
                _context.Categories.Add(category);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"CategoryController.Create | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPut("Update")]
        [AppAuthorize(AvailableModuleFunction.CategoryEdit)]
        public ActionResult<GeneralResponseMessage> Update(CategoryUpdateModel model)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == model.Id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Category"));
                var exist = _context.Categories.FirstOrDefault(x => x.Id != model.Id && x.Name == model.Name);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Name));
                }

                category.Name = model.Name;
                category.Description = model.Description;
                _context.Categories.Update(category);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"CategoryController.Update | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [AppAuthorize(AvailableModuleFunction.CategoryDelete)]
        public ActionResult<GeneralResponseMessage> Delete(Guid id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Category"));
                if (_context.Products.Any(x => x.CategoryId == id))
                {
                    throw new InvalidOperationException(ConstantString.ThisDataIsUsedInOtherTransaction);
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.DeleteSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"CategoryController.Delete | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
