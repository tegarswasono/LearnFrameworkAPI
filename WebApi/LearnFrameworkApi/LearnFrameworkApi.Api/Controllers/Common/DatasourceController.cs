using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Datas.Entities.Master;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using System.Linq.Dynamic.Core;

namespace LearnFrameworkApi.Api.Controllers.Common
{
    [Route("api/common/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class DatasourceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DatasourceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Category")]
        public ActionResult<List<GeneralDatasourceModel>> Category()
        {
            try
            {
                var result = _context.Categories.Select(x => new GeneralDatasourceModel() { Value = x.Id, Label = x.Name }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"DatasourceController.Category | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
