using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Domain.Repositories
{
	public interface IRepository<T> where T : class
	{
		Task<T> Create(T Entity);
		Task<T> ReadById(int Id);
		Task<IEnumerable<T>> ReadAll();
		void Update(T Entity);
		Task DeleteById(int Id);

	}
}
