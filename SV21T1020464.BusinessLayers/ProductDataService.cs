using SV21T1020464.DataLayers.SQLServer;
using SV21T1020464.DataLayers;
using SV21T1020464.DomainModels;
using SV21T1020464.BusinessLayers;

namespace SV21T1020464.BusinessLayers
{
    public class ProductDataService
    {
        private static readonly IProductDAL productDB;
        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Danh sách sản phẩm (tìm kiếm, phân trang)
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> ListOfProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue);
            return productDB.List(page, pageSize, searchValue, categoryID, supplierID, minPrice, maxPrice).ToList();
        }
        /// <summary>
        /// Danh sách sản phẩm (tìm kiếm, không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> ListOfProducts(string searchValue = "")
        {
            return productDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 sản phẩm dựa vào mã sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Product? GetProduct(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return productDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1 sản phẩm mới. HÀm tả về id của sản phẩm bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }
        /// <summary>
        /// Xóa thông tin sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int id)
        {
            return productDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra sản phẩm có mã id hiện có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool InUsedProduct(int id)
        {
            return productDB.InUse(id);
        }
        /// <summary>
        /// Danh sách ảnh sản phẩm (tìm kiếm, không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<ProductPhoto> ListOfProductPhotos(int productId)
        {
            return productDB.ListPhotos(productId).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 ảnh sản phẩm dựa vào mã ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ProductPhoto? GetProductPhoto(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return productDB.GetPhoto(id);
        }
        /// <summary>
        /// Bổ sung 1 ảnh sản phẩm mới. HÀm tả về id của ảnh sản phẩm bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long AddProductPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        /// <summary>
        /// Cập nhật thông tin ảnh sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProductPhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        /// <summary>
        /// Xóa thông tin ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteProductPhoto(long id)
        {
            return productDB.DeletePhoto(id);
        }
        /// <summary>
        /// Danh sách thuộc tính sản phẩm (tìm kiếm, không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<ProductAttribute> ListOfProductAttributes(int productId)
        {
            return productDB.ListAttributes(productId).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 thuộc tính sản phẩm dựa vào mã thuộc tính sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ProductAttribute? GetProductAttribute(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return productDB.GetAttribute(id);
        }
        /// <summary>
        /// Bổ sung 1 thuộc tính sản phẩm mới. HÀm tả về id của thuộc tính sản phẩm bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long AddProductAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        /// <summary>
        /// Cập nhật thông tin thuộc tính sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProductAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        /// <summary>
        /// Xóa thông tin thuộc tính sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteProductAttribute(int id)
        {
            return productDB.DeleteAttribute(id);
        }
    }
}