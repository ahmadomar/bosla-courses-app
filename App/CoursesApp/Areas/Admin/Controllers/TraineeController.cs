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
    [Authorize(Roles = "Admin")]
    public class TraineeController : Controller
    {
        private readonly TraineeCourseService traineeCourseService;
        private readonly IMapper mapper;
        public TraineeController()
        {
            traineeCourseService = new TraineeCourseService();
            mapper = AutoMapperConfig.Mapper;
        }

        // GET: Admin/Trainee
        public ActionResult Index(int? cId)
        {
            if (cId == null)
                return RedirectToAction("Index", "Default", new {area = "admin" });

            var trainees = traineeCourseService.GetTrainees(cId.Value);

            var courseTrainees = mapper.Map<IEnumerable<Trainee_Courses>, IEnumerable<TraineeCourseModel>>(trainees);
            return View(courseTrainees);
        }
    }
}