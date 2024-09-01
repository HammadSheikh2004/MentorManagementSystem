using Mentor_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Mentor_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly MyDbContext _context;
        public StudentController(MyDbContext context) 
        { 
            _context = context;
        }

        CookieOptions cookieOptions = new CookieOptions
        {
            Path = "/Student/SigIn",
            Expires = DateTime.Now.AddDays(7),
            Secure = true
        };

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(StudentModel model)
        {
            var res = _context.Students.FirstOrDefault(x => x.stdEmail == model.stdEmail);
            Response.Cookies.Append("StdId", res.stdId);
            Response.Cookies.Append("StdEmail", res.stdEmail);
            if (res != null)
            {
                if (BCrypt.Net.BCrypt.Verify(model.stdPassword, res.stdPassword))
                {
                    return RedirectToAction("StudentDashboard", new { email = res.stdEmail });
                }
            }
            return View();
        }

        [HttpGet]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult StudentDashboard(string email)
        {
            Response.Headers.TryAdd("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.TryAdd("Pragma", "no-cache");
            Response.Headers.TryAdd("Expires", "0");

            if (Request.Cookies["StdId"] == null)
            {
                return RedirectToAction("SignIn");
            }

            var student = _context.Students.FirstOrDefault(x => x.stdEmail == email);

            if (student != null)
            {
                ViewBag.StdId = Request.Cookies["StdId"].ToString();
                return View((object)student.stdEmail);
            }

            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public IActionResult logout()
        {
            if (Request.Cookies["StdId"].ToString() != null)
            {
                Response.Cookies.Delete("StdId");
                return RedirectToAction("Index", "Portal");
            }
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult ShowAttendence()
        {
            string stdId = Request.Cookies["StdId"];
            var student = _context.Students.FirstOrDefault(s => s.stdId == stdId);
            if (student != null)
            {
                var attendanceBySubject = string.IsNullOrEmpty(student.Attendance)
                ? new Dictionary<string, string>()
                : student.Attendance.Split(new[] { ", " }, StringSplitOptions.None)
                    .Select(part => part.Split(':'))
                    .ToDictionary(split => split[0], split => split[1]);

                ViewBag.AttendanceBySubject = attendanceBySubject;
                ViewBag.PresentCount = attendanceBySubject.Count(a => a.Value == "Present");
                ViewBag.AbsentCount = attendanceBySubject.Count(a => a.Value == "Absent");
            }

            return View(student); 
        }



    }
}
