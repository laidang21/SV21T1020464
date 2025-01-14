using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020464.BusinessLayers;
using SV21T1020464.DomainModels;

namespace SV21T1020464.Shop.Controllers
{
    public class ProductController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Detail(int productID)
        {
            Product? model = SV21T1020464.BusinessLayers.ProductDataService.GetProductById(productID);
            if (model == null)
            {
                // Trường hợp không tìm thấy sản phẩm
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [Authorize]
        public IActionResult AddtoCart(int ProductID, int quantity)
        {
            if (quantity <= 0)
            {
                TempData["ErrorMessage"] = "Số lượng không hợp lệ. Vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }

            // Lấy thông tin khách hàng
            var customer = User.GetUserData();
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng.";
                return RedirectToAction("Login", "Account");
            }

            // Lấy hoặc tạo giỏ hàng
            var cart = CartDataService.getCartByCustomerID(Convert.ToInt32(customer.UserId));
            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerID = Convert.ToInt32(customer.UserId),
                    Sum = 0
                };
                CartDataService.AddCart(cart);
                cart = CartDataService.getCartByCustomerID(Convert.ToInt32(customer.UserId));
            }

            // Kiểm tra sản phẩm có tồn tại không
            var product = ProductDataService.GetProductById(ProductID);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            // Kiểm tra sản phẩm đã tồn tại trong giỏ hàng chưa
            var exists = CartDataService.checkProductExists(cart.CartId, product.ProductID);
            if (exists == null)
            {
                // Sản phẩm chưa tồn tại trong giỏ hàng
                var cartDetail = new Cartdetail
                {
                    Quantity = quantity,
                    Price = product.Price,
                    CartId = cart.CartId,
                    ProductId = product.ProductID
                };
                int result = CartDataService.AddCartDetail(cartDetail);

                if (result > 0)
                {
                    cart.Sum += quantity; // Cộng số lượng vào tổng
                    CartDataService.SaveCart(cart);
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể thêm sản phẩm vào giỏ hàng.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // Sản phẩm đã tồn tại, cập nhật số lượng
                int newQuantity = exists.Quantity + quantity;
                bool updated = CartDataService.SaveCartdetail(cart.CartId, product.ProductID, newQuantity);

                if (!updated)
                {
                    TempData["ErrorMessage"] = "Không thể cập nhật số lượng sản phẩm trong giỏ hàng.";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng.";
            return RedirectToAction("Index", "Home");
        }
    }
}

