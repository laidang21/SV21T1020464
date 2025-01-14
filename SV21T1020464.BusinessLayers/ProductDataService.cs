using SV21T1020464.DataLayers.SQLServer;
using SV21T1020464.DataLayers;
using SV21T1020464.DomainModels;
using SV21T1020464.BusinessLayers;

namespace SV21T1020464.BusinessLayers
{
    public class ProductDataService
    {
        private static readonly IProductDAL<Product> productDB;
        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0,
                                                 string searchValue = "", int categoryId = 0, int supplierId = 0,
                                                 decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue, categoryId, supplierId, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice);
        }
        public static List<Product> getSimilarProducts(int categoryID)
        {

            return productDB.getSimilarProduct(categoryID);
        }
        public static Product? GetProductById(int productId)
        {
            return productDB.Get(productId);
        }
        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }
        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }
        public static bool DeleteProductByID(int productId)
        {
            return productDB.Delete(productId);
        }
        public static bool InUsed(int ProductID)
        {
            return productDB.InUsed(ProductID);
        }
        public static IList<ProductAttribute> ListProductAttributes(int productID)
        {
            return productDB.ListAttributes(productID);
        }
        public static long AddProductAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }
        public static bool UpdateProductAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }
        public static ProductAttribute? GetProductAttributeByID(long attributeID)
        {
            return productDB.GetAttribute(attributeID);
        }
        public static bool DeleteAttributeByID(long attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }
        public static IList<ProductPhoto> ListProductPhotos(int productID)
        {
            return productDB.ListPhotos(productID);
        }
        public static long AddProductPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }
        public static bool UpdateProductPhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }
        public static ProductPhoto? GetProductPhotoByID(long photoID)
        {
            return productDB.GetPhoto(photoID);
        }
        public static bool DeletePhotoByID(long photoID)
        {
            return productDB.DeletePhoto(photoID);
        }
    }
}
