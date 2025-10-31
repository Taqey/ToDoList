using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Interfaces.Services;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Repositories;

namespace ToDoList.Application.Implementaion.Services
{
	internal class ItemService : IItemService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ItemService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task CreateItem(Item item)
		{
			await _unitOfWork.ItemRepository.Create(item);
			await _unitOfWork.SaveChanges();

		}

		public async Task DeleteItem(int id)
		{
			await _unitOfWork.ItemRepository.DeleteById(id);
			await _unitOfWork.SaveChanges();

		}

		public Task GetItem()
		{
			throw new NotImplementedException();
		}

		public async Task<Item> UpdateItem(Item item)
		{
			 _unitOfWork.ItemRepository.Update(item);
			await _unitOfWork.SaveChanges();
			return await _unitOfWork.ItemRepository.ReadById(item.Id);
		}
	}
}
