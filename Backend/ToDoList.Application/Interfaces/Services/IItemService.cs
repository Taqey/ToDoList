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
		Task CreateItem(dtoItem item,string UserId);
		Task<dtoItem> GetItem(int id);
		Task <List<dtoItem>> GetItems( string UserId);
		Task UpdateItem(dtoItem item,string UserId);
		Task DeleteItem(int id);

	}
}
