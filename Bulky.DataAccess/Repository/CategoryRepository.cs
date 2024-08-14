using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
	private readonly ApplicationDbContext _db;

	// -----
	public CategoryRepository(ApplicationDbContext db) : base(db) {
		_db = db;
	}

	// -----
	// # Save() is hier niet meer nodig > Wordt geimplemteerd in UnitOfWork
	// public void Save()
	// {
	// 	_db.SaveChanges();
	// }

	public void Update(Category obj) {
		_db.Categories.Update(obj);
	}
}