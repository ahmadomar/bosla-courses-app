using System.ComponentModel.DataAnnotations;

namespace CoursesApp.Areas.Admin.Models
{
    public class LoginModel
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public string Message { get; set; }

    }
}