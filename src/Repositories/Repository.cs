using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Repositories
{
	public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : BaseEntity
	{
		public DbContext Context { get; }
		public Repository(DbContext context)
		{
			Context = context;
		}

		public IQueryable<TEntity> GetAll()
		{
			return Context.Set<TEntity>();
		}

		public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
		{
			return Context.Set<TEntity>().Where(predicate);
		}

		public void Insert(TEntity entity)
		{
			Context.Set<TEntity>().Add(entity);
			Context.SaveChanges();
		}

		public void Commit()
		{
			Context.SaveChanges();
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}