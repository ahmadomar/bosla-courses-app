using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly IMapper mapper;
        private readonly CourseService courseService;
        private readonly CategoryService categoryService;
        private readonly TrainerService trainerService;
        public CourseController()
        {
            mapper = AutoMapperConfig.Mapper;
            courseService = new CourseService();
            categoryService = new CategoryService();
            trainerService = new TrainerService();
        }
        // GET: Admin/Course
        public ActionResult Index(string query = null, int? categoryId = null, int? trainerId = null)
        {
            var coursesListData = new CoursesListModel();
            InitSelectList(ref coursesListData);

            var coursesList = courseService.ReadAll(query, trainerId, categoryId);

            var mappedCoursesList = mapper.Map<List<CourseModel>>(coursesList);
            coursesListData.Courses = mappedCoursesList;


            return View(coursesListData);
        }

        public ActionResult Create()
        {
            var courseModel = new CourseModel();

            InitSelectList(ref courseModel);
            return View(courseModel);
        }

        [HttpPost]
        public ActionResult Create(CourseModel courseData)
        {
            InitSelectList(ref courseData);

            try
            {
                if (ModelState.IsValid)
                {
                    courseData.Image_ID = SaveImageFile(courseData.ImageFile);

                    var courseDTO = mapper.Map<Cours>(courseData);
                    courseDTO.Category = null;
                    courseDTO.Trainer = null;

                    int result = courseService.Create(courseDTO);

                    if(result >= 1)
                    {
                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "An error occurred!";
                }
                return View(courseData);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(courseData);
            }
        }


        public ActionResult Edit(int? Id)
        {
            if (Id == null)
                return HttpNotFound();

            var currentCourseData = courseService.Get(Id.Value);
            var courseModel = mapper.Map<CourseModel>(currentCourseData);

            InitSelectList(ref courseModel);
            return View(courseModel);
        }

        [HttpPost]
        public ActionResult Edit(CourseModel courseData)
        {
            InitSelectList(ref courseData);

            try
            {
                if (ModelState.IsValid)
                {
                    courseData.Image_ID = SaveImageFile(courseData.ImageFile, courseData.Image_ID);

                    var courseDTO = mapper.Map<Cours>(courseData);
                    courseDTO.Category = null;
                    courseDTO.Trainer = null;

                    int result = courseService.Update(courseDTO);

                    if (result >= 1)
                    {
                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "An error occurred!";
                }
                return View(courseData);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(courseData);
            }
        }

        private void InitSelectList(ref CourseModel courseModel)
        {
            var mappedCategoriesList = GetCategories();
            courseModel.Categories = new SelectList(mappedCategoriesList, "Id", "Name");

            var mappedTrainersList = GetTrainers();
            courseModel.Trainers = new SelectList(mappedTrainersList, "Id", "Name");
        }

        private void InitSelectList(ref CoursesListModel coursesList)
        {
            var mappedCategoriesList = GetCategories();
            coursesList.Categories = new SelectList(mappedCategoriesList, "Id", "Name");

            var mappedTrainersList = GetTrainers();
            coursesList.Trainers = new SelectList(mappedTrainersList, "Id", "Name");
        }

        private IEnumerable<CategoryModel> GetCategories()
        {
            var categories = categoryService.ReadAll();
            return mapper.Map<IEnumerable<CategoryModel>>(categories);
        }

        private IEnumerable<TrainerModel> GetTrainers()
        {
            var trainers = trainerService.ReadAll();
            return mapper.Map<IEnumerable<TrainerModel>>(trainers);
        }

        private string SaveImageFile(HttpPostedFileBase imageFile,string currentImageId = "")
        {
            if (imageFile != null)
            {
                var fileExtenstion = Path.GetExtension(imageFile.FileName);
                var imageGuid = Guid.NewGuid().ToString();

                string imageId = imageGuid + fileExtenstion;

                // Save new file
                string filePath = Server.MapPath($"~/Uploads/Courses/{imageId}");
                imageFile.SaveAs(filePath);

                // Delete old file - update action
                if (!string.IsNullOrEmpty(currentImageId))
                {
                    string oldFilePath = Server.MapPath($"~/Uploads/Courses/{currentImageId}");
                    System.IO.File.Delete(oldFilePath);
                }

                return imageId;
            }
            return currentImageId;
        }
    }
}