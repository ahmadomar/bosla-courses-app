using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoursesApp.Models
{
    public class CategoryModel
    {
        public int Id{ get; set; }

        [Required(ErrorMessage = "Category name is required!")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Category name should be between 10 and 200")]
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public string ParentName { get; set; }

        public SelectList MainCategories { get; set; }

    }
}