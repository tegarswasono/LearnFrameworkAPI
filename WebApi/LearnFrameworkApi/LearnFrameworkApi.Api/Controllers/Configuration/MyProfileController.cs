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
    public class MyProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MyProfileController(AppDbContext context) 
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
    }
}
