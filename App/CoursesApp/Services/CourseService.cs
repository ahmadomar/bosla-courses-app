using CoursesApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoursesApp.Services
{
    public interface ICourseService
    {
        int Create(Cours course);
        int Update(Cours updatedCourse);
        List<Cours> ReadAll(string query = null, int? trainerId = null, int? categoryId = null);
        Cours Get(int Id);
    }
    public class CourseService : ICourseService
    {
        private readonly Courses_DBEntity db;
        public CourseService()
        {
            db = new Courses_DBEntity();
        }
        public int Create(Cours course)
        {
            course.Creation_Date = DateTime.Now;

            db.Courses.Add(course);
            return db.SaveChanges();
        }

        public Cours Get(int Id)
        {
            return db.Courses.Find(Id);
        }

        public List<Cours> ReadAll(string query = null, int? trainerId = null, int? categoryId = null)
        {
            return db.Courses.Where(c => 
                                        (trainerId == null || c.Trainer_Id == trainerId)
                                      && (categoryId == null || c.Category_Id == categoryId)
                                      && (query == null || c.Name.Contains(query))).ToList();
        }

        public int Update(Cours updatedCourse)
        {
            db.Courses.Attach(updatedCourse);
            db.Entry(updatedCourse).State = System.Data.Entity.EntityState.Modified;
            return db.SaveChanges();
        }
    }
}