using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Domain.Repositories
{
	public interface IUnitOfWork
	{
		public IRepository<Item> ItemRepository { get; }
		public IRepository<List> ListRepository { get; }
		public IRepository<ItemDate> ItemDateRepository { get; }

		Task SaveChanges();

	}
}
