using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Implementation.Services
{
	public class JwtToken : IJwtToken
	{
		private readonly IOptions<JwtSettings> _options;

		public JwtToken(IOptions<JwtSettings> options)
		{
			_options = options;
		}
		public dtoAuth CreateJwtToken(ApplicationUser user)
		{
			var claims = new List<Claim>();
			claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
			claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
			var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
			var credentials=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
			var endTime = DateTime.UtcNow.AddMinutes(15);
			var JwtToken = new JwtSecurityToken(
				issuer: _options.Value.Issuer,
				audience: _options.Value.Audience,
				claims: claims,
				signingCredentials: credentials,
				notBefore: DateTime.UtcNow,
				expires: endTime

				);
			var TokenString = new JwtSecurityTokenHandler().WriteToken(JwtToken);
			return new dtoAuth {Token=TokenString,ExpiresOn= endTime,IsAuthenticated=true,message="Login Successful" };
		}
	}
}
