using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Contracts;

namespace ToDoList.Application.Interfaces.Services
{
	public interface ILoginService
	{
		Task<dtoAuth> Login(string username, string password);
			}
}
