using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/configuration/[controller]")]
    [ApiController]
    public class SystemConfigurationController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SystemConfigurationController(AppDbContext context) 
        { 
            _context = context;
        }

        [HttpGet]
        public ActionResult<SystemConfigurationModel> Index()
        {
            try
            {
                var systemConfiguration = SystemConfiguration.GetInstance(_context);
                var result = SystemConfigurationModel.Dto(systemConfiguration);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"SystemConfigurationController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("CreateOrUpdate")]
        public ActionResult<GeneralResponseMessage> CreateOrUpdate(SystemConfigurationModelCreateOrUpdate model)
        {
            try
            {
                var systemConfiguration = SystemConfiguration.GetInstance(_context);
                if (systemConfiguration == null)
                {
                    systemConfiguration = new SystemConfiguration();
                    systemConfiguration.ExampleSetting = model.ExampleSetting;
                    _context.SystemConfigurations.Add(systemConfiguration);
                }
                else
                {
                    systemConfiguration.ExampleSetting = model.ExampleSetting;
                    _context.SystemConfigurations.Update(systemConfiguration);
                }
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"SystemConfigurationController.CreateOrUpdate | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
