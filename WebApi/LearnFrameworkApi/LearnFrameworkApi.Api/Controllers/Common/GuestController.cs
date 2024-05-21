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
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
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
        public async Task<ActionResult<GeneralResponseMessage>> IsValidResetToken(IsValidResetTokenModel model)
        {
            try
            {
                var userId = _memoryCache.Get(model.ResetToken) ?? throw new InvalidOperationException("Token is Expired");
                var user = await _userManager.FindByIdAsync(userId.ToString()!) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                bool isTokenValid = await _userManager.VerifyUserTokenAsync(user!, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", model.ResetToken);
                if (!isTokenValid)
                {
                    throw new InvalidOperationException("Token is Expired");
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
                var userId = _memoryCache.Get(model.ResetToken) ?? throw new InvalidOperationException("Token is Expired");
                var user = await _userManager.FindByIdAsync(userId.ToString()!) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                bool isTokenValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", model.ResetToken);
                if (!isTokenValid)
                {
                    throw new InvalidOperationException("Token is Expired");
                }
                var response = await _userManager.ResetPasswordAsync(user, model.ResetToken, model.NewPassword);
                if (response.Errors.Any())
                {
                    throw new InvalidOperationException(response.Errors.FirstOrDefault()!.Description);
                }

                try
                {
                    _memoryCache.Remove(model.ResetToken);
                }
                catch (Exception) { }

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
