﻿using LearnFrameworkApi.Module;
using LearnFrameworkApi.Module.Datas;
using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using LearnFrameworkApi.Module.Models.Common;
using LearnFrameworkApi.Module.Models.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Serilog;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace LearnFrameworkApi.Api.Controllers.Configuration
{
    [Route("api/configuration/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UserController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AppAuthorize(AvailableModuleFunction.UsersView)]
        public ActionResult<GeneralDatatableResponseModel<UserModel>> Index([FromQuery]GeneralDatatableRequestModel model)
        {
            try
            {
                var orderType = model.Descending ? "desc" : "asc";
                string OrderBy = "Email " + orderType;
                if (!string.IsNullOrEmpty(model.SortBy) && model.SortBy != "null")
                {
                    OrderBy = $"{model.SortBy} {orderType}";
                }
                int total = _context.Users.Count();
                var users = _context.Users
                    .OrderBy(OrderBy)
                    .Skip((model.Page - 1) * model.RowsPerPage).Take(model.RowsPerPage)
                    .Select(x => new UserModel()
                    {
                        Id = x.Id,
                        Username = x.UserName,
                        Email = x.Email,
                        FullName = x.FullName,
                        PhoneNumber = x.PhoneNumber,
                        Active = x.Active,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        Roles = (from userRole in _context.UserRoles.Where(y => y.UserId == x.Id)
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 select new RoleModel()
                                 {
                                     Id = role.Id,
                                     Name = role.Name,
                                     CreatedAt = role.CreatedAt,
                                     UpdatedAt = role.UpdatedAt
                                 }
                                ).ToList()
                    })
                    .ToList();

                var result = GeneralDatatableResponseModel<UserModel>.Dto(users, model, total);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Index | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [AppAuthorize(AvailableModuleFunction.UsersView)]
        public ActionResult<UserModel> GetById(Guid id)
        {
            try
            {
                var user = _context.Users
                    .Select(x => new UserModel()
                    {
                        Id = x.Id,
                        Username = x.UserName,
                        Email = x.Email,
                        FullName = x.FullName,
                        PhoneNumber = x.PhoneNumber,
                        Active = x.Active,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        Roles = (from userRole in _context.UserRoles.Where(y => y.UserId == x.Id)
                                 join role in _context.Roles on userRole.RoleId equals role.Id
                                 select new RoleModel()
                                 {
                                     Id = role.Id,
                                     Name = role.Name,
                                     CreatedAt = role.CreatedAt,
                                     UpdatedAt = role.UpdatedAt
                                 }
                                ).ToList()
                    })
                    .FirstOrDefault(x => x.Id == id) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.GetById | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpGet("Datasource/Roles")]
        [AppAuthorize(AvailableModuleFunction.UsersView)]
        public async Task<ActionResult<RoleModel>> DatasourceRoles()
        {
            try
            {
                var result = await _context.Roles
                    .OrderBy(x => x.Name)
                    .Select(x => RoleModel.Dto(x))
                    .ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.DatasourceRoles | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPost("Create")]
        [AppAuthorize(AvailableModuleFunction.UsersAdd)]
        public async Task<ActionResult<GeneralResponseMessage>> Create(UserCreateModel1 model)
        {
            try
            {
                var exist = _context.Users.FirstOrDefault(x => x.Email == model.Email);
                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Email));
                }

                var user = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.Email,

                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Active = model.Active,
                };
                var response = await _userManager.CreateAsync(user, model.Password);
                if (!response.Succeeded)
                {
                    throw new InvalidOperationException(response.Errors.FirstOrDefault()!.Description);
                }

                model.Roles ??= [];
                foreach (var role in model.Roles)
                {
                    var response1 = await _userManager.AddToRoleAsync(user, role.Name!);
                    if (!response1.Succeeded) 
                    {
                        throw new InvalidOperationException(response1.Errors.FirstOrDefault()!.Description);
                    }
                }
                await _context.SaveChangesAsync();

                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Create | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpPut("Update")]
        [AppAuthorize(AvailableModuleFunction.UsersEdit)]
        public async Task<ActionResult<GeneralResponseMessage>> Update(UserUpdateModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString()) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                var exist = _context.Users.FirstOrDefault(x => x.Id != model.Id && x.Email == model.Email);

                if (exist != null)
                {
                    throw new InvalidOperationException(string.Format(ConstantString.DataAlreadyExist, model.Email));
                }

                user.Email = model.Email;
                user.UserName = model.Email;
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Active = model.Active;

                await _userManager.UpdateAsync(user);

                //handle userRole
                model.Roles ??= [];
                List<string> rolesDb = (await _userManager.GetRolesAsync(user)).ToList();
                if (rolesDb.Count == 0)
                {
                    foreach (var role in model.Roles)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name!);
                    }
                }
                else
                {
                    //Delete
                    foreach (var roleDb in rolesDb)
                    {
                        var any = model.Roles.Exists(x => x.Name == roleDb);
                        if (!any)
                        {
                            await _userManager.RemoveFromRoleAsync(user, roleDb);
                        }
                    }
                    //Insert
                    foreach (var role in model.Roles.Where(x=> !rolesDb.Exists(y=> y == x.Name)))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name!);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(GeneralResponseMessage.ProcessSuccessfully());
            }
            catch (Exception ex)
            {
                Log.Error($"UserController.Update | Message: {ex.Message} | Inner Exception: {ex.InnerException}");
                return BadRequest(GeneralResponseMessage.Dto(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [AppAuthorize(AvailableModuleFunction.UsersDelete)]
        public async Task<ActionResult<GeneralResponseMessage>> Delete(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString()) ?? throw new InvalidOperationException(string.Format(ConstantString.DataNotFound, "User"));
                var response = await _userManager.DeleteAsync(user);
                if (!response.Succeeded)
                {
                    throw new InvalidOperationException(response.Errors.FirstOrDefault()!.Description);
                }
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
