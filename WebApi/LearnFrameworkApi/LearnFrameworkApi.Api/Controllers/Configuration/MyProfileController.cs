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
using LearnFrameworkApi.Module.Helpers;

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

        [HttpGet("MyMenu")]
        public async Task<ActionResult> MyMenu()
        {
            try
            {
                var user = (await _userManager.FindByIdAsync(_currentUserResolver.CurrentId))!;
                var menus = _context.Menus
                    .Where(x => x.Visible)
                    .OrderBy(x => x.OrderIndex)
                    .ToList();


                var result = new List<MyMenuModel>();
                //section
                foreach (var section in menus.GroupBy(x => x.Section))
                {
                    var newSection = new MyMenuModel()
                    {
                        Section = section.Key,
                    };
                    //menu
                    foreach (var menu in menus.Where(x => x.Section == section.Key && x.ParentId == null))
                    {
                        var newMenu = new MyMenuModelItem()
                        {
                            Title = menu.Title,
                            Icon = menu.IconClass,
                            Url = menu.Url
                        };
                        //subMenu
                        if (menus.Any(x => x.ParentId == menu.Id))
                        {
                            foreach (var subMenu in menus.Where(x => x.ParentId == menu.Id))
                            {
                                var newSubmenu = new MyMenuModelItem()
                                {
                                    Title = subMenu.Title,
                                    Icon = subMenu.IconClass,
                                    Url = subMenu.Url
                                };
                                newMenu.Child.Add(newSubmenu);
                            }
                        }
                        newSection.Child.Add(newMenu);
                    }
                    result.Add(newSection);
                }
                //var result = menus.GroupBy(x => x.Section).Select(x => new MyMenuModel()
                //{
                //    Section = x.Key,
                //    Child = x.Select(y => new MyMenuModelItem()
                //    {
                //        Title = y.Title,
                //        Icon = y.IconClass,
                //        Url = y.Url
                //    }).ToList()
                //});
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.MyMenu | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
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

        [HttpPut("ChangeProfilePicture")]
        public async Task<ActionResult<GeneralResponseMessage>> ChangeProfilePicture(MyProfileModelChangeProfilePicture model)
        {
            try
            {
                var user = (await _userManager.FindByIdAsync(_currentUserResolver.CurrentId))!;
                if (model.File == null)
                {
                    if (!string.IsNullOrEmpty(user.ProfilePicture))
                    {
                        bool exist2 = System.IO.File.Exists(ConstantString.PathProfilePicture + user.ProfilePicture);
                        if (exist2)
                        {
                            System.IO.File.Delete(ConstantString.PathProfilePicture + user.ProfilePicture);
                        }
                    }
                    user.ProfilePicture = "";
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    var file = model.File!;
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string fileName = user.Id + extension;

                    //Delete
                    bool exist1 = System.IO.File.Exists(ConstantString.PathProfilePicture + fileName);
                    if (exist1)
                    {
                        System.IO.File.Delete(ConstantString.PathProfilePicture + fileName);
                    }
                    if (!string.IsNullOrEmpty(user.ProfilePicture))
                    {
                        bool exist2 = System.IO.File.Exists(ConstantString.PathProfilePicture + user.ProfilePicture);
                        if (exist2)
                        {
                            System.IO.File.Delete(ConstantString.PathProfilePicture + user.ProfilePicture);
                        }
                    }

                    //Upload
                    var filePath = Path.Combine(ConstantString.PathProfilePicture, fileName);
                    using (var stream = System.IO.File.Create(filePath))
                        await file.CopyToAsync(stream);

                    user.ProfilePicture = fileName;
                    await _userManager.UpdateAsync(user);
                }
                return Ok(GeneralResponseMessage.Dto("Process Successfully, Please hard refresh your page"));
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.ChangeProfilePicture | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(ex.Message);
            }
        }
    }
}
