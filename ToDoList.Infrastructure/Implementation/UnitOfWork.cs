using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;
using ToDoList.Domain.Repositories;
using ToDoList.Infrastructure.Persistence;

namespace ToDoList.Infrastructure.Implementation
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;

		public IRepository<Item> ItemRepository { get; }
		public IRepository<List> ListRepository { get; }
		public IRepository<ItemDate> ItemDateRepository { get; }

		public UnitOfWork(AppDbContext context,IRepository<Item> ItemRepository, IRepository<List> ListRepository, IRepository<ItemDate> ItemDateRepository)
		{
			_context = context;
			this.ItemRepository = ItemRepository;
			this.ListRepository = ListRepository;
			this.ItemDateRepository = ItemDateRepository;
		}


		public async Task SaveChanges()
		{
		await _context.SaveChangesAsync();
		}
	}
}
