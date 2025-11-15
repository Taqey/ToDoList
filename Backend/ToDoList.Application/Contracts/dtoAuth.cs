using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Application.Contracts
{
	public class dtoAuth
	{
		public string message { get; set; }
		public bool IsAuthenticated { get; set; } = false;
		public string? Token { get; set; }
		public DateTime? ExpiresOn { get; set; }
	}
}
