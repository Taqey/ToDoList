using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Implementaion.Services
{
	public class LoginService : ILoginService
	{
		private readonly IUserService _userService;
		private readonly IJwtToken _jwtToken;
		private readonly IRefreshToken _refreshToken;
		private readonly IHttpContextAccessor _httpContext;

		public LoginService(IUserService userService,IJwtToken jwtToken,IRefreshToken refreshToken, IHttpContextAccessor httpContext )
		{
			_userService = userService;
			_jwtToken = jwtToken;
			_refreshToken = refreshToken;
			_httpContext = httpContext;
		}
		public async Task<dtoAuth> Login(string username, string password)
		{
			ApplicationUser user;

			// Get user by email or username
			if (username.Contains('@'))
				user = await _userService.GetByEmailAsync(username);
			else
				user = await _userService.GetByUsernameAsync(username);

			if (user == null)
				return new dtoAuth { message = "username or password is wrong" };

			if (!await _userService.CheckPasswordAsync(user, password))
				return new dtoAuth { message = "username or password is wrong" };

			// Create tokens
			var jwt = _jwtToken.CreateJwtToken(user);
			var refresh = _refreshToken.CreateRefreshToken();

			// Save refresh token in DB
			user.RefreshTokens.Add(refresh);
			await _userService.UpdateAsync(user);

			// Set cookie
			var cookieoptions = new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				Expires = refresh.ExpiresOn.ToUniversalTime(),
				SameSite = SameSiteMode.None
			};

			_httpContext.HttpContext.Response.Cookies.Append("RefreshToken", refresh.Token, cookieoptions);

			// return DTO
			return new dtoAuth
			{
				IsAuthenticated = true,
				Token = jwt.Token,
				ExpiresOn = jwt.ExpiresOn,
				message = "Logged in"
			};
		}
		public async Task<dtoAuth> RefreshToken(string token)
		{
			var user = await _userService.GetUserByToken(token);
			if (user == null)
			{
				return new dtoAuth { message = "Invalid Token" };
			}

			var refreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == token);
			if (refreshToken == null)
			{
				return new dtoAuth { message = "Token not found" };
			}

			if (!refreshToken.IsActive)
			{
				return new dtoAuth { message = "Inactive or expired token" };
			}

			refreshToken.RevokedOn = DateTime.UtcNow;

			var newRefreshToken = _refreshToken.CreateRefreshToken();
			user.RefreshTokens.Add(newRefreshToken);

			var newAccessToken = _jwtToken.CreateJwtToken(user);
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Secure = _httpContext.HttpContext.Request.IsHttps,
				SameSite = SameSiteMode.None,
				Expires = newRefreshToken.ExpiresOn.ToUniversalTime(),
				Path = "/"
			};
			_httpContext.HttpContext.Response.Cookies.Append("RefreshToken", newRefreshToken.Token, cookieOptions);


			await _userService.UpdateAsync(user);

			return new dtoAuth
			{
				IsAuthenticated = true,
				Token = newAccessToken.Token,
				ExpiresOn = newAccessToken.ExpiresOn,
				message="Refreshed"
			};
		}
	}
}
