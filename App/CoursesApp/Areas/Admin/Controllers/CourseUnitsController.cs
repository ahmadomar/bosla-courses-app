using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Areas.Admin.Controllers
{
    public class CourseUnitsController : Controller
    {
        private readonly IMapper mapper;
        private readonly CourseUnitService courseUnitService;

        public CourseUnitsController()
        {
            mapper = AutoMapperConfig.Mapper;
            courseUnitService = new CourseUnitService();
        }


        // GET: Admin/CourseUnits?courseId=1
        public ActionResult Index(int? courseId)
        {
            if (courseId == null)
                return HttpNotFound();

            var units = courseUnitService.ReadCourseUnits(courseId.Value);
            var mappedUnits = mapper.Map<IEnumerable<Course_Units>, IEnumerable<CourseUnitModel>>(units);

            ViewBag.CourseName = mappedUnits.FirstOrDefault()?.CourseName;
            ViewBag.CourseId = courseId;

            return View(mappedUnits);
        }


        // POST: Admin/CourseUnits/Create
        [HttpPost]
        public JsonResult Create(CourseUnitModel unitData)
        {
            try
            {
                var unit = mapper.Map<Course_Units>(unitData);

                var savingStatus = courseUnitService.Create(unit);

                if (savingStatus == Common.SavingStatus.Saved)
                {
                    return Json(new { saved = true });
                }
                else if(savingStatus == Common.SavingStatus.Exists)
                {
                    return Json(new { saved = false, message = "exists" });
                }
                else
                {
                    return Json(new { saved = false });
                }
            }
            catch(Exception ex)
            {
                return Json(new { saved = false, message = ex.Message });
            }
        }

        // GET: Admin/CourseUnits/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/CourseUnits/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/CourseUnits/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/CourseUnits/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
