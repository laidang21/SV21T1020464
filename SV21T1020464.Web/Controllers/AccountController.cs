using Microsoft.AspNetCore.Mvc;

namespace SV21T1020464.Web.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}