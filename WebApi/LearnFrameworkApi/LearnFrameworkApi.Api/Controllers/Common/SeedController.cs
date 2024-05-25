using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnFrameworkApi.Api.Controllers.Common
{
    [Route("api/common/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public SeedController(AppDbContext context, RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<GeneralResponseMessage>> Index()
        {
            InitSMTPSystemConfiguration();
            RefreshModuleFunction();
            RefreshMenu();
            await InitAppRole();
            await InitAppUser();

            _context.SaveChanges();
            return Ok(GeneralResponseMessage.ProcessSuccessfully());
        }

        [HttpGet("RunMigration")]
        public ActionResult<GeneralResponseMessage> RunMigration()
        {
            _context.Database.Migrate();
            return Ok(GeneralResponseMessage.ProcessSuccessfully());
        }

        private void InitSMTPSystemConfiguration()
        {
            if (!_context.SmtpSettings.Any())
            {
                var smtpSetting = new SmtpSetting()
                {
                    SmtpServer = "smtp.googlemail.com",
                    SmtpPort = 587,
                    SmtpUser = "anotherme336@gmail.com",
                    SmtpPassword = "epdvowvithbworvi",
                    SmtpIsUseSsl = true,
                };
                _context.SmtpSettings.Add(smtpSetting);
            }
            if (!_context.SystemConfigurations.Any())
            {
                var systemConfiguration = new SystemConfiguration()
                {
                    ExampleSetting = "Example"
                };
                _context.SystemConfigurations.Add(systemConfiguration);
            }
        }
        private void RefreshModuleFunction()
        {
            _context.Modules.ExecuteDelete();
            _context.ModuleFunctions.ExecuteDelete();
            var availableModuleFunctions = AvailableModuleFunction.GetAll();
            var modules = availableModuleFunctions.Select(x => x.Module).Distinct().ToList();
            foreach (var module in modules)
            {
                var module1 = new Module.Datas.Entities.Configuration.Module()
                {
                    Id = module,
                    Name = module,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "SYSTEM"
                };
                _context.Modules.Add(module1);
            }
            foreach (var data in availableModuleFunctions)
            {
                var function1 = new ModuleFunction()
                {
                    Id = data.Id,
                    ModuleId = data.Module,
                    Name = data.FunctionName,
                    Description = data.Id,
                    Order = data.Order,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "SYSTEM"
                };
                _context.ModuleFunctions.Add(function1);
            }
        }
        private void RefreshMenu()
        {
            _context.Menus.ExecuteDelete();
            var menu1 = new Menu()
            {
                Id = Guid.Parse("95894029-0AC2-4CF8-B962-FFB4B7CC9D64"),
                ParentId = null,
                Title = "Home",
                Url = "~/Home",
                Description = "Home",

                IconClass = "pe-7s-home",
                OrderIndex = 0,
                Visible = true,
                FunctionId = "",
                Section = "HOME"
            };

            var menu2 = new Menu()
            {
                Id = Guid.Parse("20CF2630-37D7-4F47-974A-6DC91C6A65CA"),
                ParentId = null,
                Title = "Bookings",
                Url = "~/Transaction/Bookings",
                Description = "Bookings",

                IconClass = "pe-7s-note2",
                OrderIndex = 1,
                Visible = true,
                FunctionId = "Bookings.View",
                Section = "TRANSACTIONS"
            };

            var menu3 = new Menu()
            {
                Id = Guid.Parse("E0E006AA-0F29-4B95-89B6-95C2799A2FAB"),
                ParentId = null,
                Title = "Master",
                Url = "",
                Description = "Master",

                IconClass = "pe-7s-notebook",
                OrderIndex = 2,
                Visible = true,
                FunctionId = "",
                Section = "MASTERS"
            };

            var menu4 = new Menu()
            {
                Id = Guid.Parse("CCCDDE7D-7954-4830-9E60-CF54340FD851"),
                ParentId = Guid.Parse("E0E006AA-0F29-4B95-89B6-95C2799A2FAB"),
                Title = "Products",
                Url = "~/Master/Products",
                Description = "Products",

                IconClass = "pe-7s-notebook",
                OrderIndex = 3,
                Visible = true,
                FunctionId = "Product.View",
                Section = "MASTERS"
            };

            var menu5 = new Menu()
            {
                Id = Guid.Parse("779031D0-C8A5-421F-8CD8-2FD7F36CFD79"),
                ParentId = Guid.Parse("E0E006AA-0F29-4B95-89B6-95C2799A2FAB"),
                Title = "Categories",
                Url = "~/Master/Categories",
                Description = "Categories",

                IconClass = "pe-7s-notebook",
                OrderIndex = 4,
                Visible = true,
                FunctionId = "Category.View",
                Section = "MASTERS"
            };

            var menu6 = new Menu()
            {
                Id = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                ParentId = null,
                Title = "Configuration",
                Url = "",
                Description = "Configuration",

                IconClass = "pe-7s-config",
                OrderIndex = 5,
                Visible = true,
                FunctionId = "",
                Section = "MASTERS"
            };

            var menu7 = new Menu()
            {
                Id = Guid.Parse("37D5FF11-F16B-4379-A616-6334028A4AE4"),
                ParentId = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                Title = "Users",
                Url = "~/Configuration/Users",
                Description = "Users",

                IconClass = "pe-7s-config",
                OrderIndex = 6,
                Visible = true,
                FunctionId = "Users.View",
                Section = "MASTERS"
            };

            var menu8 = new Menu()
            {
                Id = Guid.Parse("49102DD2-D79E-4B36-88B8-A6BCE797569F"),
                ParentId = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                Title = "Roles",
                Url = "~/Configuration/Roles",
                Description = "Roles",

                IconClass = "pe-7s-config",
                OrderIndex = 7,
                Visible = true,
                FunctionId = "Roles.View",
                Section = "MASTERS"
            };

            var menu9 = new Menu()
            {
                Id = Guid.Parse("E59A45E0-F311-4DE1-89C5-7814A10EAA39"),
                ParentId = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                Title = "SystemConfiguration",
                Url = "~/Configuration/SystemConfiguration",
                Description = "SystemConfiguration",

                IconClass = "pe-7s-config",
                OrderIndex = 8,
                Visible = true,
                FunctionId = "SystemConfiguration.View",
                Section = "MASTERS"
            };

            var menu10 = new Menu()
            {
                Id = Guid.Parse("8FB27C78-5697-4875-A5B5-357232685A5F"),
                ParentId = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                Title = "SMTPSetting",
                Url = "~/Configuration/SMTPSetting",
                Description = "SMTPSetting",

                IconClass = "pe-7s-config",
                OrderIndex = 9,
                Visible = true,
                FunctionId = "SMTPSetting.View",
                Section = "MASTERS"
            };

            var menu11 = new Menu()
            {
                Id = Guid.Parse("4705448D-FF73-4764-BC02-42F44BFF3849"),
                ParentId = null,
                Title = "MyProfile",
                Url = "~/Configuration/MyProfile",
                Description = "MyProfile",

                IconClass = "pe-7s-user",
                OrderIndex = 10,
                Visible = true,
                FunctionId = "",
                Section = "MASTERS"
            };

            _context.Menus.Add(menu1); _context.Menus.Add(menu2); _context.Menus.Add(menu3); _context.Menus.Add(menu4); _context.Menus.Add(menu5);
            _context.Menus.Add(menu6); _context.Menus.Add(menu7); _context.Menus.Add(menu8); _context.Menus.Add(menu9); _context.Menus.Add(menu10);
            _context.Menus.Add(menu11);
        }
        private async Task InitAppRole()
        {
            if (!_context.Roles.Any())
            {
                var appRole1 = new AppRole()
                {
                    Name = "Administrator",
                };
                var appRole2 = new AppRole()
                {
                    Name = "Buyer",
                };
                var appRole3 = new AppRole()
                {
                    Name = "Shop Owner",
                };
                await _roleManager.CreateAsync(appRole1);
                await _roleManager.CreateAsync(appRole2);
                await _roleManager.CreateAsync(appRole3);

                //roleFunction administrator
                var availableModuleFunctions = AvailableModuleFunction.GetAll();
                foreach (var data in availableModuleFunctions)
                {
                    var roleFunction = new RoleFunction()
                    {
                        RoleId = appRole1.Id,
                        FunctionId = data.Id
                    };
                    _context.RoleFunctions.Add(roleFunction);
                }

                //roleFunction Buyer
                var buyerRoleFunction1 = new RoleFunction()
                {
                    RoleId = appRole2.Id,
                    FunctionId = "Bookings.View"
                };
                _context.RoleFunctions.Add(buyerRoleFunction1);

                //roleFunction ShopOwner
                var shopRoleFunction0 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Bookings.View"
                };
                var shopRoleFunction1 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Category.Add"
                };
                var shopRoleFunction2 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Category.Delete"
                };
                var shopRoleFunction3 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Category.Edit"
                };
                var shopRoleFunction4 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Category.View"
                };

                var shopRoleFunction11 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Product.Add"
                };
                var shopRoleFunction12 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Product.Delete"
                };
                var shopRoleFunction13 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Product.Edit"
                };
                var shopRoleFunction14 = new RoleFunction()
                {
                    RoleId = appRole3.Id,
                    FunctionId = "Product.View"
                };

                _context.RoleFunctions.Add(shopRoleFunction0);

                _context.RoleFunctions.Add(shopRoleFunction1);
                _context.RoleFunctions.Add(shopRoleFunction2);
                _context.RoleFunctions.Add(shopRoleFunction3);
                _context.RoleFunctions.Add(shopRoleFunction4);

                _context.RoleFunctions.Add(shopRoleFunction11);
                _context.RoleFunctions.Add(shopRoleFunction12);
                _context.RoleFunctions.Add(shopRoleFunction13);
                _context.RoleFunctions.Add(shopRoleFunction14);
            }
        }
        private async Task InitAppUser()
        {
            if (!_context.Users.Any())
            {
                var appUser1 = new AppUser()
                {
                    Email = "tegar.s@weefer.co.id",
                    UserName = "tegar.s@weefer.co.id",
                    FullName = "Administrator",
                    Active = true,
                };
                var appUser2 = new AppUser()
                {
                    Email = "buyer@gmail.com",
                    UserName = "buyer@gmail.com",
                    FullName = "Buyer",
                    Active = true,
                };
                var appUser3 = new AppUser()
                {
                    Email = "shopOwner@gmail.com",
                    UserName = "shopOwner@gmail.com",
                    FullName = "Shop Owner",
                    Active = true,
                };

                await _userManager.CreateAsync(appUser1, "Admin1234!");
                await _userManager.CreateAsync(appUser2, "Admin1234!");
                await _userManager.CreateAsync(appUser3, "Admin1234!");

                await _userManager.AddToRoleAsync(appUser1, "Administrator");
                await _userManager.AddToRoleAsync(appUser2, "Buyer");
                await _userManager.AddToRoleAsync(appUser3, "Shop Owner");
            }
        }
    }
}
