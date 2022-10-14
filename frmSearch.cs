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
namespace CarsDatabase
{
    public partial class frmSearch : Form
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        DataTable data = new DataTable();
        ConnectionDB db = new ConnectionDB();

        public frmSearch()
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(db.GetConnection());
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {

            cbField.Items.Add("Make");                  //Adding the fields in the drop down
            cbField.Items.Add("EngineSize");
            cbField.Items.Add("RentalPerDay");
            cbField.Items.Add("Available");

            cbOperator.Items.Add("=");                  //Adding the operators in the drop down
            cbOperator.Items.Add("<");
            cbOperator.Items.Add(">");
            cbOperator.Items.Add("<=");
            cbOperator.Items.Add(">=");

            string sql = "SELECT * FROM tblCar";
            DataSet dataSet = new DataSet();
            SqlConnection conn = new SqlConnection(db.GetConnection());   
            SqlCommand cmd = new SqlCommand(sql, conn);       
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            conn.Open();
            sda.Fill(dataSet, "tblCar");

            string query = cmd.CommandText;
            foreach (SqlParameter p in cmd.Parameters)
            {
                query = query.Replace(p.ParameterName, p.Value.ToString());
            }

            dataGridView1.DataSource = dataSet.Tables["tblCar"].DefaultView;
            dataGridView1.Columns["RentalPerDay"].DefaultCellStyle.Format = "C2";
            dataGridView1.Columns["RentalPerDay"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            conn.Close();

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                int value;

                if (cbField.Text == "" || cbOperator.Text == "" || txtValue.Text == "")
                {
                    MessageBox.Show("Select the fields", "Required fields", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                if (cbField.SelectedItem == "Available")
                {
                    if(txtValue.Text == "Yes" || txtValue.Text == "yes" || txtValue.Text=="YES")
                    {
                        value = 1;
                    }
                    else 
                    {
                        value = 0;
                    }

                    string sql = $"SELECT * FROM tblCar WHERE {cbField.Text} {@cbOperator.Text} '{value}'";
                    DataSet dataSet = new DataSet();
                    SqlConnection conn = new SqlConnection(db.GetConnection());     //Connect to database             
                    SqlCommand cmd = new SqlCommand(sql, conn);                     //Create sql command using sql and conn
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);                   //Create sql adapter using cmd
                    conn.Open();                                                    //open our connection
                    sda.Fill(dataSet, "tblCar");                                    //fill it with the database object

                    string query = cmd.CommandText;
                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }

                    dataGridView1.DataSource = dataSet.Tables["tblCar"].DefaultView;
                }
                else
                {
                    string sql = $"SELECT * FROM tblCar WHERE {cbField.Text} {@cbOperator.Text} '{txtValue.Text}'";
                    DataSet dataSet = new DataSet();
                    SqlConnection conn = new SqlConnection(db.GetConnection());     //Connect to database             
                    SqlCommand cmd = new SqlCommand(sql, conn);                     //Create sql command using sql and conn
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);                   //Create sql adapter using cmd
                    conn.Open();                                                    //open our connection
                    sda.Fill(dataSet, "tblCar");                                    //fill it with the database object

                    string query = cmd.CommandText;
                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        query = query.Replace(p.ParameterName, p.Value.ToString());
                    }

                    dataGridView1.DataSource = dataSet.Tables["tblCar"].DefaultView;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);             
            }
            con.Close();        
        }

        private void btnClose_Click(object sender, EventArgs e)
        {   if(MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                frmCars form1 = new frmCars();
                form1.ShowDialog();
                this.Close();
            }
            
        }

        private void cbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValue.Text = String.Empty;
            lblavblValue.Text = "No for Not Available,Yes for Available";
            lblavblValue.Visible= false;

            if (cbField.Text =="Make" ||  cbField.Text== "Available")
            {
                cbOperator.Items.Clear();
                cbOperator.Items.Add("=");
            }
            else if(cbField.Text =="EngineSize" || cbField.Text == "RentalPerDay")
            {
                cbOperator.Items.Clear();
                cbOperator.Items.Add("=");
                cbOperator.Items.Add("<");
                cbOperator.Items.Add(">");
                cbOperator.Items.Add("<=");
                cbOperator.Items.Add(">=");
            }

            if (cbField.Text == "Available")
            {
                lblavblValue.Visible = true;                             
            }
        }

        private void gbSearch_Enter(object sender, EventArgs e)
        {

        }
    }
}
