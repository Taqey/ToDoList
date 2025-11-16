using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Repositories;

namespace ToDoList.Application.Implementaion.Services
{
	public class ItemService : IItemService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserService _userService;

		public ItemService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task CreateItem(dtoItem item, string userId)
		{
			var newItem = new Item
			{
				Name = item.Name,
				Description = item.Description,
				IsCompleted = item.IsCompleted,
				UserId = userId  
			};

			await _unitOfWork.ItemRepository.Create(newItem);
			await _unitOfWork.SaveChanges();
		}


		public async Task DeleteItem(int id)
		{
			await _unitOfWork.ItemRepository.DeleteById(id);
			await _unitOfWork.SaveChanges();

		}

		public async Task<dtoItem> GetItem(int id)
		{

			var item = await _unitOfWork.ItemRepository.ReadById(id);
			var Item = new dtoItem { Description = item.Description, Name = item.Name, Id = item.Id, IsCompleted = item.IsCompleted };
			return Item;
		}

		public async Task<List<dtoItem>> GetItems(string UserId)
		{
			var items = (await _unitOfWork.ItemRepository.Search(e => e.UserId == UserId)).Select(item => new dtoItem
			{
				Id = item.Id,
				Name = item.Name,
				Description = item.Description,
				IsCompleted = item.IsCompleted
			}).ToList();

			return items;
		}

		public async Task UpdateItem(dtoItem item, string UserId)
		{
			var Item = new Item { Name = item.Name, IsCompleted = item.IsCompleted, Description = item.Description, Id = item.Id,UserId=UserId };

			_unitOfWork.ItemRepository.Update(Item);
			await _unitOfWork.SaveChanges();
		}
	}
}
