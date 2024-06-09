using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using LearnFrameworkApi.Module;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/configuration/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class SystemConfigurationController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SystemConfigurationController(AppDbContext context) 
        { 
            _context = context;
        }

        [HttpGet]
        [AppAuthorize(AvailableModuleFunction.SystemConfigurationView)]
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

        [HttpPut("CreateOrUpdate")]
        [AppAuthorize(AvailableModuleFunction.SystemConfigurationCreateOrUpdate)]
        public ActionResult<GeneralResponseMessage> CreateOrUpdate(SystemConfigurationModelCreateOrUpdate model)
        {
            try
            {
                var systemConfiguration = SystemConfiguration.GetInstance(_context);
                if (systemConfiguration == null)
                {
                    systemConfiguration = new SystemConfiguration
                    {
                        AppBaseUrl = model.AppBaseUrl,
                        DefaultRoleId = model.DefaultRoleId,
                        ExampleSetting = model.ExampleSetting
                    };
                    _context.SystemConfigurations.Add(systemConfiguration);
                }
                else
                {
                    systemConfiguration.AppBaseUrl = model.AppBaseUrl;
                    systemConfiguration.DefaultRoleId = model.DefaultRoleId;
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

        [HttpGet("Datasource/Roles")]
        [AppAuthorize(AvailableModuleFunction.SystemConfigurationView)]
        public async Task<ActionResult<GeneralDatasourceModel>> DatasourceRoles()
        {
            try
            {
                var result = await _context.Roles
                    .OrderBy(x => x.Name)
                    .Select(x => new GeneralDatasourceModel() { Value = x.Id, Label = x.Name! })
                    .ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"SystemConfigurationController.DatasourceRoles | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
