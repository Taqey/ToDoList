using System.Security.Cryptography;
using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Implementation.Services
{
	public class Refresh : IRefreshToken
	{
		public RefreshToken CreateRefreshToken()
		{
			var rnd=new byte[32];
			using var rng=new RNGCryptoServiceProvider();
			rng.GetBytes(rnd);
			var refreshtoken = new RefreshToken { Token= Convert.ToBase64String(rnd),ExpiresOn=DateTime.UtcNow.AddDays(7) };
			return refreshtoken;	
		}
	}
}
