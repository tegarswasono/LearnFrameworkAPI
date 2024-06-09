using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
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
        private readonly AppDbContext _context;
        public GuestController(UserManager<AppUser> userManager, EmailService smtpClient, IMemoryCache memoryCache, AppDbContext context)
        {
            _userManager = userManager;
            _emailService = smtpClient;
            _memoryCache = memoryCache;
            _context = context;
        }

        [HttpPost("SendLinkResetPassword")]
        public async Task<ActionResult<GeneralResponseMessage>> SendLinkResetPassword(SendLinkResetPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, model.Email));
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                _memoryCache.Set(resetToken, user.Id, DateTimeOffset.Now.AddHours(1));
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
                catch (Exception) 
                { 
                    //ignore exception
                }

                return Ok(GeneralResponseMessage.Dto("Your password has been successfully reset. Please log in using your new password."));
            }
            catch (Exception ex)
            {
                Log.Error($"GuestController.SendLinkResetPassword | Message: {ex.Message} | InnerException: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("SendLinkSignUp")]
        public async Task<ActionResult<GeneralResponseMessage>> SendLinkSignUp(SendLinkSignUpModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    throw new InvalidOperationException($"{model.Email} already registered. Please login using your account");
                }

                string registrationToken = Utils.GenerateRandomString(15);
                while (true)
                {
                    if (_memoryCache.Get(registrationToken) != null)
                    {
                        registrationToken = Utils.GenerateRandomString(15);
                    }
                    else
                    {
                        break;
                    }
                }
                _memoryCache.Set(registrationToken, model.Email, DateTimeOffset.Now.AddHours(1));
                _emailService.SendLinkSignUp(model.Email, registrationToken);

                return Ok(GeneralResponseMessage.Dto("A registration link has been sent. Please check your email."));
            }
            catch (Exception ex)
            {
                Log.Error($"GuestController.SendLinkSignUp | Message: {ex.Message} | InnerException: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("IsValidRegistrationForm")]
        public ActionResult<IsValidRegistrationFormModelResponse> IsValidRegistrationForm(IsValidRegistrationFormModel model)
        {
            try
            {
                var response = _memoryCache.Get(model.RegistrationToken) ?? throw new InvalidOperationException("Registration Token Already Expired");
                var result = new IsValidRegistrationFormModelResponse()
                {
                    RegistrationToken = model.RegistrationToken,
                    Email = response!.ToString()!
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"GuestController.IsValidRegistrationForm | Message: {ex.Message} | InnerException: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("SubmitRegistrationForm")]
        public async Task<ActionResult> SubmitRegistrationForm(SubmitRegistrationFormModel model)
        {
            try
            {
                var isValid = model.IsValid();
                if (!isValid.IsValid)
                {
                    throw new InvalidOperationException(isValid.Message);
                }
                var response = _memoryCache.Get(model.RegistrationToken) ?? throw new InvalidOperationException("Registration Token Already Expired");
                string email = response!.ToString()!;

                var systemConfiguration = SystemConfiguration.GetInstance(_context);
                if (systemConfiguration.DefaultRoleId == null)
                {
                    throw new InvalidOperationException("Please setup the Default Role First");
                }

                var user = new AppUser()
                {
                    Email = email,
                    UserName = email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Active = true,
                    EmailConfirmed = true
                };
                var response1 = await _userManager.CreateAsync(user, model.Password);
                if (!response1.Succeeded)
                {
                    throw new InvalidOperationException(response1.Errors.FirstOrDefault()!.Description);
                }
                var response2 = await _userManager.AddToRoleAsync(user, systemConfiguration.DefaultRole!.Name!);
                if (!response2.Succeeded)
                {
                    throw new InvalidOperationException(response2.Errors.FirstOrDefault()!.Description);
                }

                return Ok(GeneralResponseMessage.Dto("Process successfully, please login using your account"));
            }
            catch (Exception ex)
            {
                Log.Error($"GuestController.SubmitRegistrationForm | Message: {ex.Message} | InnerException: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
