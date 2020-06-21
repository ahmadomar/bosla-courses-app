using CoursesApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoursesApp.Services
{
    public interface ITraineeCourseService
    {
        IEnumerable<Trainee_Courses> GetTrainees(int? courseId = null);
    }
    public class TraineeCourseService : ITraineeCourseService
    {
        private readonly Courses_DBEntity courses_DBEntity;
        public TraineeCourseService()
        {
            courses_DBEntity = new Courses_DBEntity();
        }

        public IEnumerable<Trainee_Courses> GetTrainees(int? courseId = null)
        {
            return courses_DBEntity.Trainee_Courses.Where(t => 
                            courseId == null || t.Course_Id == courseId);                            
        }
    }
}