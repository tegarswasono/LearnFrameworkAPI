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
                foreach (var roleFunction in model.RoleFunctions)
                {
                    foreach (var item in roleFunction.Items.Where(x => x.IsChecked))
                    {
                        var roleFunction1 = new RoleFunction()
                        {
                            RoleId = role.Id,
                            FunctionId = item.Id
                        };
                        _context.RoleFunctions.Add(roleFunction1);
                    }
                }
                _context.SaveChanges();
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

                _context.RoleFunctions.Where(x => x.RoleId == role.Id).ExecuteDelete();
                foreach (var roleFunction in model.RoleFunctions)
                {
                    foreach (var item in roleFunction.Items.Where(x => x.IsChecked))
                    {
                        var roleFunction1 = new RoleFunction()
                        {
                            RoleId = role.Id,
                            FunctionId = item.Id
                        };
                        _context.RoleFunctions.Add(roleFunction1);
                    }
                }
                _context.SaveChanges();
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

                _context.RoleFunctions.Where(x => x.RoleId == id).ExecuteDelete();
                _context.SaveChanges();
                await _roleManager.DeleteAsync(role);
                return Ok(GeneralResponseMessage.DeleteSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"RoleController.Delete | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("RoleFunctions/{roleId?}")]
        [AppAuthorize(AvailableModuleFunction.RolesView)]
        public async Task<ActionResult<List<RoleFunctionModel>>> RoleFunctions(Guid? roleId)
        {
            try
            {
                var moduleFunctionModels = await _context.ModuleFunctions
                    .OrderBy(x => x.ModuleId).ThenBy(x => x.Order)
                    .Select(x => new ModuleFunctionModel()
                    {
                        Id = x.Id,
                        ModuleId = x.ModuleId,
                        Name = x.Name,
                        Description = x.Description,
                        Order = x.Order,
                        IsChecked = roleId == null || roleId == Guid.Empty ? false : _context.RoleFunctions.Any(y => y.RoleId == roleId && y.FunctionId == x.Id)
                    })
                    .ToListAsync();

                var result = moduleFunctionModels
                    .GroupBy(x => x.ModuleId)
                    .Select(x => new RoleFunctionModel()
                    {
                        ModuleId = x.Key,
                        IsChecked = !x.Any(x => !x.IsChecked),
                        Items = x.Select(y => new RoleFunctionModelItem()
                        {
                            Id = y.Id,
                            Name = y.Name,
                            IsChecked = y.IsChecked

                        }).ToList()
                    }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"RoleController.RoleFunctions | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
