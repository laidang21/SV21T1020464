using Azure;
using Dapper;
using SV21T1020464.DomainModels;
using System.Data;


namespace SV21T1020464.DataLayers.SQLServer
{
    public class ProductDAL : BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Product data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Products where ProductID = @ProductID)
                                select -1
                               else
                                   begin
                                        insert into Products(ProductName, ProductDescription, CategoryID, SupplierID, Unit, Price, Photo, IsSelling)
                                        values(@ProductName, @ProductDescription, @CategoryID, @SupplierID, @Unit, @Price, @Photo, @IsSelling);
                                        select @@identity;
                                    end";
                var parameter = new
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling
                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameter, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                INSERT INTO ProductAttributes(ProductID, AttributeName, AttributeValue, DisplayOrder) 
                VALUES(@ProductID, @AttributeName, @AttributeValue, @DisplayOrder);
                
                SELECT @@IDENTITY";
                var parameter = new
                {
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder
                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameter, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                INSERT INTO ProductPhotos(ProductID, Photo, Description, DisplayOrder, IsHidden) 
                VALUES(@ProductID, @Photo, @Description, @DisplayOrder, @IsHidden);
                
                SELECT @@IDENTITY";
                var parameter = new
                {
                    ProductID = data.ProductID,
                    Photo = data.Photo ?? "",
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden
                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameter, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @" select count(*)
                        from Products
                        where (ProductName like @searchvalue) 
                              or ( CategoryID = @categoryID)
                              or (SupplierID = @supplierID) 
                              or (Price between @minPrice and @maxPrice)
                        ";
                var parameters = new
                {
                    searchValue = $"%{searchValue}%",
                    categoryID = categoryID,
                    supplierID = supplierID,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int productId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" delete from Products
                                where ProductID = @ProductID";
                var parameters = new
                {
                    ProductID = productId
                };
                result = (connection.Execute(sql, parameters, commandType: CommandType.Text)) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteAttribute(long attributeId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM ProductAttributes WHERE AttributeID = @AttributeID";
                var parameters = new
                {
                    AttributeID = attributeId
                };
                result = (connection.Execute(sql, parameters, commandType: CommandType.Text)) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long photoId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM ProductPhotos WHERE PhotoID = @PhotoID";
                var parameters = new
                {
                    PhotoID = photoId
                };
                result = (connection.Execute(sql, parameters, commandType: CommandType.Text)) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productId)
        {
            Product? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @" select * from Products
                                where ProductID = @ProductID";
                var parameters = new
                {
                    productID = productId
                };
                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductAttribute? GetAttribute(long attributeId)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM ProductAttributes WHERE AttributeID = @AttributeID";
                var parameters = new
                {
                    AttributeID = attributeId
                };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductPhoto? GetPhoto(long photoId)
        {
            ProductPhoto? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM ProductAttributes WHERE PhotoID = @PhotoID";
                var parameters = new
                {
                    PhotoID = photoId
                };
                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUse(int productId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Products where ProductID = @ProductID)
                                select 1
                            else
                                select 0;";
                var parameters = new { ProductID = productId };
                result = connection.ExecuteScalar<bool>(sql, parameters, commandType: CommandType.Text);

                connection.Close();
            }
            return result;
        }

        public IList<Product> List(int page = 1, int pagesize = 0, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> data = new List<Product>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT *
                                FROM (
                                        SELECT *,ROW_NUMBER() OVER(ORDER BY ProductName) AS RowNumber
                                        FROM Products
                                        WHERE (@searchValue = N'' OR ProductName LIKE @searchValue)
                                            and (@categoryID = 0 OR CategoryID = @categoryID)
                                            and (@supplierID = 0 OR SupplierId = @supplierID)
                                            and (Price >= @minPrice)
                                            and (@maxPrice <= 0 OR Price <= @maxPrice)) AS t
                                WHERE (@pageSize = 0)
                                       OR (RowNumber BETWEEN (@page - 1)*@pageSize + 1 AND @page * @pageSize)
                                ";
                var parameters = new
                {
                    page = page,
                    pagesize = pagesize,
                    categoryId = categoryId,
                    supplierId = supplierId,
                    minPrice = minPrice,
                    maxPrice = maxPrice,
                    searchValue = $"%{searchValue}%",
                };
                data = connection.Query<Product>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public IList<ProductAttribute> ListAttributes(int productId)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();
            using (var connection = OpenConnection())
            {
                var sql = @"
                            SELECT *
                            FROM ProductAttributes";

                data = connection.Query<ProductAttribute>(sql: sql, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public IList<ProductPhoto> ListPhotos(int productId)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();
            using (var connection = OpenConnection())
            {
                var sql = @"
                            SELECT *
                            FROM ProductPhotos";

                data = connection.Query<ProductPhoto>(sql: sql, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @" update Products 
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
                                ";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName ?? "",
                    ProductDescription = data.ProductDescription ?? "",
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit ?? "",
                    Price = data.Price,
                    Photo = data.Photo ?? "",
                    IsSelling = data.IsSelling
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Products 
                            SET ProductID = @ProductID,
                                AttributeName  = @AttributeName,
                                AttributeValue = @AttributeValue,
                                DisplayOrder = @DisplayOrder,
                            WHERE AttributeID = @AttributeID";
                var parameters = new
                {
                    AttributeID = data.AttributeID,
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName ?? "",
                    AttributeValue = data.AttributeValue ?? "",
                    DisplayOrder = data.DisplayOrder,
                };
                result = (connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text)) > 0;
                connection.Close();
            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE ProductPhotos 
                            SET ProductID = @ProductID
                                Photo = @Photo,
                                Description = @Description,
                                DisplayOrder = @DisplayOrder,
                                IsHidden = @IsHidden,
                            WHERE PhotoID = @PhotoID";
                var parameters = new
                {
                    PhotoID = data.PhotoID,
                    ProductID = data.ProductID,
                    Photo = data.Photo ?? "",
                    Description = data.Description ?? "",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                };
                result = (connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text)) > 0;
                connection.Close();
            }
            return result;
        }
    }
}