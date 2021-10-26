using System;
using System.Linq.Expressions;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Repositories
{
	public interface IRepository<TEntity> where TEntity : BaseEntity
	{
		IQueryable<TEntity> GetAll();
		IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
		void Insert(TEntity entity);
		void Commit();
	}
}