	using Microsoft.EntityFrameworkCore;
	using System.Linq.Expressions;
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
		public async ValueTask<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate) => await _context.Set<T>().Where(predicate).ToListAsync();
		public async ValueTask<IEnumerable<T>> SearchInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _context.Set<T>().AsQueryable();
			foreach (var include in includes)
			{
				query = query.Include(include);
			}
			return await query.Where(predicate).ToListAsync();
			
		}

		public async Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includes)
			{
				IQueryable<T> query = _context.Set<T>().AsQueryable();
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
				return await query.ToListAsync();
			}
			public async Task<IEnumerable<T>> GetAllIncluding2(params Expression<Func<T, object>>[] includes)
			{
				IQueryable<T> query = _context.Set<T>().AsQueryable();
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
				return await query.AsNoTracking().ToListAsync();
			}
		}
	}
