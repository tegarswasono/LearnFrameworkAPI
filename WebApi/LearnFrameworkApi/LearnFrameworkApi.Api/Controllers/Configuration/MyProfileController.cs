using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using LearnFrameworkApi.Module.Services.Configuration;
using LearnFrameworkMvc.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public MyProfileController(AppDbContext context, ICurrentUserResolver currentUserResolver) 
        { 
            _context = context;
            _currentUserResolver = currentUserResolver;
        }

        [HttpGet]
        public ActionResult<UserModel> Index()
        {
            try
            {
                var user = _context.Users
                    .Where(x => x.Id == Guid.Parse(_currentUserResolver.CurrentId))
                    .Select(x => MyProfileModel.Dto(x))
                    .FirstOrDefault();
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPut("Update")]
        public ActionResult<GeneralResponseMessage> Update(MyProfileModelUpdate model)
        {
            try
            {
                var user = _context.Users
                    .Where(x => x.Id == Guid.Parse(_currentUserResolver.CurrentId))
                    .FirstOrDefault()!;

                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                _context.Users.Update(user);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.Update | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
