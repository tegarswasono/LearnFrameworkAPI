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
    public class SmtpSettingController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SmtpSettingController(AppDbContext context) 
        { 
            _context = context;
        }

        [HttpGet]
        public ActionResult<SmtpSettingModel> Index()
        {
            try
            {
                var smtpSetting = SmtpSetting.GetInstance(_context);
                var result = SmtpSettingModel.Dto(smtpSetting);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"SmtpSettingController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("CreateOrUpdate")]
        public ActionResult<GeneralResponseMessage> CreateOrUpdate(SmtpSettingModelCreateOrUpdate model)
        {
            try
            {
                var smtpSetting = SmtpSetting.GetInstance(_context);
                if (smtpSetting == null)
                {
                    smtpSetting = new SmtpSetting();
                    smtpSetting.SmtpServer = model.SmtpServer;
                    smtpSetting.SmtpPort = model.SmtpPort;
                    smtpSetting.SmtpUser = model.SmtpUser;
                    smtpSetting.SmtpPassword = model.SmtpPassword;
                    smtpSetting.SmtpIsUseSsl = model.SmtpIsUseSsl;
                    _context.SmtpSettings.Add(smtpSetting);
                }
                else
                {
                    smtpSetting.SmtpServer = model.SmtpServer;
                    smtpSetting.SmtpPort = model.SmtpPort;
                    smtpSetting.SmtpUser = model.SmtpUser;
                    smtpSetting.SmtpPassword = model.SmtpPassword;
                    smtpSetting.SmtpIsUseSsl = model.SmtpIsUseSsl;
                    _context.SmtpSettings.Update(smtpSetting);
                }
                _context.SaveChanges();

                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"SmtpSettingController.CreateOrUpdate | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
