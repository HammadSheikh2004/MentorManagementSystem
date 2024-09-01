using BCrypt.Net;
using Mentor_Management_System.Areas.Identity.Data;
using Mentor_Management_System.Areas.Identity.Pages.Account;
using Mentor_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mentor_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<Mentor_Management_SystemUser> _signInManager;
        private readonly UserManager<Mentor_Management_SystemUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyDbContext _myDb;

        public AdminController(SignInManager<Mentor_Management_SystemUser> signInManager, UserManager<Mentor_Management_SystemUser> userManager, MyDbContext myDb, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _myDb = myDb;
            _roleManager = roleManager;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var model = new MentorViewModel();
            if (user != null)
            {
                model.User = new UserModel
                {
                    User_First_Name = user.FirstName,
                };
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetUserInfo()
        {
            var userList = (
                   from user in _myDb.Users
                   join userInfo in _myDb.UserInfo on user.User_Id equals userInfo.User_Id into UI
                   from allUser in UI.DefaultIfEmpty()
                   select new MentorViewModel
                   {
                       User = user,
                       UserInfo = allUser
                   }).OrderByDescending(Id => Id.User.User_Id).ToList();
            return View(userList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateField([FromBody] UpdateFieldModel model)
        {
            var user = _myDb.Users.Find(model.Id);
            if (user != null)
            {
                switch (model.Field)
                {
                    case "APF_Challan":
                        user.APF_Challan = model.Value;
                        break;

                    case "TF_Challan":
                        user.TF_Challan = model.Value;
                        break;

                    case "Admit_Card":
                        user.Admit_Card = model.Value;
                        break;

                    case "Wellcome_Letter":
                        user.WellCome_Letter = model.Value;
                        break;
                }
                _myDb.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Mentor");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult StudentInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchCourseField(string course)
        {
            var field = _myDb.UserInfo.Count(c => c.Course == course);
            var model = new MentorViewModel
            {
                FieldCount = field,
                CourseName = course
            };
            return View("StudentInfo", model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult StudentInfo(string course, string password, int count, string initialStudentId, string FacultyName, int FacultyId, DateTime classTime)
        {
            if (string.IsNullOrWhiteSpace(initialStudentId) || count <= 0 || string.IsNullOrWhiteSpace(course) || string.IsNullOrWhiteSpace(password))
            {
                return View();
            }

            if (initialStudentId.Length < 4 || !int.TryParse(initialStudentId.Substring(initialStudentId.Length - 4), out int startingId))
            {
                return View();
            }

            string baseId = initialStudentId.Substring(0, initialStudentId.Length - 4);

            var courses = course.Split(',').Select(c => c.Trim()).ToList();

            // Join the courses into a single string with comma separation
            string combinedCourses = string.Join(", ", courses);

            for (int i = 0; i < count; i++)
            {


                string studentId = $"{baseId}{(startingId + i):D4}";
                string studentEmail = $"{studentId}@gmail.com";

                string hashPass = BCrypt.Net.BCrypt.HashPassword(studentId);

                var student = new StudentModel
                {
                    stdId = studentId,
                    stdEmail = studentEmail,
                    stdCourse = combinedCourses,
                    stdPassword = hashPass,
                    FacultyName = FacultyName,
                    FacultyId = FacultyId,
                    ClassTime = classTime
                };

                _myDb.Students.Add(student);

            }
            _myDb.SaveChanges();
            return View();
        }




        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult FacultyCreate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult FacultyCreate(FacultyModel facultyModel)
        {
            string password = facultyModel.Password;
            string hashPass = BCrypt.Net.BCrypt.HashPassword(password);
            facultyModel.Password = hashPass;
            _myDb.Faculties.Add(facultyModel);
            _myDb.SaveChanges();
            return View(facultyModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DisplayStudent()
        {
            var data = _myDb.Students.OrderByDescending(x => x.Id).ToList();
            return View(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DisplayFaculty()
        {
            var data = _myDb.Faculties.OrderByDescending(x => x.FacultyId).ToList();
            return View(data);
        }
    }
}
