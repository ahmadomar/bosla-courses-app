using AutoMapper;
using CoursesApp.Data;
using CoursesApp.Models;
using CoursesApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IMapper mapper;
        private readonly CourseService courseService;
        public DefaultController()
        {
            mapper = AutoMapperConfig.Mapper;

            courseService = new CourseService();
        }
        // GET: Course
        public ActionResult Index()
        {
            var courses = courseService.ReadAll();
            return View(mapper.Map<List<Cours>, List<CourseModel>>(courses));
        }
    }
}