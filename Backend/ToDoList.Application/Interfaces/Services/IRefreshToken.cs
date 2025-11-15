using ToDoList.Application.Contracts;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces.Services
{
	public interface IRefreshToken
	{
		RefreshToken CreateRefreshToken();

	}
}
