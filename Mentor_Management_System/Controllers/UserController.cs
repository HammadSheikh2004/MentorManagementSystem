using Mentor_Management_System.Areas.Identity.Data;
using Mentor_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace Mentor_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<Mentor_Management_SystemUser> _userManager;
        private readonly SignInManager<Mentor_Management_SystemUser> _signInManager;
        private readonly MyDbContext _myDb;
        private readonly IWebHostEnvironment _web;
        

        public UserController(UserManager<Mentor_Management_SystemUser> userManager, SignInManager<Mentor_Management_SystemUser> signInManager, IWebHostEnvironment web, MyDbContext myDb)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _web = web;
            _myDb = myDb;
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserDashboard()
        {
            var userId = _userManager.GetUserId(User);
            var userDetail = await _userManager.FindByIdAsync(userId);
            var model = new MentorViewModel();

            if (userDetail != null)
            {               
                    var challan = await _myDb.Users
                                                .Where(u => u.User_Email == userDetail.Email)
                                                .Select(u => new { u.APF_Challan, u.TF_Challan })
                                                .FirstOrDefaultAsync();
                var apfChallan = challan?.APF_Challan;
                var tfChallan = challan?.TF_Challan;

                    model.User = new UserModel
                    {
                        User_First_Name = userDetail.FirstName,
                        APF_Challan = apfChallan,
                        TF_Challan = tfChallan,

                    };   
            }
            return View(model);
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> AdmissionProcess()
        {
            var userId = _userManager.GetUserId(User);
            var userDetail = await _userManager.FindByIdAsync(userId);
            var model = new MentorViewModel();

            if (userDetail != null)
            {
                model.User = new UserModel
                {
                    User_First_Name = userDetail.FirstName,
                    User_Last_Name = userDetail.LastName,
                    User_Email = userDetail.Email,
                    User_Phone = userDetail.phone
                };
            }
            return View(model);
        }

        [Authorize(Roles = "User")]
        [Route("User/AdmissionProcess")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdmissionProcess(MentorViewModel mentorView, IFormFile User_Image)
        {
           
            if (User_Image == null || User_Image.Length == 0)
            {
                ModelState.AddModelError("User_Image", "Please upload an image.");
                return View(mentorView);
            }

            if (mentorView.UserInfo.School_Marks < 50)
            {
                ModelState.AddModelError("UserInfo.School_Marks", "Matric Percenatge should be greater than 50%");
                return View(mentorView);
            }else if (mentorView.UserInfo.College_Marks < 50)
            {
                ModelState.AddModelError("UserInfo.College_Marks", "Inter Percenatge should be greater than 50%");
                return View(mentorView);
            }
            else
            {
                try
                {
                    var path = Path.Combine(_web.WebRootPath, "Users/UserImages");
                    var fileName = Path.GetFileName(User_Image.FileName);
                    var extensionName = Path.GetExtension(fileName);
                    var ds = DateTime.Now.Millisecond;
                    string imageName = "user_img_" + ds + extensionName;
                    var fileSavePath = Path.Combine(path, imageName);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
                    {
                        User_Image.CopyTo(stream);
                    }

                    mentorView.UserInfo.User_Image = imageName;

                    _myDb.Users.Add(mentorView.User);
                    var userSaveResult = _myDb.SaveChanges();

                    mentorView.UserInfo.User_Id = mentorView.User.User_Id;
                    _myDb.UserInfo.Add(mentorView.UserInfo);
                    var userInfoSaveResult = _myDb.SaveChanges();

                    if (userSaveResult > 0 && userInfoSaveResult > 0)
                    {
                        ViewBag.Message = "Data Inserted Successfully";
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Data Not Inserted";
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return View(mentorView);
                }
            }   
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
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> APFChallanPrint()
        {
            var userId = _userManager.GetUserId(User);
            var userDetail = await _userManager.FindByIdAsync(userId);
            var model = new MentorViewModel();

            if (userDetail != null)
            {

                var course = (from u in _myDb.Users
                              join ui in _myDb.UserInfo on u.User_Id equals ui.User_Id
                              where u.User_Email == userDetail.Email
                              select ui.Course).FirstOrDefault();
                
                model.User = new UserModel
                {
                    User_First_Name = userDetail.FirstName,
                };

                model.UserInfo = new UserInfoModel
                {
                    Course = course,
                };
            }

            return View(model);
        }

        [Authorize(Roles = "User")]
        public IActionResult PaymentConfirmation()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult DisplayPayment()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult Payment()
        {
            var domain = "http://localhost:5006/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"User/PaymentConfirmation",
                CancelUrl = domain + $"Identity/Account/Login",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 1500,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Payment for Exam",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }









    }
}
