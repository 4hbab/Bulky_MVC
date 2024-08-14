namespace Bulky.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
	ICategoryRepository CategoryRepo { get; }
	IProductRepository ProductRepo { get; }
	void Save();
}