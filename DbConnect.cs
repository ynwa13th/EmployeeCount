using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;


namespace EmployeeCount
{
    class DbConnect
    {
        public static string GetConnectionStrings()
        {
            string MyConnectionStr = ConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            return MyConnectionStr;
        }

        public static string sql;
        public static SqlConnection con = new SqlConnection();
        public static SqlCommand cmd = new SqlCommand("", con);
        public static SqlDataReader dr;
        public static SqlDataAdapter da;
        public static DataTable dt;

        public static void OpenConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = GetConnectionStrings();
                    con.Open();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось подключиться к базе данных" + MessageBoxButton.OK);
            }
        }

        public static void CloseConnection()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось отключиться от базы данных" + MessageBoxButton.OK);
            }
        }
    }
}
