using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

                };
                _context.SmtpSettings.Add(smtpSetting);
            }
            if (_context.SystemConfigurations.FirstOrDefault() == null)
            {
                var systemConfiguration = new SystemConfiguration()
                {

                };
                _context.SystemConfigurations.Add(systemConfiguration);
            }
            _context.SaveChanges();
            return Ok(GeneralResponseMessage.ProcessSuccessfully());
        }
    }
}
