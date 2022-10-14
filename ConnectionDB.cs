using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CarsDatabase
{
    internal class ConnectionDB
    {
        public string GetConnection()
        { 
            string con = @"Data Source=LIBERTIES90\SQLEXPRESS;Initial Catalog=HIRE;Integrated Security=True";
            return con;
        }

        public DataTable UploadData()
        {
            //SqlConnection connString = new SqlConnection(Properties.Settings.Default.connString);

            //creating a connection variable
            SqlConnection conn = new SqlConnection(@"Data Source=LIBERTIES90\SQLEXPRESS;Initial Catalog=HIRE;Integrated Security=True");
            //creating an object of our database
            DataTable dt = new DataTable();
            

            try
            {
                string sql = "SELECT * FROM tblCar";

                //create cmd using conn and sql
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Create sql adapter using cmd
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                string query = cmd.CommandText;

                foreach (SqlParameter p in cmd.Parameters)
                {
                    query = query.Replace(p.ParameterName, p.Value.ToString());
                }

                //open our connection
                conn.Open();

                //fill it with the database object
                sda.Fill(dt);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                conn.Close();
            }

            return dt;
        }
    }
}
