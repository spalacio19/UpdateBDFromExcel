using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace UpdateBDFromExcel
{
    public class SqlDatabase
    {
        private string _connectionString = "Data Source=172.190.120.3;Encrypt=False;Integrated Security=False;User ID=sa;Password=";

        public void TestConnection()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    // Connection successful
                }
                catch (Exception ex)
                {
                    // Handle connection error
                    MessageBox.Show("Database connection failed: " + ex.Message);
                }
            }
        }
    }
}