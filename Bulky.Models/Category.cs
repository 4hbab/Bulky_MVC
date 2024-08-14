using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models;

public class Category
{
	[Key] // EF
	public int Id { get; set; }

	[Required] // EF
	[DisplayName("Category Name")]
	[MaxLength(30)] // server side validation
	public string Name { get; set; } = null!;

	[DisplayName("Display Order")]
	[Range(1, 100, ErrorMessage = "The field Display Order must be between 1 - 100")] // server side validation
	public int DisplayOrder { get; set; }

	//public List<Product> Products { get; set; } = new List<Product>();
}