using ToDoList.Application.Contracts;
using ToDoList.Domain.Entities;

namespace ToDoList.API.Contracts
{
	public class ListDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public List<dtoItem>? Items { get; set; } = new List<dtoItem>();

	}
}
