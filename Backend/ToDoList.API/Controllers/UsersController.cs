using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoList.API.Contracts;
using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;

namespace ToDoList.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly ILoginService _loginService;
		private readonly IRegisterService _registerService;

		public UsersController(ILoginService loginService,IRegisterService registerService)
		{
			_loginService = loginService;
			_registerService = registerService;
		}
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromForm]LoginDto loginDto)
		{
			var result=await _loginService.Login(loginDto.Username, loginDto.password);
			if (result.IsAuthenticated) {
				var token=new TokenResponse { Token=result.Token,ExpiresOn=result.ExpiresOn};
				return Ok(token);
			}
			return BadRequest(result.message);

		}
		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromForm] dtoRegister registerDto)
		{
			
			var result=await _registerService.Register(registerDto);
			if (result.IsRegistered)
			{
				return Ok(result.Message);
			}
			return BadRequest(result.Message);
		}
		[HttpPost("Refresh")]
		public async Task<IActionResult> Refresh()
		{
			var token = Request.Cookies["RefreshToken"];

			Console.WriteLine("Refresh Token from Cookie: " + token);

			var result = await _loginService.RefreshToken(token);

			return Ok(result);
		}

	}
}
