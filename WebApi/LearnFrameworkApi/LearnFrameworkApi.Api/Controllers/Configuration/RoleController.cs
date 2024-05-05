using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Linq.Dynamic.Core;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/configuration/[controller]")]
    [ApiController]
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
        public ActionResult<GeneralDatatableResponseModel<RoleModel>> Index([FromQuery] GeneralDatatableRequestModel model)
        {
            try
            {
                var orderType = model.Descending ? "desc" : "asc";
                string OrderBy = "Email " + orderType;
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
    }
}
