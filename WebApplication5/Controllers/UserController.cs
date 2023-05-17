using Microsoft.AspNetCore.Mvc;
using WebApplication5.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using System.Text;
using System.Security.Cryptography; // same with bcrypt to hash password.

namespace WebApplication5.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	// hashing password using password hasher not bcrypt. through implementation IPasswordHasher.
	public class UserController : ControllerBase
	{
		private readonly DotnetAuthenticationContext _DBContext;
		public UserController(DotnetAuthenticationContext dbContext)
		{
			_DBContext = dbContext;
		}

		// create user (register function).
		[HttpPost("create")]
		public async Task<IActionResult> CreateUser(UserDTO request)
		{
			// find existing email. if email already exists will send message.
			var existingUser = await _DBContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
			if (existingUser != null)
			{
				return BadRequest(new { email = "Email already exists!" });
			}

			var newUser = new User
			{
				Username = request.Username,
				Email = request.Email,
				Password = request.Password
			};

			// check password is match or not.
			if (request.Password != request.PasswordConfirm)
			{
				return BadRequest("Password confirmation does not match!");
			}

			newUser.Password = HashPassword(newUser.Password);

			await _DBContext.Users.AddAsync(newUser);
			await _DBContext.SaveChangesAsync();

			return Ok(newUser);
		}

		// hashing passowrd function.
		private string HashPassword(string password)
		{
			using var sha256 = SHA256.Create();
			var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Encoding.UTF8.GetString(hashedBytes);
		}
	}
}