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
            await InitAppRole();
            await InitAppUser();
            InitSMTPSystemConfiguration();
            RefreshModuleFunction();
            RefreshMenu();

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
                var defaultRole = _context.Roles.FirstOrDefault(x => x.Name == "Buyer")!;
                var systemConfiguration = new SystemConfiguration()
                {
                    AppBaseUrl = "http://localhost:5173",
                    DefaultRoleId = defaultRole.Id,
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
                OrderIndex = 0,
                Section = "Home",
                ParentId = null,
                Title = "Home",

                Url = "homeindex",
                IconClass = "home",
                Description = "Home",
                Visible = false,
                FunctionId = "",
            };

            var menu2 = new Menu()
            {
                Id = Guid.Parse("20CF2630-37D7-4F47-974A-6DC91C6A65CA"),
                OrderIndex = 1,
                Section = "Transactions",
                ParentId = null,
                Title = "Bookings",

                Url = "bookingindex",
                IconClass = "shopping_cart",
                Description = "Bookings",
                Visible = true,
                FunctionId = "Bookings.View",
            };

            var menu3 = new Menu()
            {
                Id = Guid.Parse("E0E006AA-0F29-4B95-89B6-95C2799A2FAB"),
                OrderIndex = 2,
                Section = "Configurations",
                ParentId = null,
                Title = "Masters",

                Url = "",
                IconClass = "assignment",
                Description = "Masters",
                Visible = true,
                FunctionId = "",
            };

            var menu4 = new Menu()
            {
                Id = Guid.Parse("CCCDDE7D-7954-4830-9E60-CF54340FD851"),
                OrderIndex = 3,
                Section = "Configurations",
                ParentId = Guid.Parse("E0E006AA-0F29-4B95-89B6-95C2799A2FAB"),
                Title = "Products",

                Url = "productindex",
                IconClass = "inventory",
                Description = "Products",
                Visible = true,
                FunctionId = "Product.View",
            };

            var menu5 = new Menu()
            {
                Id = Guid.Parse("779031D0-C8A5-421F-8CD8-2FD7F36CFD79"),
                OrderIndex = 4,
                Section = "Configurations",
                ParentId = Guid.Parse("E0E006AA-0F29-4B95-89B6-95C2799A2FAB"),
                Title = "Categories",

                Url = "categoryindex",
                IconClass = "category",
                Description = "Categories",
                Visible = true,
                FunctionId = "Category.View",
            };

            var menu6 = new Menu()
            {
                Id = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                OrderIndex = 5,
                Section = "Configurations",
                ParentId = null,
                Title = "Configurations",

                Url = "",
                IconClass = "settings",
                Description = "Configurations",
                Visible = true,
                FunctionId = "",
            };

            var menu7 = new Menu()
            {
                Id = Guid.Parse("37D5FF11-F16B-4379-A616-6334028A4AE4"),
                OrderIndex = 6,
                Section = "Configurations",
                ParentId = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                Title = "Users",

                Url = "userindex",
                IconClass = "group",
                Description = "Users",
                Visible = true,
                FunctionId = "Users.View",
            };

            var menu8 = new Menu()
            {
                Id = Guid.Parse("49102DD2-D79E-4B36-88B8-A6BCE797569F"),
                OrderIndex = 7,
                Section = "Configurations",
                ParentId = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                Title = "Roles",

                Url = "roleindex",
                IconClass = "key",
                Description = "Roles",
                Visible = true,
                FunctionId = "Roles.View",
            };

            var menu9 = new Menu()
            {
                Id = Guid.Parse("E59A45E0-F311-4DE1-89C5-7814A10EAA39"),
                OrderIndex = 8,
                Section = "Configurations",
                ParentId = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                Title = "System Configuration",

                Url = "systemconfigurationindex",
                IconClass = "manufacturing",
                Description = "System Configuration",
                Visible = true,
                FunctionId = "SystemConfiguration.View",
            };

            var menu10 = new Menu()
            {
                Id = Guid.Parse("8FB27C78-5697-4875-A5B5-357232685A5F"),
                OrderIndex = 9,
                Section = "Configurations",
                ParentId = Guid.Parse("60A40743-F4F9-4D2F-8647-A0E77A626933"),
                Title = "SMTP Setting",

                Url = "smtpsettingindex",
                IconClass = "mail_lock",
                Description = "SMTP Setting",
                Visible = true,
                FunctionId = "SMTPSetting.View",
            };

            var menu11 = new Menu()
            {
                Id = Guid.Parse("4705448D-FF73-4764-BC02-42F44BFF3849"),
                OrderIndex = 10,
                Section = "Configurations",
                ParentId = null,
                Title = "My Profile",

                Url = "myprofileindex",
                IconClass = "person",
                Description = "My Profile",
                Visible = true,
                FunctionId = "",
            };

            _context.Menus.Add(menu1); _context.Menus.Add(menu2); _context.Menus.Add(menu3); _context.Menus.Add(menu4); _context.Menus.Add(menu5);
            _context.Menus.Add(menu6); _context.Menus.Add(menu7); _context.Menus.Add(menu8); _context.Menus.Add(menu9); _context.Menus.Add(menu10);
            _context.Menus.Add(menu11);

            //Example
            var menu81 = new Menu()
            {
                Id = Guid.Parse("422B4163-2E90-4093-BB23-FF65A18EC03E"),
                OrderIndex = 81,
                Section = "Example",
                ParentId = null,
                Title = "Example",

                Url = "",
                IconClass = "science",
                Description = "Example",
                Visible = true,
                FunctionId = "",
            };
            var menu82 = new Menu()
            {
                Id = Guid.Parse("2213651F-EDB1-4EB5-BAED-E428268068F5"),
                OrderIndex = 81,
                Section = "Example",
                ParentId = Guid.Parse("422B4163-2E90-4093-BB23-FF65A18EC03E"),
                Title = "Example 1",

                Url = "example1index",
                IconClass = "science",
                Description = "Example 1",
                Visible = true,
                FunctionId = "",
            };
            _context.Menus.Add(menu81); _context.Menus.Add(menu82);

            //3 level menu example
            var menu91 = new Menu()
            {
                Id = Guid.Parse("53EFDED8-DFB4-436A-BF70-F536D788D387"),
                OrderIndex = 91,
                Section = "Example",
                ParentId = null,
                Title = "Menu 1",

                Url = "",
                IconClass = "settings",
                Description = "Menu",
                Visible = true,
                FunctionId = "",
            };
            var menu92 = new Menu()
            {
                Id = Guid.Parse("486C3BF1-1FE9-4A6D-A87B-5D2602DD3CA6"),
                OrderIndex = 92,
                Section = "Example",
                ParentId = null,
                Title = "Menu 2",

                Url = "bookingindex",
                IconClass = "settings",
                Description = "Menu",
                Visible = true,
                FunctionId = "",
            };
            var menu911 = new Menu()
            {
                Id = Guid.Parse("186C54A9-AA15-44A4-A1F8-59BD34338555"),
                OrderIndex = 91,
                Section = "Example",
                ParentId = Guid.Parse("53EFDED8-DFB4-436A-BF70-F536D788D387"),
                Title = "Menu 11",

                Url = "",
                IconClass = "settings",
                Description = "Menu",
                Visible = true,
                FunctionId = "",
            };
            var menu912 = new Menu()
            {
                Id = Guid.Parse("912E49A9-2990-4970-AF7B-1A7DC5B6569E"),
                OrderIndex = 91,
                Section = "Example",
                ParentId = Guid.Parse("53EFDED8-DFB4-436A-BF70-F536D788D387"),
                Title = "Menu 12",

                Url = "bookingindex",
                IconClass = "settings",
                Description = "Menu",
                Visible = true,
                FunctionId = "",
            };
            var menu9111 = new Menu()
            {
                Id = Guid.Parse("10F0ECB8-23CF-45FC-A62D-CD4373C109E1"),
                OrderIndex = 91,
                Section = "Example",
                ParentId = Guid.Parse("186C54A9-AA15-44A4-A1F8-59BD34338555"),
                Title = "Menu 111",

                Url = "bookingindex",
                IconClass = "settings",
                Description = "Menu",
                Visible = true,
                FunctionId = "",
            };
            var menu9112 = new Menu()
            {
                Id = Guid.Parse("F900E10D-8ECA-4DC1-A8A1-9101DB389F4F"),
                OrderIndex = 91,
                Section = "Example",
                ParentId = Guid.Parse("186C54A9-AA15-44A4-A1F8-59BD34338555"),
                Title = "Menu 112",

                Url = "bookingindex",
                IconClass = "settings",
                Description = "Menu",
                Visible = true,
                FunctionId = "",
            };
            _context.Menus.Add(menu91); _context.Menus.Add(menu92); 
            _context.Menus.Add(menu911); _context.Menus.Add(menu912);
            _context.Menus.Add(menu9111); _context.Menus.Add(menu9112);
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
