using Microsoft.AspNetCore.Mvc;

namespace Mentor_Management_System.Controllers
{
    public class PortalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
