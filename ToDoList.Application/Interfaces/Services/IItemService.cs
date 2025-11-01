using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Contracts;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces.Services
{
	public interface IItemService
	{
		Task CreateItem(dtoItem item);
		Task<dtoItem> GetItem(int id);
		Task <List<dtoItem>> GetItems();
		Task UpdateItem(dtoItem item);
		Task DeleteItem(int id);

	}
}
