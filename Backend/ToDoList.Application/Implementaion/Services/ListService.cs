using ToDoList.Application.Contracts;
using ToDoList.Application.Interfaces.Services;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Repositories;

namespace ToDoList.Application.Implementaion.Services
{
	public class ListService : IListService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ListService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task AddItemToList(int listId, int itemId)
		{
			var list=await _unitOfWork.ListRepository.ReadById(listId);

			var item = await _unitOfWork.ItemRepository.ReadById(itemId);
			list.Items.Add(item);
			 _unitOfWork.ListRepository.Update(list);
			await _unitOfWork.SaveChanges();
		}
		public async Task RemoveItemToList(int listId, int itemId)
		{
			var lists = await _unitOfWork.ListRepository.GetAllIncluding(l => l.Items);
			var list = lists.FirstOrDefault(e => e.Id == listId);
			var item = list.Items.FirstOrDefault(e => e.Id == itemId);
			list.Items.Remove(item);
			_unitOfWork.ListRepository.Update(list);
			await _unitOfWork.SaveChanges();


		}

		public async Task CreateList(dtoList list, string UserId)
		{
			var List = new List { Description = list.Description, Name = list.Name,UserId=UserId };

			await _unitOfWork.ListRepository.Create(List);
			await _unitOfWork.SaveChanges();

		}


		public async Task RemoveList(int id)
		{
			await _unitOfWork.ListRepository.DeleteById(id);
			await _unitOfWork.SaveChanges();
		}

		public async Task<dtoList> ShowList(int id)
		{
			var lists= await _unitOfWork.ListRepository.GetAllIncluding2(l=>l.Items);
			var list= lists.FirstOrDefault(e=>e.Id==id);
			var dtoitems = list.Items.Select(x => new dtoItem {Id=x.Id,Description=x.Description,IsCompleted=x.IsCompleted,Name=x.Name }).ToList();
			var dtolist = new dtoList { Id = id, Description = list.Description, Name = list.Name, Items = dtoitems };
			return dtolist;

		}

		public async Task<List<dtoList>> ShowLists(string UserId)
		{
			var lists = await _unitOfWork.ListRepository.SearchInclude(l=>l.UserId==UserId,l=>l.Items);
			var dtoLists = new List<dtoList>();
			var items= new List<dtoItem>();
			foreach (var listEntity in lists)
			{
				var dtoItems = listEntity.Items.Select(item=>new dtoItem { Id=item.Id,Description=item.Description,IsCompleted=item.IsCompleted,Name=item.Name} ).ToList();
				var dtolist=new dtoList { Name = listEntity.Name,Description=listEntity.Description,Id=listEntity.Id,Items=dtoItems };
				dtoLists.Add(dtolist);
			}

			return dtoLists;
		}

		public async Task UpdateList(dtoList dto, string userId)
		{
			// 1. هات الـ List الأصلية من الداتا
			var list = await _unitOfWork.ListRepository.ReadById(dto.Id);

			if (list == null)
				throw new Exception("List not found");

			// 2. حدّث الخصائص الأساسية
			list.Name = dto.Name;
			list.Description = dto.Description;
			list.UserId = userId;

			// 3. تحديث الـ Items (Remove + Add + Update)
			list.Items.Clear();

			foreach (var x in dto.Items)
			{
				list.Items.Add(new Item
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					IsCompleted = x.IsCompleted,
					UserId = userId,
					ListId = list.Id
				});
			}

			// 4. Save changes
			await _unitOfWork.SaveChanges();
		}


	}
}
