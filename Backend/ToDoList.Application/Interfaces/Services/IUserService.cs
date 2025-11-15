using System.Threading.Tasks;
using ToDoList.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ToDoList.Application.Interfaces.Services
{
	public interface IUserService
	{
		Task<ApplicationUser?> GetByEmailAsync(string email);
		Task<ApplicationUser?> GetByUsernameAsync(string username);
		Task<IdentityResult> CreateUserAsync(ApplicationUser applicationUser, string password);
		Task<bool> CheckPasswordAsync(ApplicationUser applicationUser, string Password);
		Task<IdentityResult> ChangePasswordAsync(ApplicationUser applicationUser, string oldPassword, string newPassword);
		Task<IdentityResult> ForgetPasswordAsync(ApplicationUser applicationUser, string newPassword);
		Task<ApplicationUser> GetUserByToken(string token);
		Task UpdateAsync(ApplicationUser user);
	}
}
