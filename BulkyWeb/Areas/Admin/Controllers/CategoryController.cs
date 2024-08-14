using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
// Toegang beperken
[Authorize(Roles = SD.Role_Admin)]
public class CategoryController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	// --------------------------------------------------
	public CategoryController(IUnitOfWork unitOfWork) {
		_unitOfWork = unitOfWork;
	}

	// --------------------------------------------------
	// GET
	public IActionResult Index() {
		var objCategoryList = _unitOfWork.CategoryRepo.GetAll().ToList();
		return View(objCategoryList);
	}

	// --------------------------------------------------
	// Create
	public IActionResult Create() {
		return View();
	}

	[HttpPost]
	public IActionResult Create(Category obj) {
		if (obj.Name == obj.DisplayOrder.ToString()) {
			ModelState.AddModelError("Name", "The DisplayOrder cannot be exactly match the Name");
		}

		if (obj.Name != null && obj.Name.ToLower() == "test") {
			ModelState.AddModelError("", "Test is an invalid value"); // Key is leeg > maar wordt getoond in de validation-summary
		}

		if (ModelState.IsValid) {
			_unitOfWork.CategoryRepo.Add(obj);
			_unitOfWork.Save();

			// TempData
			// - TempData behoudt zijn waarde enkel voor de volgende render.
			// - TempData-functionaliteit is enkel voor deze reden gecreëerd.
			TempData["succes"] = "Category created succesfully";

			return RedirectToAction(nameof(Index));
		}

		return View();
	}

	// --------------------------------------------------
	// Edit
	public IActionResult Edit(int? id) {
		if (id == null || id == 0) {
			return NotFound();
		}

		Category? categoryFromDb = _unitOfWork.CategoryRepo.Get(c => c.Id == id);
		// Category? categoryFromDb1 = _db.Categories.FirstOrDefault(c => c.Id == id);
		// Category? categoryFromDb2 = _db.Categories.Where(c => c.Id == id).FirstOrDefault();

		if (categoryFromDb == null) {
			return NotFound();
		}

		return View(categoryFromDb);
	}

	[HttpPost]
	public IActionResult Edit(Category obj) {
		if (ModelState.IsValid) {
			// obj wordt gevonden door de PK (id)
			_unitOfWork.CategoryRepo.Update(obj);
			_unitOfWork.Save();

			TempData["succes"] = "Category updated succesfully";

			return RedirectToAction(nameof(Index));
		}

		return View();
	}

	// --------------------------------------------------
	// DELETE
	public IActionResult Delete(int? id) {
		if (id == null || id == 0) {
			return NotFound();
		}

		Category? categoryFromDb = _unitOfWork.CategoryRepo.Get(c => c.Id == id);

		if (categoryFromDb == null) {
			return NotFound();
		}

		return View(categoryFromDb);
	}

	[HttpPost, ActionName("Delete")]
	public IActionResult DeletePOST(int? id) {
		Category? obj = _unitOfWork.CategoryRepo.Get(c => c.Id == id);

		if (obj == null) {
			return NotFound();
		}

		_unitOfWork.CategoryRepo.Remove(obj);
		_unitOfWork.Save();

		TempData["succes"] = "Category deleted succesfully";

		return RedirectToAction(nameof(Index));
	}
}