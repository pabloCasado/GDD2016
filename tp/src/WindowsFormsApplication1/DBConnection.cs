using System.Data.SqlClient;
using System.Configuration;

namespace WindowsFormsApplication1
{
    class DBConnection
    {
        string server = ConfigurationManager.AppSettings["server"].ToString();
        string user = ConfigurationManager.AppSettings["user"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();

        public SqlConnection openConnection()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "SERVER=" + server + "\\SQLSERVER2102;DATABASE=GD1C2016;UID=" + user + ";PASSWORD=" + password + ";";
            return con;
        }
    }
}
