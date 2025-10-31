using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces.Services
{
	public interface IItemService
	{
		Task CreateItem(Item item);
		Task GetItem();
		Task<Item> UpdateItem();
		Task DeleteItem(int id);

	}
}
