﻿using Microsoft.Data.SqlClient;     

namespace SV21T1020464.DataLayers.SQLServer
{
    /// <summary>
    /// lớp cơ sở(lớp cha) cảu các lớp cài đặt các phép xử lý dữ liệu trên SQL server
    /// </summary>
    public abstract class BaseDAL
        
    {
    /// <summary>
    /// Chuỗi than số kết nối đến CSDL SQL Server
    /// </summary>
        protected string connectionString = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseDAL(string connectionString) {
        
            this.connectionString = connectionString; 
        }
        /// <summary>
        /// Tạo và mở 1 kết nối đến CSDL(SQL server)
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }


    }
           
}
