using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using LearnFrameworkApi.Module.Services.Configuration;
using LearnFrameworkApi.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Serilog;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/configuration/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class MyProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserResolver _currentUserResolver;
        private readonly UserManager<AppUser> _userManager;
        public MyProfileController(AppDbContext context, ICurrentUserResolver currentUserResolver, UserManager<AppUser> userManager) 
        { 
            _context = context;
            _currentUserResolver = currentUserResolver;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<UserModel>> Index()
        {
            try
            {
                var user = (await _userManager.FindByIdAsync(_currentUserResolver.CurrentId))!;
                var result = MyProfileModel.Dto(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<GeneralResponseMessage>> Update(MyProfileModelUpdate model)
        {
            try
            {
                var user = (await _userManager.FindByIdAsync(_currentUserResolver.CurrentId))!;
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;

                await _userManager.UpdateAsync(user);
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.Update | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<ActionResult<GeneralResponseMessage>> ChangePassword(MyProfileModelChangePassword model)
        {
            try
            {
                var isValid = model.IsValid();
                if (!isValid.IsValid)
                {
                    throw new InvalidOperationException(isValid.Message);
                }
                var user = (await _userManager.FindByIdAsync(_currentUserResolver.CurrentId))!;
                var response = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (response.Errors.Any())
                {
                    throw new InvalidOperationException(response.Errors.FirstOrDefault()!.Description);
                }
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.ChangePassword | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
