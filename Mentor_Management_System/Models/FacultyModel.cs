using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor_Management_System.Models
{
    public class FacultyModel
    {
        [Key]
        public int FacultyId { get; set; }
        [Required (ErrorMessage = "Faculty Name is Requied!")]
        public string FacultyName { get; set; }
        [Required (ErrorMessage = "Faculty Email is Requird!")]
        [EmailAddress]
        public string FacultyEmail { get;  set; }
        [Required(ErrorMessage = "Faculty Qualification is Requied!")]
        public string FacultyQualification { get; set; }
        [Required(ErrorMessage = "Faculty Course is Requied!")]
        public string FacultyCourse { get; set; }
        [Required(ErrorMessage = "Password Field is Requied!")]
        public string Password { get; set; }
        public ICollection<StudentModel> Students { get; set; } 

    }
}
