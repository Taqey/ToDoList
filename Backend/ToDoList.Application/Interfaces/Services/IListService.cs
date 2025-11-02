using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Contracts;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Interfaces.Services
{
	public interface IListService
	{
		Task CreateList(dtoList list);
		Task AddItemToList(int listId, int itemId);
		Task RemoveItemToList(int listId, int itemId);
		Task UpdateList(dtoList list);
		Task RemoveList(int id);
		Task<dtoList> ShowList(int id);
		Task<List<dtoList>> ShowLists();
	}
}