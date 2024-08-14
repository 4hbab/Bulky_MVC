using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository;

// # Unit of unitOfWork (Bernd - Eigen interpretatie)
// Het bundelen van de verschillende services (repo's) van 1 database
// ... in opniew een centrale eenheid
// Je creëert hier eigenlijk opniew een centrale DataBaseUnit
// ... met extra functionaliteiten geïmplemnteerd (via de verschillende services)
// De Servies (repo's) worden hier manueel geïnjecteerd met DI

public class UnitOfWork : IUnitOfWork
{
	// Variables
	private readonly ApplicationDbContext _db;

	// --------------------------------------------------
	// Properties
	public ICategoryRepository CategoryRepo { get; private set; }
	public IProductRepository ProductRepo { get; private set; }

	// --------------------------------------------------
	// Constructor
	public UnitOfWork(ApplicationDbContext db) {
		_db = db;

		// CategoryRepository manueel injecteren met DI
		CategoryRepo = new CategoryRepository(_db);
		ProductRepo = new ProductRepository(_db);
	}

	// --------------------------------------------------
	// Methodes

	// # Dit is nu een globale Save voor de gehele database
	// > Zodat je niet op elke dbRepo een saveChanges kan uitvoeren
	// > Je voert het gewoon uit op de UnitOfWork
	// ... wetende dat de gehele database wordt opgeslagen
	// ... en dus zo worden de wijzigingen van alle repo's opgeslagen met 1 centraal commando
	public void Save() {
		_db.SaveChanges();
	}
}