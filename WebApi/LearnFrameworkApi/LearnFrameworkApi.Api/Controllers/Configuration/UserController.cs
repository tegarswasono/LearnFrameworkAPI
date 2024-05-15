using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using LearnFrameworkMvc.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/configuration/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UserController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AppAuthorize(AvailableModuleFunction.UsersView)]
        public ActionResult<GeneralDatatableResponseModel<UserModel>> Index([FromQuery]GeneralDatatableRequestModel model)
        {
            try
            {
                var orderType = model.Descending ? "desc" : "asc";
                string OrderBy = "Email " + orderType;
                if (!string.IsNullOrEmpty(model.SortBy) && model.SortBy != "null")
                {
                    OrderBy = $"{model.SortBy} {orderType}";
                }
                int total = _context.Users.Count();
                var users = _context.Users
                    .OrderBy(OrderBy)
                    .Skip((model.Page - 1) * model.RowsPerPage).Take(model.RowsPerPage)
                    .Select(x => UserModel.Dto(x))
                    .ToList();

                var result = GeneralDatatableResponseModel<UserModel>.Dto(users, model, total);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [AppAuthorize(AvailableModuleFunction.UsersView)]
        public ActionResult<UserModel> GetById(Guid id)
        {
            try
            {
                var user = _context.Users
                    .Select(x => UserModel.Dto(x))
                    .FirstOrDefault(x => x.Id == id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.GetById | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("Create")]
        [AppAuthorize(AvailableModuleFunction.UsersAdd)]
        public async Task<ActionResult<GeneralResponseMessage>> Create(UserCreateModel1 model)
        {
            try
            {
                var exist = _context.Users.FirstOrDefault(x => x.Email == model.Email);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Email));
                }

                var user = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.Email,

                    FullName = model.FullName,
                    Active = model.Active,
                };

                await _userManager.CreateAsync(user, model.Password);
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Create | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPut("Update")]
        [AppAuthorize(AvailableModuleFunction.UsersEdit)]
        public async Task<ActionResult<GeneralResponseMessage>> Update(UserUpdateModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString()) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                var exist = _context.Users.FirstOrDefault(x => x.Id != model.Id && x.Email == model.Email);

                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Email));
                }

                user.Email = model.Email;
                user.UserName = model.Email;
                user.FullName = model.FullName;
                user.Active = model.Active;

                await _userManager.UpdateAsync(user);
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Update | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [AppAuthorize(AvailableModuleFunction.UsersDelete)]
        public async Task<ActionResult<GeneralResponseMessage>> Delete(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString()) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                await _userManager.DeleteAsync(user);
                return Ok(GeneralResponseMessage.DeleteSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Delete | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
