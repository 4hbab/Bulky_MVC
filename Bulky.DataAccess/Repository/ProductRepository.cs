using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
	private readonly ApplicationDbContext _db;

	// --------------------------------------------------
	// Constructor
	public ProductRepository(ApplicationDbContext db) : base(db) {
		_db = db;
	}

	// --------------------------------------------------
	// Methods
	public void Update(Product obj) {
		// _db.Products.Update(obj);

		// # Expliciet updaten
		// - Niet per se nodig
		// - Handig als je eigen logica wil implementeren
		// - Onderstaande is nodig wanneer je een prematuur record wil updaten
		// ... waar nog geen ImageUrl aan werd toegekend > heeft "" gekregen toen.
		// ... maar wordt door data-binding (input-tag) geconverteerd...
		// ... naar NULL wanneer een update van andere zaken plaats vindt
		// ... en geeft op die momenten een error.
		// - Hieronder doe je dan een "expliciete" update (dus gewoon een custom update)
		var objFromDb = _db.Products.FirstOrDefault(p => p.Id == obj.Id);
		if (objFromDb != null) {
			objFromDb.Title = obj.Title;
			objFromDb.ISBN = obj.ISBN;
			objFromDb.Price = obj.Price;
			objFromDb.ListPrice = obj.ListPrice;
			objFromDb.Price50 = obj.Price50;
			objFromDb.Price100 = obj.Price100;
			objFromDb.Description = obj.Description;
			objFromDb.CategoryId = obj.CategoryId;
			objFromDb.Author = obj.Author;

			if (obj.ImageUrl != null) {
				objFromDb.ImageUrl = obj.ImageUrl;
			}
		}
	}
}