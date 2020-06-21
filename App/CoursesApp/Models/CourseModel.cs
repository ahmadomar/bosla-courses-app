using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Models
{
    public class CourseModel
    {
        public int Course_Id { get; set; }

        [Required]
        public string Name { get; set; }
        public System.DateTime Creation_Date { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }

        [Required]
        [Display(Name = "Trainer")]
        public Nullable<int> Trainer_Id { get; set; }
        public string TrainerName { get; set; }

        private string _imageId;
        public string Image_ID
        {
            set
            {
                _imageId = string.IsNullOrWhiteSpace(value) ? "empty.jpg" : value;
            }
            get
            {
                return _imageId;
            }
        }

        public HttpPostedFileBase ImageFile { get; set; }


        public SelectList Trainers { get; set; }
        public SelectList Categories { get; set; }

    }


    public class CoursesListModel
    {
        public IEnumerable<CourseModel> Courses { get; set; }
        public string Query { get; set; }

        [Display(Name = "Trainer")]
        public int TrainerId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public SelectList Trainers { get; set; }
        public SelectList Categories { get; set; }
    }
}