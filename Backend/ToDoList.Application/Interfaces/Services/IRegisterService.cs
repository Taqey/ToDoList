using ToDoList.Application.Contracts;

namespace ToDoList.Application.Interfaces.Services
{
	public interface IRegisterService
	{
		void Register (dtoRegister dto);
	}
}
