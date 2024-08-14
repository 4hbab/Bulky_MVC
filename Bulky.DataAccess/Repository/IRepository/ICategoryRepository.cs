using Bulky.Models;

namespace Bulky.DataAccess.Repository.IRepository;

// Erft de Algemene RepoInterface met specifiek type "Category" geïmplementeerd
public interface ICategoryRepository : IRepository<Category>
{
	void Update(Category obj);

	// # Save() is hier niet meer nodig > Wordt geimplemteerd in UnitOfWork
	// void Save();
}