using Azure;
using Dapper;
using SV21T1020464.DomainModels;
using System.Data;


namespace SV21T1020464.DataLayers.SQLServer
{
    public class ProductDAL : BaseDAL, IProductDAL<Product>
    {
        public ProductDAL(string connectString) : base(connectString)
        {
        }

        public int Add(Product data)
        {
            int id = 0;
            using (var conn = OpenConnection())
            {
                string sql = @"if exists(select * from Products where ProductName = @ProductName and CategoryID = @categoryID and SupplierID = @SupplierID)
                                select -1
                               else
                                   begin
                                        insert into Products( ProductName, ProductDescription, CategoryID, SupplierID, Unit, Price, Photo, IsSelling)
                                        values( @ProductName, @ProductDescription, @CategoryID, @SupplierID, @Unit, @Price, @Photo, @IsSelling);
                                        select @@identity
                                    end";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    CategoryID = data.CategoryID,
                    SupplierID = data.SupplierID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling
                };
                id = conn.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                conn.Close();
            }
            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into ProductAttributes(ProductID,AttributeName,AttributeValue,DisplayOrder)
                                    values(@ProductID,@AttributeName,@AttributeValue,@DisplayOrder);
                                    select @@identity;
                                ";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into ProductPhotos(ProductID,Photo,Description,DisplayOrder,IsHidden)
                                    values(@ProductID,@Photo,@Description,@DisplayOrder,@IsHidden);
                                    select @@identity;
                                ";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    Photo = data.Photo ?? "",
                    DisplayOrder = data.DisplayOrder,
                    Description = data.Description ?? "",
                    IsHidden = data.IsHidden
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;

        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {

            int count = 0;
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                string sql = @" select count(*)
                            from Products
                            where (ProductName like @searchValue)
                                    and (@categoryID = 0 or CategoryID = @categoryID)
                                    and (@supplierID = 0 or SupplierID = @supplierID)
                                    and (Price >= @minPrice)
                                    and (@maxPrice <= 0 or Price <= @maxPrice) 
                        ";
                var parameters = new
                {
                    searchValue = searchValue,
                    categoryID = categoryID,
                    supplierID = supplierID,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int ProductID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos where ProductID = @productID
                            delete from ProductAttributes where ProductID = @productID
                            delete from Products where ProductID = @productID";
                var parameters = new
                {
                    productID = ProductID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteAttribute(long attributeID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes where AttributeID = @AttributeID";
                var parameters = new
                {
                    AttributeID = attributeID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long photoID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                string sql = @" delete from ProductPhotos
                                where PhotoID = @PhotoID";
                var parameters = new
                {
                    PhotoID = photoID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int ProductID)
        {
            var product = new Product();
            using (var connection = OpenConnection())
            {
                string sql = @" select * from Products
                                where ProductID = @ProductID";
                var parameters = new
                {
                    ProductID = ProductID
                };
                product = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return product;
        }

        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute productAttribute = new ProductAttribute();
            using (var connection = OpenConnection())
            {
                string sql = @"select * from ProductAttributes where AttributeID = @AttributeID";
                var parameters = new
                {
                    AttributeID = attributeID
                };
                productAttribute = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return productAttribute;
        }

        public ProductPhoto? GetPhoto(long photoID)
        {
            ProductPhoto productPhoto = new ProductPhoto();
            using (var connection = OpenConnection())
            {
                string sql = @"select * from ProductPhotos where PhotoID = @PhotoID";
                var parameters = new
                {
                    PhotoID = photoID
                };
                productPhoto = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return productPhoto;
        }

        public List<Product> getSimilarProduct(int categoryID)
        {

            List<Product> products = new List<Product>();
            using (var connection = OpenConnection())
            {
                string sql = @"SELECT TOP (20) *
                   FROM Products
                   Where CategoryID = @CategoryID";
                var parameters = new
                {
                    CategoryID = categoryID
                };
                products = connection.Query<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return products;
        }

        public bool InUse(int ProductID)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int ProductID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Products where ProductID = @ProductID and IsSelling = 1)
                                select 1
                            else
                                select 0;";
                var parameters = new
                {
                    ProductID = ProductID
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public List<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> products = new List<Product>();
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                string sql = @"SELECT *
                                FROM (
                                        SELECT *,ROW_NUMBER() OVER(ORDER BY ProductName) AS RowNumber
                                        FROM Products
                                        WHERE (@SearchValue = N'' OR ProductName LIKE @SearchValue)
                                            and (@CategoryID = 0 OR CategoryID = @CategoryID)
                                            and (@SupplierID = 0 OR SupplierId = @SupplierID)
                                            and (Price >= @MinPrice)
                                            and (@MaxPrice <= 0 OR Price <= @MaxPrice)) AS t
                                WHERE (@PageSize = 0)
                                       OR (RowNumber BETWEEN (@Page - 1)*@PageSize + 1 AND @Page * @PageSize)
                                        order by RowNumber
                                ";
                var parameters = new
                {
                    SearchValue = searchValue,
                    CategoryID = categoryID,
                    SupplierID = supplierID,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    Page = page,
                    PageSize = pageSize

                };
                products = connection.Query<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return products;
        }
        public IList<ProductAttribute> ListAttributes(int productID)
        {
            List<ProductAttribute> productAttributes = new List<ProductAttribute>();
            using (var connection = OpenConnection())
            {
                string sql = @"select * 
                               from ProductAttributes 
                               where ProductID = @ProductID
                               ";
                var parameters = new
                {
                    ProductID = productID
                };
                productAttributes = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return productAttributes;
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            List<ProductPhoto> productPhotos = new List<ProductPhoto>();
            using (var connection = OpenConnection())
            {
                string sql = @"select *
                              from ProductPhotos 
                              where ProductID = @ProductID 
                              ";
                var parameters = new
                {
                    ProductID = productID
                };
                productPhotos = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return productPhotos;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (var conn = OpenConnection())
            {
                string sql = @" if not exists(select * from Products where ProductID <> @ProductID and ProductName = @ProductName and CategoryID = @categoryID and SupplierID = @SupplierID)
                                    begin
                                        update Products 
                                        set 
                                            ProductName = @ProductName,
                                            ProductDescription = @ProductDescription,
                                            CategoryID = @categoryID,
                                            SupplierID = @SupplierID,
                                            Unit = @Unit,
                                            Price = @Price,
                                            Photo  = @Photo,
                                            IsSelling = @IsSelling
                                        where ProductID = @ProductID;
                                    end";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    ProductID = data.ProductID,
                    CategoryID = data.CategoryID,
                    SupplierID = data.SupplierID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                };
                result = conn.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;

            }
            return result;
        }


        public bool UpdateAttribute(ProductAttribute data)
        {
          
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" Update ProductAttributes
                                  set 
                                    ProductID = @ProductID,
                                    AttributeName = @AttributeName,
                                    AttributeValue = @AttributeValue,
                                    DisplayOrder = @DisplayOrder
                             Where AttributeID  = @AttributeID";
                var parameters = new
                {
                    AttributeID = data.AttributeID,
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
           
            bool result = false;
            using (var connection = OpenConnection())
            {

                var sql = @" Update ProductPhotos
                             set 
                                ProductID = @ProductID,
                                Photo= @Photo,
                                Description = @Description,
                                DisplayOrder = @DisplayOrder,
                                IsHidden = @IsHidden
                            Where PhotoID  = @PhotoID";
                var parameters = new
                {
                    PhotoID = data.PhotoID,
                    ProductID = data.ProductID,
                    Photo = data.Photo ?? "",
                    DisplayOrder = data.DisplayOrder,
                    Description = data.Description ?? "",
                    IsHidden = data.IsHidden,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
