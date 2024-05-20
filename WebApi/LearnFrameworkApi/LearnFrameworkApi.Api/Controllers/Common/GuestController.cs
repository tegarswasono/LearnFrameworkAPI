using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Helpers;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Services.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Reflection.Metadata;

namespace LearnFrameworkApi.Api.Controllers.Common
{
    [Route("api/common/[controller]")]
    [ApiController]
    public class GuestController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService _emailService;
        private readonly IMemoryCache _memoryCache;
        public GuestController(UserManager<AppUser> userManager, EmailService smtpClient, IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _emailService = smtpClient;
            _memoryCache = memoryCache;
        }

        [HttpPost("SendLinkResetPassword")]
        public async Task<ActionResult<GeneralResponseMessage>> SendLinkResetPassword(SendLinkResetPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, model.Email));
                var resetToken = Utils.GenerateRandomNumberAsString(6);
                while (_memoryCache.TryGetValue(resetToken, out string? tokenValue) && tokenValue != null)
                {
                    resetToken = Utils.GenerateRandomNumberAsString(6);
                }
                _memoryCache.Set(resetToken, user.Id, DateTimeOffset.Now.AddMinutes(5));
                _emailService.SendResetPasswordLink(user.Email!, user.FullName, resetToken);

                return Ok(GeneralResponseMessage.Dto("A reset password link has been sent. Please check your email."));
            }catch(Exception ex)
            {
                Log.Error($"GuestController.SendLinkResetPassword | Message: {ex.Message} | InnerException: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("IsValidResetToken")]
        public ActionResult<GeneralResponseMessage> IsValidResetToken(IsValidResetTokenModel model)
        {
            try
            {
                var userId = _memoryCache.Get(model.ResetToken);
                if (userId == null)
                {
                    throw new InvalidOperationException("Token Is Expired");
                }
                return Ok(GeneralResponseMessage.Dto("Token is valid"));
            }
            catch (Exception ex)
            {
                Log.Error($"GuestController.SendLinkResetPassword | Message: {ex.Message} | InnerException: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<GeneralResponseMessage>> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var userId = _memoryCache.Get(model.ResetToken) ?? throw new InvalidOperationException("Token Is Expired");
                string userId1 = userId.ToString()!;
                var user = await _userManager.FindByIdAsync(userId1) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                var tmp = await _userManager.ResetPasswordAsync(user, userId1, model.NewPassword);

                return Ok(GeneralResponseMessage.Dto("Your password has been successfully reset. Please log in using your new password."));
            }
            catch (Exception ex)
            {
                Log.Error($"GuestController.SendLinkResetPassword | Message: {ex.Message} | InnerException: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
