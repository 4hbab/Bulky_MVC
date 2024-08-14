using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using BulkyWeb.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;

namespace BulkyWeb.Areas.Admin.Controllers_OUD;

/*
	# WebHostEnvironment
	- Object om rootfolder te benaderen voor images (t:6u57m45s)
*/

[Area("AdminOUD")]
public class ProductController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	// # WebHostEnvironment
	private readonly IWebHostEnvironment _webHostEnvironment;

	// --------------------------------------------------
	public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) {
		_unitOfWork = unitOfWork;
		_webHostEnvironment = webHostEnvironment;
	}

	// --------------------------------------------------
	// GET
	public IActionResult Index() {
		IList<Product> productLIst = _unitOfWork.ProductRepo.GetAll(includeProperties: "Category").ToList();

		return View(productLIst);
	}

	// --------------------------------------------------
	// UPSERT - CREATE - EDIT
	// public IActionResult Create()
	public IActionResult Upsert(int? id) {
		IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepo.GetAll()
			.Select(c => new SelectListItem {
				Text = c.Name,
				Value = c.Id.ToString()
			});

		// # Verschillende manieren om complexe object-data door te geven naar view
		// - Uiteindelijk wordt gebruikt gemaakt van een ViewModel (ProductVM)
		// ViewBag.CategoryList = CategoryList;
		// ViewData["CategoryList"] = CategoryList;

		var productVM = new ProductVM() {
			Product = new Product(),
			CategoryList = CategoryList
		};

		// CREATE (Insert)
		if (id == null || id == 0) {
			return View(productVM);
		}
		// EDIT (Update)
		else {
			productVM.Product = _unitOfWork.ProductRepo.Get(p => p.Id == id)!;
			return View(productVM);
		}
	}

	[HttpPost]
	// public IActionResult Create(ProductVM productVM)
	public IActionResult Upsert(ProductVM productVM, IFormFile? file) {
		if (ModelState.IsValid) {

			// # WebHostEnvironment
			string wwwRootPath = _webHostEnvironment.WebRootPath;
			if (file != null) {
				// random file-name
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
				string productPath = Path.Combine(wwwRootPath, @"img/product");

				string imgUrl = productVM.Product.ImageUrl;
				if (!string.IsNullOrEmpty(imgUrl)) {
					// Delete old image
					string oldImagePath = Path.Combine(wwwRootPath, imgUrl.TrimStart('/'));

					if (System.IO.File.Exists(oldImagePath)) {
						System.IO.File.Delete(oldImagePath);
					}
				}

				// Bestand opslaan
				using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) {
					file.CopyTo(fileStream);
				}

				productVM.Product.ImageUrl = @"/img/product/" + fileName;
			}

			if (productVM.Product.Id == 0) {
				_unitOfWork.ProductRepo.Add(productVM.Product);
			}
			else {
				_unitOfWork.ProductRepo.Update(productVM.Product);
			}

			_unitOfWork.Save();

			// TempData
			// - TempData behoudt zijn waarde enkel voor de volgende render.
			// - TempData-functionaliteit is enkel voor deze reden gecreëerd.
			// - Indien TempData != null > wordt er een Notification getoond
			// - Zie _Layout.cshtml voor meer info
			TempData["succes"] = "Product created succesfully";

			return RedirectToAction(nameof(Index));
		}
		else {
			IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepo.GetAll()
				.Select(c => new SelectListItem {
					Text = c.Name,
					Value = c.Id.ToString()
				});

			productVM.CategoryList = CategoryList;
		}

		return View(productVM);
	}

	// --------------------------------------------------
	// EDIT (niet meer nodig > Upsert)
	#region 
	// public IActionResult Edit(int? id)
	// {
	// 	if (id == null || id == 0) {
	// 		return NotFound();
	// 	}

	// 	Product? product = _unitOfWork.ProductRepo.Get(p => p.Id == id);

	// 	if (product == null) {
	// 		return NotFound();
	// 	}

	// 	return View(product);
	// }

	// [HttpPost]
	// public IActionResult Edit(Product obj)
	// {
	// 	if (ModelState.IsValid) {
	// 		// obj wordt gevonden door de PK (id)
	// 		_unitOfWork.ProductRepo.Update(obj);
	// 		_unitOfWork.Save();

	// 		TempData["succes"] = "Category updated succesfully";

	// 		return RedirectToAction(nameof(Index));
	// 	}

	// 	return View();
	// }
	#endregion

	// --------------------------------------------------
	// DELETE
	public IActionResult Delete(int? id) {
		if (id == null || id == 0) {
			return NotFound();
		}

		Product? product = _unitOfWork.ProductRepo.Get(p => p.Id == id);

		if (product == null) {
			return NotFound();
		}

		return View(product);
	}

	[HttpPost, ActionName(nameof(Delete))]
	public IActionResult DeletePOST(int? id) {
		Product? obj = _unitOfWork.ProductRepo.Get(c => c.Id == id);

		if (obj == null) {
			return NotFound();
		}

		_unitOfWork.ProductRepo.Remove(obj);
		_unitOfWork.Save();

		TempData["succes"] = "Category deleted succesfully";

		return RedirectToAction(nameof(Index));
	}

	// --------------------------------------------------
	// API CALLS
	#region
	[HttpGet]
	public IActionResult GetAll() {
		IList<Product> productLIst = _unitOfWork.ProductRepo.GetAll(includeProperties: "Category").ToList();
		return Json(new { data = productLIst });
	}
	#endregion
}