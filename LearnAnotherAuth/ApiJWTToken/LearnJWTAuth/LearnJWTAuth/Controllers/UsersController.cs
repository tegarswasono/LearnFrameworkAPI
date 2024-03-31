using LearnJWTAuth.Models;
using LearnJWTAuth.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearnJWTAuth.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IJWTManagerRepository _jWTManager;

		public UsersController(IJWTManagerRepository jWTManager)
		{
			this._jWTManager = jWTManager;
		}

		[HttpGet]
		public List<string> Get()
		{
			string userId = User.FindFirst(ClaimTypes.Name)?.Value;

			var users = new List<string>
			{
				"Satinder Singh",
				"Amit Sarna",
				"Davin Jon"
			};
			return users;
		}

        [HttpGet("LoggedUser")]
        public string LoggedUser()
        {
            string userId = User.FindFirst(ClaimTypes.Name)?.Value;
            return userId;
        }

        [AllowAnonymous]
		[HttpPost]
		[Route("authenticate")]
		public IActionResult Authenticate(Users usersdata)
		{
			var token = _jWTManager.Authenticate(usersdata);

			if (token == null)
			{
				return Unauthorized();
			}

			return Ok(token);
		}
	}
}
