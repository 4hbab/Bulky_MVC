using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net.Quic;

namespace Bulky.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
	private readonly ApplicationDbContext _db;
	private readonly DbSet<T> dbSet;

	// --------------------------------------------------
	public Repository(ApplicationDbContext db) {
		_db = db;

		// # _db.Categories == dbSet
		dbSet = _db.Set<T>();
	}

	// --------------------------------------------------
	public void Add(T entity) {
		dbSet.Add(entity);
	}

	public T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null) {
		IQueryable<T> query = dbSet;

		if (!string.IsNullOrEmpty(includeProperties)) {
			foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
				query = query.Include(includeProp);
			}
		}

		query = query.Where(filter);
		return query.FirstOrDefault();
	}

	public IEnumerable<T> GetAll(string? includeProperties = null) {
		IQueryable<T> query = dbSet;

		if (!string.IsNullOrEmpty(includeProperties)) {
			foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
				query = query.Include(includeProp);
			}
		}

		return query.ToList();
	}

	public void Remove(T entity) {
		dbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entities) {
		dbSet.RemoveRange(entities);
	}
}