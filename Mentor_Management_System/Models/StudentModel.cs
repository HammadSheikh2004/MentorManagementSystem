using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mentor_Management_System.Models
{
    public class StudentModel
    {
        [Key]
        public int Id { get; set; }
        public string stdId { get; set; }
        public string stdEmail { get; set; }
        public string stdPassword { get; set; }
        public string stdCourse { get; set; }
        public string FacultyName { get; set; }
        [DataType(DataType.Date)]
        public DateTime ClassTime { get; set; }

        public string Attendance { get; set; } = string.Empty;
        public int FacultyId { get; set; }
        [ForeignKey(nameof(FacultyId))]
        public virtual FacultyModel Faculty { get; set; }

        [NotMapped] 
        public Dictionary<string, string> AttendanceBySubject { get; set; } = new Dictionary<string, string>();
    }
}
