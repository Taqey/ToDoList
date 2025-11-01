using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Contracts
{
	public class dtoList
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public List<dtoItem>? Items { get; set; } = new List<dtoItem>();
	}
}
