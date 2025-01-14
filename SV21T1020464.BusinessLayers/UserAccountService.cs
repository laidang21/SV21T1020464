using SV21T1020464.BusinessLayers;
using SV21T1020464.DataLayers;
using SV21T1020464.DataLayers.SQLServer;
using SV21T1020464.DomainModels;
using static SV21T1020464.BusinessLayers.UserAccountService;


namespace SV21T1020464.BusinessLayers
{
    public class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccountDB;
        private static readonly IUserAccountDAL customerAccountDB;

        static UserAccountService()
        {
            String connectionString = Configuration.ConnectionString;
            employeeAccountDB = new EmployeeAccountDAL(connectionString);
            customerAccountDB = new CustomerAccountDAL(connectionString);
        }

        public static UserAccount? Authorize(UserTypes userTypes, string username, string password)
        {
            if (userTypes == UserTypes.Employee)
            {
                return employeeAccountDB.Authorize(username, password);
            }
            else
            {
                return customerAccountDB.Authorize(username, password);
            }
        }
        public static bool ChangePassword(UserTypes userTypes,string userName, string oldPass, string newPass)
        {
            if (userTypes == UserTypes.Employee)
            {
                return employeeAccountDB.ChangePassword(userName, oldPass, newPass);

            }
            else
                return customerAccountDB.ChangePassword(userName, oldPass, newPass);
        }
 

        public enum UserTypes
        {
            Employee,
            Customer
        }

    }
}