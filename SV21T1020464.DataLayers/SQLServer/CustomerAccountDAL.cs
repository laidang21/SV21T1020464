using SV21T1020464.DataLayers.SQLServer;
using SV21T1020464.DataLayers;
using SV21T1020464.DomainModels;
using Dapper;


namespace SV21T1020464.DataLayers.SQLServer
{
    public class CustomerAccountDAL : BaseDAL, IUserAccountDAL
    {
        public CustomerAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? userAccount = null;
            using (var conn = OpenConnection())
            {
                var sql = @"select CustomerID as UserId,
                                   Email as UserName,
                                   ContactName as DisplayName,
                                   RoleNames
                            from Customers where Email = @Email and Password = @Password";
                var parameters = new
                {
                    Email = username,
                    Password = password
                };
                userAccount = conn.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                conn.Close();
            }
            return userAccount;
        }


        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            bool result = false;
            using (var cn = OpenConnection())
            {
                var sql = @"update Customers 
                            set Password = @newPassword 
                            where Email = @userName and Password = @oldPassword";
                var parameters = new
                {
                    userName = userName,
                    oldPassword = oldPassword,
                    newPassword = newPassword,
                };
                result = cn.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                cn.Close();
            }
            return result;
        }
    }
}