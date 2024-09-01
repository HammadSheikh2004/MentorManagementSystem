using Microsoft.AspNetCore.Mvc;

namespace Mentor_Management_System.Controllers
{
    public class MentorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
       
    }
}
