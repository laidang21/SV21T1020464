using SV21T1020464.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020464.DataLayers
{
    public interface IProductDAL<T> where T : class
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"> Trang cần hiển thi</param>
        /// <param name="pageSize">Số dòng trên mỗi trang(0 nếu ko phân trang)</param>
        /// <param name="searchValue">Tên mặt hàng cần tìm (rỗng nếu ko tìm kiếm)</param>
        /// <param name="categoryID">Mã loại hàng( 0 nếu ko tìm theo loại hàng)</param>
        /// <param name="supplierID">mã nhà cung cấp( 0 nếu ko tìm theo mã ncc)</param>
        /// <param name="minPrice">Giá nhỏ nhất cần tìm</param>
        /// <param name="maxPrice">Giá lớn nhất </param>
        /// <returns></returns>
        List<Product> List(int page = 1, int pageSize = 0, string searchValue = "",
                            int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);
        /// <summary>
        /// Đếm số luọng mặt hàng tìm đc theo các tham số truyền vào
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="minPrice"></param>
        /// <param name="maximumPrice"></param>
        /// <returns></returns>
        int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);
        /// <summary>
        /// Lấy mặt hàng theo id
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        Product? Get(int ProductID);
        /// <summary>
        /// Bổ sung mặt hàng trả về mã mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// Bổ sung mặt hàng trả về mã hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);
        bool Delete(int productID);
        /// <summary>
        /// Kiểm tra mặt hàng có đơn hàng liên quan hay ko?
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        bool InUsed(int ProductID);
        /// <summary>
        /// Lấy ds ảnh của mặt hàng theo id
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<ProductPhoto> ListPhotos(int productID);
        /// <summary>
        /// Lấy thông tin 1 ảnh theo id
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        ProductPhoto? GetPhoto(long photoID);
        long AddPhoto(ProductPhoto data);
        bool UpdatePhoto(ProductPhoto data);
        bool DeletePhoto(long photoID);
        /// <summary>
        /// Lấy ds các thuộc tính của mặt hàng, sắp xếp theo DíplayOrder
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<ProductAttribute> ListAttributes(int productID);
        ProductAttribute? GetAttribute(long attributeID);
        long AddAttribute(ProductAttribute data);
        bool UpdateAttribute(ProductAttribute data);
        bool DeleteAttribute(long attributeID);
        List<Product> getSimilarProduct(int categoryID);
    }
}
