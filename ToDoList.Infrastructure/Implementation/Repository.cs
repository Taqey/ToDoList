using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Repositories;
using ToDoList.Infrastructure.Persistence;

namespace ToDoList.Infrastructure.Implementation
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly AppDbContext _context;

		public Repository(AppDbContext context)
		{
			_context = context;
		}
		public async Task<T> Create(T Entity)
		{
			var result = await _context.Set<T>().AddAsync(Entity);
			return result.Entity;
		}

		public async Task DeleteById(int id)
		{
			var entity = await _context.Set<T>().FindAsync(id);
			_context.Set<T>().Remove(entity);

		}


		public async Task<IEnumerable<T>> ReadAll()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<T> ReadById(int Id)
		{
			return await _context.Set<T>().FindAsync(Id);
		}

		public void Update(T Entity)
		{
			_context.Entry(Entity).State = EntityState.Modified;

		}
	}
}
