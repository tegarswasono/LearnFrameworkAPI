using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkMvc.Module;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnFrameworkApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SeedController(AppDbContext context) 
        { 
            _context = context;
        }

        [HttpGet]
        public ActionResult<GeneralResponseMessage> Index()
        {
            if (_context.SmtpSettings.FirstOrDefault() == null)
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
            if (_context.SystemConfigurations.FirstOrDefault() == null)
            {
                var systemConfiguration = new SystemConfiguration()
                {
                    ExampleSetting = "Example"
                };
                _context.SystemConfigurations.Add(systemConfiguration);
            }
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
                Title = "Home",
                Url = "~/Home",
                Description = "Home",
                IconClass = "pe-7s-home",
                OrderIndex = 0,
                Visible = true,
                Section = "HOME"
            };
        }
    }
}
