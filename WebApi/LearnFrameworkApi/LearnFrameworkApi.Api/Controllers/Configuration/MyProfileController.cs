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
        public ActionResult<UserModel> Index()
        {
            try
            {
                var user = _context.Users.OrderBy(x => x.Email).FirstOrDefault();
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error($"MyProfileController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
