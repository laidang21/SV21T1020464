using Dapper;
using SV21T1020464.DataLayers.SQLServer;
using SV21T1020464.DataLayers;
using SV21T1020464.DomainModels;
using System.Data;

namespace SV21T1020464.DataLayers.SQLServer
{
    public class EmployeeAccountDAL : BaseDAL, IUserAccountDAL
    {
        public EmployeeAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            bool result = false;
            using (var cn = OpenConnection())
            {
                var sql = @"update Employees 
                            set Password = @newPassword 
                            where Email = @userName and Password = @oldPassword";
                var parameters = new
                {
                    userName = userName,
                    oldPassword = oldPassword,
                    newPassword = newPassword
                };
                result = cn.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                cn.Close();
            }
            return result;
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"
                            SELECT 
                                EmployeeID AS UserId, 
                                Email AS UserName, 
                                FullName AS DisplayName, 
                                Photo, 
                                RoleNames 
                            FROM Employees 
                            WHERE Email = @Email AND Password = @Password";

                var parameters = new
                {
                    Email = username,
                    Password = password
                };
                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }
       
    }
}