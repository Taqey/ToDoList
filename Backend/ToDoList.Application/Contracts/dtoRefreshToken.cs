using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Application.Contracts
{
	public class dtoRefreshToken
	{
		public string token {  get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ExpiresOn { get; set; }

	}
}
