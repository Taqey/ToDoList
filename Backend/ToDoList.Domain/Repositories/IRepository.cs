using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
		Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includes);
		Task<IEnumerable<T>> GetAllIncluding2(params Expression<Func<T, object>>[] includes);
		ValueTask<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate);

	}
}
