using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SV21T1020464.BusinessLayers;
using SV21T1020464.DataLayers;


namespace SV21T1020464.Shop.Controllers
{
    public class AccountController : Controller
    {
        public const string SUM = "sum";
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            ViewBag.Username = username;
            //kiem tra thong tin ddau vao
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("Error", "Nhập tên và mật khẩu");
                return View();
            }
            var userAccount = UserAccountService.Authorize(UserAccountService.UserTypes.Customer, username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Dang nhap that bai");
                return View();
            }
            string customerID = userAccount.UserId;
            int sum = 0;
            sum = SV21T1020464.BusinessLayers.CartDataService.getCartByCustomerID(Convert.ToInt32(customerID)).Sum;
            // Dang nhap thanh cong:
            var userData = new WebUserData
            {
                UserId = userAccount.UserId,
                UserName = userAccount.UserName,
                DisplayName = userAccount.DisplayName,
                Roles = userAccount.RoleNames.Split(",").ToList(),
                sum = sum + ""
            };

            //
            await HttpContext.SignInAsync(userData.CreatePrincipal());
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        public IActionResult Cart()
        {
           var user = User.GetUserData();
            int userID = Convert.ToInt32(user.UserId);
            var model = SV21T1020464.BusinessLayers.CartDataService.listViewCart(userID);
            return View(model);
        }
        public IActionResult EditProfile()
        {
            return View();
        }
        public IActionResult ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword)
        {
            if (Request.Method == "POST")
            {
                if (confirmPassword.Trim().Equals(newPassword.Trim()) == false)
                    ModelState.AddModelError("confirmPass", "Mật khẩu không khớp");
                if (ModelState.IsValid == false)
                {
                    return View();
                }
                else
                {
                    var result = UserAccountService.ChangePassword(UserAccountService.UserTypes.Customer,userName, oldPassword, newPassword);
                    if (result == true)
                    {
                        return RedirectToAction("Logout");
                    }
                    else
                    {
                        ModelState.AddModelError("oldPass", "Mật khẩu cũ không đúng");
                        return View();
                    }
                }
            }
            return View();
        }
        public IActionResult DeleteFromCart(int cartDetailID)
        {
            CartDataService.DeleteDetail(cartDetailID);
            return RedirectToAction("Cart");
        }

    }
       
}
