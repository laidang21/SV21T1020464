using Microsoft.AspNetCore.Mvc;
using SV21T1020464.BusinessLayers;
using SV21T1020464.DomainModels;
using SV21T1020464.Web.Models;
using SV21T1020464.Web.AppCodes;
using Microsoft.AspNetCore.Authorization;



namespace SV21T1020464.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMINISTRATOR},{WebUserRoles.EMPLOYEE}")]

    public class ProductController : Controller
    {


        private const int PAGE_SIZE = 30;
        private const string PRODUCT_SEARCH_CONDITON = "ProductSearchCondition";
        public IActionResult Index()
        {
            ProductSearchInput? condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITON);
            if (condition == null)
                condition = new ProductSearchInput
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    MinPrice = 0,
                    MaxPrice = 0,
                };
            return View(condition);
        }
        public IActionResult Search(ProductSearchInput condition)
        {
            int rowCount;
            var data = ProductDataService.ListProducts(out rowCount, condition.Page, condition.PageSize, condition.SearchValue ?? "", condition.CategoryID, condition.SupplierID, condition.MinPrice, condition.MaxPrice);
            ProductSearchResult model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                MinPrice = condition.MinPrice,
                MaxPrice = condition.MaxPrice,
                Data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITON, condition);
            return View(model);

        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật mặt hàng";
            var data = ProductDataService.GetProductById(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung sản phẩm";
            var data = new Product()
            {
                ProductID = 0,
                Photo = "nophoto.png",
                IsSelling = true
            };
            return View("Edit", data);
        }
        public IActionResult Delete(int id)
        {
            var data = ProductDataService.GetProductById(id);
            if (data == null)
                return RedirectToAction("Index");
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProductByID(id);
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public IActionResult Photo(int id = 0, string method = "", long photoid = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";

                    var data = new ProductPhoto()
                    {
                        PhotoID = 0,
                        DisplayOrder = 0,
                        Photo = "nophoto.png",
                        ProductID = id
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Cập nhật ảnh cho mặt hàng";
                    var photo = ProductDataService.GetProductPhotoByID(photoid);
                    if (photo == null)
                        return RedirectToAction("Edit");
                    return View(photo);
                case "delete":
                    ProductDataService.DeletePhotoByID(photoid);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult Attribute(int id = 0, string method = "", long attributeID = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính của mặt hàng";

                    var data = new ProductAttribute()
                    {
                        AttributeID = 0,
                        DisplayOrder = 0,
                        ProductID = id
                    };
                    return View(data);
                case "edit":
                    ViewBag.Title = "Cập nhật thuộc tính của mặt hàng";
                    var Attribute = ProductDataService.GetProductAttributeByID(attributeID);
                    if (Attribute == null)
                        return RedirectToAction("Edit");
                    return View(Attribute);
                case "delete":
                    ProductDataService.DeleteAttributeByID(attributeID);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        public IActionResult Save(Product data, IFormFile? _Photo)
        {
            ViewBag.Title = data.ProductID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";
            decimal _Price = Convert.ToDecimal(data.Price);
            int _CategoryID = Convert.ToInt32(data.CategoryID);
            int _SupplỉeiD = Convert.ToInt32(data.SupplierID);
            if (string.IsNullOrEmpty(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Vui lòng nhập tên mặt hàng");
            if (_CategoryID == 0)
                ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
            if (_SupplỉeiD == 0)
                ModelState.AddModelError(nameof(data.SupplierID), "Vui lòng chọn nhà cung cấp");
            if (string.IsNullOrEmpty(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Vui lòng nhập đơn vị bán");
            if (_Price == null)
                ModelState.AddModelError(nameof(data.Price), "Vui lòng nhập giá bán");
            if (data.Photo == null)
                ModelState.AddModelError(nameof(data.Photo), "Vui lòng chọn ảnh");
            if (ModelState.IsValid == false)
            {
                return View("Edit", data);
            }
            if (_Photo != null)
            {
                string fileName = $"{DateTime.Now.Ticks}-{_Photo.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\products", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    _Photo.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            try
            {
                if (data.ProductID == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError("existed", "Mặt hàng đã tồn tai!");
                        return View("Edit", data);
                    }
                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);
                    if (result == false)
                    {
                        ModelState.AddModelError("existed", "Mặt hàng bị trùng");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //
                ModelState.AddModelError("error", "Hệ thống bị gián đoạn");
                return View("Edit", data);
            }

        }
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? _Photo = null)
        {
            if (string.IsNullOrWhiteSpace(data.Description))
                ModelState.AddModelError(nameof(data.Description), "Mô tả không được để trống");
            if (data.DisplayOrder == 0)
                ModelState.AddModelError(nameof(data.DisplayOrder), "Vui lòng chọn nhập thứ tự hiển thị");

            if (!ModelState.IsValid && data.PhotoID > 0)
            {
                ViewBag.Title = "Cập nhật ảnh cho mặt hàng";
                return View("Photo", data);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                return View("Photo", data);
            }
            if (_Photo != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{_Photo.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\productphotos", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    _Photo.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            if (data.PhotoID == 0)
            {
                long id = ProductDataService.AddProductPhoto(data);
                if (id <= 0)
                {
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = ProductDataService.UpdateProductPhoto(data);
                if (!result)
                {
                    ViewBag.Title = "Cập nhật thông tin cho ảnh mặt hàng";
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Edit", new { id = data.ProductID });

        }
        public IActionResult SaveAttribute(ProductAttribute data)
        {
            if (string.IsNullOrWhiteSpace(data.AttributeName))
                ModelState.AddModelError(nameof(data.AttributeName), "Tên thuộc tính không được để trống");
            if (string.IsNullOrWhiteSpace(data.AttributeValue))
                ModelState.AddModelError(nameof(data.AttributeValue), "Giá trị thuộc tính không được để trống");
            if (data.DisplayOrder == 0)
                ModelState.AddModelError(nameof(data.DisplayOrder), "Thứ tự không được để trống ");

            if (!ModelState.IsValid && data.AttributeID > 0)
            {
                ViewBag.Title = "Cập nhật thuộc tính của mặt hàng";
                return View("Attribute", data);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = "Bổ sung thuộc tính của mặt hàng";
                return View("Attribute", data);
            }
            if (data.AttributeID == 0)
            {
                long id = ProductDataService.AddProductAttribute(data);
                if (id <= 0)
                {
                    ViewBag.Title = "Bổ sung thuộc tính của mặt hàng";
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = ProductDataService.UpdateProductAttribute(data);
                if (!result)
                {
                    ViewBag.Title = "Cập nhật thông tin thuộc tính của ảnh mặt hàng";
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Edit", new { id = data.ProductID });
        }
    }
}

