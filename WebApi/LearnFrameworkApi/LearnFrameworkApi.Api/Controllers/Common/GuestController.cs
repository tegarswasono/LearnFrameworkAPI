using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Services.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Reflection.Metadata;

namespace LearnFrameworkApi.Api.Controllers.Common
{
    [Route("api/common/[controller]")]
    [ApiController]
    public class GuestController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService _emailService;
        public GuestController(UserManager<AppUser> userManager, EmailService smtpClient)
        {
            _userManager = userManager;
            _emailService = smtpClient;
        }

        [HttpPost("SendLinkResetPassword")]
        public async Task<ActionResult<GeneralResponseMessage>> SendLinkResetPassword(SendLinkResetPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, model.Email));
                _emailService.SendResetPasswordLink(user.Email!, user.FullName, "ABCDE123");
                return Ok(GeneralResponseMessage.Dto("A reset password link has been sent. Please check your email."));
            }catch(Exception ex)
            {
                Log.Error($"GuestController.SendLinkResetPassword | Message: {ex.Message} | InnerException: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
