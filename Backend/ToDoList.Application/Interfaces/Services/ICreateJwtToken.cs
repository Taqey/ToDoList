using ToDoList.Application.Contracts;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces.Services
{
	public interface IJwtToken
	{
		dtoAuth CreateJwtToken(ApplicationUser user);
	}
}
