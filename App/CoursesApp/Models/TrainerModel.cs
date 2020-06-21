using System.ComponentModel.DataAnnotations;

namespace CoursesApp.Models
{
    public class TrainerModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please, Write Email in correct format!")]
        public string Email { get; set; }

        [StringLength(250, MinimumLength = 10)]
        public string Description { get; set; }

        [Url(ErrorMessage = "Please, enter correct url!")]
        public string Website { get; set; }
        public System.DateTime Creation_Date { get; set; }
    }
}