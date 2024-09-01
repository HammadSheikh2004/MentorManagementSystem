using Mentor_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mentor_Management_System.Controllers
{
    public class FacultyController : Controller
    {
        private readonly MyDbContext _context;
        public FacultyController(MyDbContext context)
        {
            _context = context;
        }

        CookieOptions options = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1),
            Path = "/Faculty/SignIn",
            Secure = true
        };
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignIn(FacultyModel facultyModel)
        {
            var res = _context.Faculties.FirstOrDefault(x => x.FacultyEmail == facultyModel.FacultyEmail);
            Response.Cookies.Append("FacultyId", res.FacultyId.ToString());
            Response.Cookies.Append("FacultyEmail", res.FacultyEmail);
            if (res != null)
            {
                BCrypt.Net.BCrypt.Verify(facultyModel.Password, res.Password);
                return RedirectToAction("FacultyDashboard", new { email = res.FacultyEmail });
            }

            return View();
        }

        [HttpGet]
        public IActionResult FacultyDashboard(string email)
        {
            Response.Headers.TryAdd("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.TryAdd("Pragma", "no-cache");
            Response.Headers.TryAdd("Expires", "0");

            if (Request.Cookies["FacultyId"] == null)
            {
                return View("SignIn");
            }

            var facultyEmail = _context.Faculties.FirstOrDefault(x => x.FacultyEmail == email);
            if (facultyEmail != null)
            {
                ViewBag.FacultyId = Request.Cookies["FacultyId"];
                return View((object)facultyEmail.FacultyEmail);
            }
            return RedirectToAction("SignIn");
        }

        public IActionResult Logout()
        {
            if (Request.Cookies["FacultyId"] != null)
            {
                Response.Cookies.Delete("FacultyId");
                return RedirectToAction("Index", "Portal");
            }
            return RedirectToAction("SignIn");
        }

        public IActionResult Attendence()
        {
            var facultyIdCookie = Request.Cookies["FacultyId"];
            if (int.TryParse(facultyIdCookie, out int facultyId))
            {
                var course = _context.Faculties.Where(c => c.FacultyId == facultyId).Select(c => c.FacultyCourse).FirstOrDefault();

                if (course != null && course.Any())
                {
                    if (!string.IsNullOrEmpty(course))
                    {
                        var courses = course.Split(',').Select(s => s.Trim()).ToList();

                        ViewBag.courses = courses.Select(c => new SelectListItem { Value = c, Text = c }).ToList();
                    }
                }
                else
                {
                    ViewBag.Courses = new List<SelectListItem>();
                }
                return View();
            }
            return RedirectToAction("SignIn", "Faculty");
        }

        [HttpPost]
        public IActionResult fetchStudent(DateTime date, string course)
        {
            if (string.IsNullOrWhiteSpace(course))
            {
                return View("Attendence", new List<StudentModel>());
            }

            var selectedCourses = course.Split(',').Select(c => c.Trim()).ToList();
            var studentsForDate = _context.Students
                                          .Where(student => student.ClassTime == date)
                                          .ToList();
            var filteredStudents = studentsForDate
                                   .Where(student => selectedCourses.Any(sc => student.stdCourse.Contains(sc)))
                                   .ToList();
            ViewBag.SelectedCourse = course;

            return View("Attendence", filteredStudents);
        }

        [HttpPost]
        public IActionResult MarkAttendance(Dictionary<int, int> studentIds, Dictionary<int, string> attendanceStatuses, string course)
        {
            foreach (var kvp in studentIds)
            {
                int index = kvp.Key;
                int studentId = kvp.Value;
                string attendanceStatus = attendanceStatuses[index];

                var student = _context.Students.Find(studentId);

                if (student != null)
                {
                    var attendanceBySubject = string.IsNullOrEmpty(student.Attendance) ? new Dictionary<string, string>() :
                        student.Attendance.Split(new[] { ", " }, StringSplitOptions.None).Select(part => part.Split(':')).ToDictionary(split => split[0], split => split[1]);

                    attendanceBySubject[course] = attendanceStatus;

                    student.Attendance = string.Join(", ", attendanceBySubject.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Attendence");
        }




    }
}
