using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CoursesApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryService categoryService;

        private readonly IMapper mapper;
        public CategoryController()
        {
            categoryService = new CategoryService();
            mapper = AutoMapperConfig.Mapper;
        }

        // GET: Admin/Category
        public ActionResult Index()
        {
            var categories = categoryService.ReadAll();

            var categoriesList = mapper.Map<List<CategoryModel>>(categories);

            return View(categoriesList);
        }


        public ActionResult Create()
        {
            var categoryModel = new CategoryModel();

            InitMainCategories(null, ref categoryModel);

            return View(categoryModel);
        }

        [HttpPost]
        public ActionResult Create(CategoryModel data)
        {
            var newCategory = mapper.Map<Category>(data);
            newCategory.Category1 = null;
            int creationResult = categoryService.Create(newCategory);

            if (creationResult == -2)
            {
                InitMainCategories(null, ref data);

                ViewBag.Message = "Category Name is exists!";
                return View(data);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return RedirectToAction("index", "Home");
            }

            var currentCategory = categoryService.ReadById(id.Value);

            if(currentCategory == null)
            {
                return HttpNotFound($"This category ({id}) not found!");
            }

            var categoryModel = mapper.Map<CategoryModel>(currentCategory);

            InitMainCategories(currentCategory.Id, ref categoryModel);

            return View(categoryModel);
        }

        [HttpPost]
        public ActionResult Edit(CategoryModel data)
        {
            var updatedCategory = mapper.Map<Category>(data);

            var result = categoryService.Update(updatedCategory);

            if (result == -2)
            {
                ViewBag.Message = "Category Name is exists!";
                InitMainCategories(data.Id, ref data);
                return View(data);
            }
            else if (result > 0)
            {
                ViewBag.Success = true;
                ViewBag.Message = $"Category ({data.Id}) updated successfully.";
            }
            else
                ViewBag.Message = $"An error occurred!";

            InitMainCategories(data.Id, ref data);
            return View(data);
        }


        public ActionResult Delete(int? Id)
        {
            if(Id != null)
            {
                var category = categoryService.ReadById(Id.Value);

                var categoryInfo = mapper.Map<CategoryModel>(category);

                return View(categoryInfo);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int? Id)
        {
            if(Id!= null)
            {
                var deleted = categoryService.Delete(Id.Value);
                if (deleted)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Delete", new { Id = Id });
            }
            return HttpNotFound();
        }

        private void InitMainCategories(int? categoryToExclude, ref CategoryModel categoryModel)
        {
            var categoriesList = categoryService.ReadAll();

            if (categoryToExclude!=null)
            {
                var currentCategory = categoriesList.Where(c => c.Id == categoryToExclude).FirstOrDefault();
                categoriesList.Remove(currentCategory);
            }

            categoryModel.MainCategories = new SelectList(categoriesList, "ID", "Name");
        }
    }
}