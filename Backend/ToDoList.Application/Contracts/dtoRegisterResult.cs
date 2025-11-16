using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Application.Contracts
{
	public class dtoRegisterResult
	{
		public bool IsRegistered { get; set; } = false;
		public string Message { get; set; }
	}
}
