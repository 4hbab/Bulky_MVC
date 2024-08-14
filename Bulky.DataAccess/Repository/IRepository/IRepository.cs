using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
	// # T - Category
	// # includeProperties (case-sentitive): Category,CoverType,...
	IEnumerable<T> GetAll(string? includeProperties = null);
	T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null);
	void Add(T entity);

	// # Update in algemene Repository kan voor problemen zorgen
	// - daarom Update buiten de repo houden
	// void Update(T entity);

	void Remove(T entity);
	void RemoveRange(IEnumerable<T> Entities);
}