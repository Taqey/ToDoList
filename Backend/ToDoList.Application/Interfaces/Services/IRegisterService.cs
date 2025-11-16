using ToDoList.Application.Contracts;

namespace ToDoList.Application.Interfaces.Services
{
	public interface IRegisterService
	{
		Task<dtoRegisterResult> Register (dtoRegister dto);
	}
}
