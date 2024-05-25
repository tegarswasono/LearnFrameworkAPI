using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using LearnFrameworkApi.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using System.Linq.Dynamic.Core;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/configuration/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(AppDbContext context, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AppAuthorize(AvailableModuleFunction.RolesView)]
        public ActionResult<GeneralDatatableResponseModel<RoleModel>> Index([FromQuery] GeneralDatatableRequestModel model)
        {
            try
            {
                var orderType = model.Descending ? "desc" : "asc";
                string OrderBy = "Name " + orderType;
                if (!string.IsNullOrEmpty(model.SortBy) && model.SortBy != "null")
                {
                    OrderBy = $"{model.SortBy} {orderType}";
                }
                int total = _context.Roles.Count();
                var roles = _context.Roles
                    .OrderBy(OrderBy)
                    .Skip((model.Page - 1) * model.RowsPerPage).Take(model.RowsPerPage)
                    .Select(x => RoleModel.Dto(x))
                    .ToList();

                var result = GeneralDatatableResponseModel<RoleModel>.Dto(roles, model, total);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"RoleController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [AppAuthorize(AvailableModuleFunction.RolesView)]
        public ActionResult<RoleModel> GetById(Guid id)
        {
            try
            {
                var role = _context.Roles.FirstOrDefault(x => x.Id == id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Role"));
                return Ok(RoleModel.Dto(role));
            }
            catch (Exception ex)
            {
                Log.Error($"RoleController.GetById | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("Create")]
        [AppAuthorize(AvailableModuleFunction.RolesAdd)]
        public async Task<ActionResult<GeneralResponseMessage>> Create(RoleCreateModel model)
        {
            try
            {
                var exist = _context.Roles.FirstOrDefault(x => x.Name == model.Name);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Name));
                }

                var role = new AppRole()
                {
                    Name = model.Name,
                    NormalizedName = model.Name.ToUpper()
                };
                
                await _roleManager.CreateAsync(role);
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"RoleController.Create | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPut("Update")]
        [AppAuthorize(AvailableModuleFunction.RolesEdit)]
        public async Task<ActionResult<GeneralResponseMessage>> Update(RoleUpdateModel model)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(model.Id.ToString()) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Role"));
                var exist = _context.Roles.FirstOrDefault(x => x.Id != model.Id && x.Name == model.Name);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Name));
                }

                role.Name = model.Name;
                role.NormalizedName = model.Name.ToUpper();
                await _roleManager.UpdateAsync(role);
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"RoleController.Update | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [AppAuthorize(AvailableModuleFunction.RolesDelete)]
        public async Task<ActionResult<GeneralResponseMessage>> Delete(Guid id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString()) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Role"));
                await _roleManager.DeleteAsync(role);
                return Ok(GeneralResponseMessage.DeleteSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"RoleController.Delete | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("GetAllModules")]
        [AppAuthorize(AvailableModuleFunction.RolesView)]
        public async Task<ActionResult<List<Module.Models.Common.ModuleFunctionModel>>> GetAllModules(Guid? roleId)
        {
            try
            {
                var moduleFunctions = await _context.ModuleFunctions
                    .OrderBy(x => x.ModuleId).ThenBy(x => x.Order)
                    .ToListAsync();
                var result = moduleFunctions
                    .GroupBy(x => x.ModuleId)
                    .Select(x => new Module.Models.Common.ModuleFunctionModel()
                    {
                        ModuleId = x.Key,
                        Items = x.Select(y => new Module.Models.Common.ModuleFunctionModelItem()
                        {
                            Id = y.Id,
                            Name = y.Name
                        }).ToList()
                    }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"RoleController.GetAllModules | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
