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

		public ItemService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task CreateItem(dtoItem item)
		{
			var Item=new Item { Name = item.Name ,IsCompleted=item.IsCompleted,Description=item.Description};
			await _unitOfWork.ItemRepository.Create(Item);
			await _unitOfWork.SaveChanges();

		}

		public async Task DeleteItem(int id)
		{
			await _unitOfWork.ItemRepository.DeleteById(id);
			await _unitOfWork.SaveChanges();

		}

		public async Task<dtoItem> GetItem(int id)
		{

			var item= await _unitOfWork.ItemRepository.ReadById(id);
			var Item=new dtoItem { Description = item.Description ,Name=item.Name,Id=item.Id,IsCompleted=item.IsCompleted };
			return Item;
		}

		public async Task<List<dtoItem>> GetItems()
		{
			var items=await _unitOfWork.ItemRepository.ReadAll();
			var Items=new List<dtoItem>();
			foreach (var item in items) {
				Items.Add(new dtoItem { Description = item.Description, Name = item.Name, Id = item.Id, IsCompleted = item.IsCompleted });
			}
			return Items;
		}

		public async Task UpdateItem(dtoItem item)
		{
			var Item = new Item { Name = item.Name, IsCompleted = item.IsCompleted, Description = item.Description,Id=item.Id };

			_unitOfWork.ItemRepository.Update(Item);
			await _unitOfWork.SaveChanges();
		}
	}
}
