using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Paybill_STK
{
    public class DBSqlServerUtils
    {
        public static SqlConnection GetDBConnection()
        {
            // Connection String for SQL Server.
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }
    }
}
