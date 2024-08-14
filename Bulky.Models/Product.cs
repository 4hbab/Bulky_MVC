using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models;

public class Product
{
	[Key] // EF
	public int Id { get; set; }

	[Required] // EF
	public string Title { get; set; } = null!;

	public string Description { get; set; } = null!;

	[Required] // EF
	public string ISBN { get; set; } = null!;

	[Required] // EF
	public string Author { get; set; } = null!;

	[Display(Name = "List Price")]
	[Range(1, 1000)]
	public double ListPrice { get; set; }

	[Display(Name = "Price for 1-50")]
	[Range(1, 1000)]
	public double Price { get; set; }

	[Display(Name = "Price for 50+")]
	[Range(1, 1000)]
	public double Price50 { get; set; }

	[Display(Name = "Price for 100+")]
	[Range(1, 1000)]
	public double Price100 { get; set; }

	[ForeignKey(nameof(Category))]
	public int CategoryId { get; set; }

	[ValidateNever]
	public Category Category { get; set; } = null!;

	[ValidateNever]
	public string ImageUrl { get; set; } = "N/A";
}