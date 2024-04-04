using LearnFrameworkApi.Module;
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
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<UserModel>> Index()
        {
            try
            {
                var users = _context.Users.Select(x => new UserModel()
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                    IsActive = x.IsActive
                }).ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public ActionResult<RoleModel> GetById(Guid id)
        {
            try
            {
                var roles = _context.Roles.Select(x => new RoleModel() { Id = x.Id, Name = x.Name }).FirstOrDefault(x => x.Id == id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Role"));
                return Ok(roles);
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.GetById | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("Create")]
        public ActionResult<GeneralResponseMessage> Create(RoleCreateModel model)
        {
            try
            {
                var exist = _context.Roles.FirstOrDefault(x => x.Name == model.Name);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Name));
                }

                var role = new AppRole()
                {
                    Name = model.Name
                };
                _context.Roles.Add(role);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Create | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("Update")]
        public ActionResult<GeneralResponseMessage> Update(RoleUpdateModel model)
        {
            try
            {
                var role = _context.Roles.FirstOrDefault(x => x.Id == model.Id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Role"));
                var exist = _context.Roles.FirstOrDefault(x => x.Id != model.Id && x.Name == model.Name);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Name));
                }

                role.Name = model.Name;
                _context.Roles.Update(role);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Update | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<GeneralResponseMessage> Delete(Guid id)
        {
            try
            {
                var role = _context.Roles.FirstOrDefault(x => x.Id == id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "Role"));

                _context.Roles.Remove(role);
                _context.SaveChanges();
                return Ok(GeneralResponseMessage.DeleteSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Delete | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }
    }
}
