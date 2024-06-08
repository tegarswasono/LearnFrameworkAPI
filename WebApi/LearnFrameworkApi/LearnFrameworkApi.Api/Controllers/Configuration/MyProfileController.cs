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
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<List<MyMenuModel>>> MyMenu()
        {
            try
            {
                Guid userId = Guid.Parse(_currentUserResolver.CurrentId);
                var menus = await _context.Menus
                    .Where(x =>
                            x.Visible &&
                            (x.FunctionId == "" || _context.RoleFunctions
                                .Any(y =>
                                    y.FunctionId == x.FunctionId &&
                                    _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).Contains(y.RoleId)
                                )
                            )
                        )
                    .OrderBy(x => x.OrderIndex)
                    .ToListAsync();

                //Method 1 : using foreach
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
                        if (menus.Any(x => x.ParentId == menu.Id || menu.Url != ""))
                        {
                            var newMenu = new MyMenuModelItem()
                            {
                                Title = menu.Title,
                                Icon = menu.IconClass,
                                Url = menu.Url
                            };
                            //subMenu
                            foreach (var subMenu in menus.Where(x => x.ParentId == menu.Id))
                            {
                                if(menus.Any(x=>x.ParentId == subMenu.Id || subMenu.Url != ""))
                                {
                                    var newSubmenu = new MyMenuModelItem()
                                    {
                                        Title = subMenu.Title,
                                        Icon = subMenu.IconClass,
                                        Url = subMenu.Url
                                    };
                                    //subMenu1
                                    foreach (var subMenu1 in menus.Where(x => x.ParentId == subMenu.Id))
                                    {
                                        var newSubMenu1 = new MyMenuModelItem()
                                        {
                                            Title = subMenu1.Title,
                                            Icon = subMenu1.IconClass,
                                            Url = subMenu1.Url
                                        };
                                        newSubmenu.Child.Add(newSubMenu1);
                                    }
                                    newMenu.Child.Add(newSubmenu);
                                }
                            }
                            newSection.Child.Add(newMenu);
                        }
                    }
                    result.Add(newSection);
                }

                //Method 2 : Using Linq
                var result2 = menus
                    .GroupBy(x => x.Section)
                    .Select(section => new MyMenuModel
                    {
                        Section = section.Key,
                        Child = menus
                            .Where(menu => menu.Section == section.Key && menu.ParentId == null && (menus.Any(x => x.ParentId == menu.Id) || menu.Url != ""))
                            .Select(menu => new MyMenuModelItem
                            {
                                Title = menu.Title,
                                Icon = menu.IconClass,
                                Url = menu.Url,
                                Child = menus
                                    .Where(subMenu => subMenu.ParentId == menu.Id && (menus.Any(x => x.ParentId == subMenu.Id) || subMenu.Url != ""))
                                    .Select(subMenu => new MyMenuModelItem
                                    {
                                        Title = subMenu.Title,
                                        Icon = subMenu.IconClass,
                                        Url = subMenu.Url,
                                        Child = menus
                                            .Where(subMenu1 => subMenu1.ParentId == subMenu.Id)
                                            .Select(subMenu1 => new MyMenuModelItem
                                            {
                                                Title = subMenu1.Title,
                                                Icon = subMenu1.IconClass,
                                                Url = subMenu1.Url
                                            }).ToList()
                                    }).ToList()
                            }).ToList()
                    }).ToList();

                return Ok(result2);
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.MyMenu | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("MyPermission")]
        public async Task<ActionResult<List<MyPermissionModel>>> MyPermission()
        {
            try
            {
                var functionIds = await _context.RoleFunctions
                .Where(x =>
                    _context.UserRoles.Where(x => x.UserId == Guid.Parse(_currentUserResolver.CurrentId!)).Select(x => x.RoleId).Contains(x.RoleId)
                )
                .Select(x => new MyPermissionModel() { FunctionId = x.FunctionId })
                .ToListAsync();
                return Ok(functionIds);
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.MyPermission | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
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
