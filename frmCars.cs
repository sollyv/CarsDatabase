using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;



namespace CarsDatabase
{
    public partial class frmCars : Form
    {
        SqlConnection con=new SqlConnection();
        SqlCommand cmd=new SqlCommand();
        DataTable data=new DataTable();
        ConnectionDB db=new ConnectionDB();
        int rowno;

        public frmCars()
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(db.GetConnection());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {   
            this.Hide();
            frmSearch form2=new frmSearch();
            form2.ShowDialog();            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit", "EXIT",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        
        private void frmCars_Load(object sender, EventArgs e)
        {
            rowno = 0;
            DataTable data = db.UploadData();
            displayData(data);
            tbRecordNo.Text = $"1 of {data.Rows.Count.ToString()}";
            tbRecordNo.Enabled = false;
        }

        private void displayData(DataTable data)
        {
            txtRegNo.Text = data.Rows[rowno][0].ToString();
            txtMake.Text = data.Rows[rowno][1].ToString();
            txtEngSize.Text = data.Rows[rowno][2].ToString();
            dtpDate.Text = data.Rows[rowno][3].ToString();
            txtRent.Text = $"£{Convert.ToDecimal(data.Rows[rowno][4]).ToString("0.00")}";
            if (Convert.ToInt32(data.Rows[rowno][5]) == 1)
            { cbAvailable.Checked = true; }
            else { cbAvailable.Checked = false; }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DataTable dt = db.UploadData();
            displayData(dt);
            tbRecordNo.Text = $"{rowno + 1} of {dt.Rows.Count.ToString()}";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //Error handling to check if any textboxes are empty
                if (txtRegNo.Text == "" || txtMake.Text == "" || txtRent.Text == "" || dtpDate.Text == "" || txtEngSize.Text == "")
                {
                    MessageBox.Show("Required fields", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool x, y, a;
                x = txtEngSize.Text.EndsWith("L");
                y = txtEngSize.Text.EndsWith("l");
                a = txtRent.Text.StartsWith("£");
                string engsize;
                string final_rent;

                //Checking if the input for engine size is with or without L or l
                if(!x && !y) { engsize = txtEngSize.Text + "L"; }
                else if (y) 
                {
                    string[] eng_size = txtEngSize.Text.Split('l');
                    engsize = eng_size[0] + "L";
                }
                else {engsize = txtEngSize.Text;}

                //Checking whether input for Rent is with or without £
                if (a){final_rent = txtRent.Text.Substring(1);}
                else{final_rent = txtRent.Text;}
                    

                SqlConnection con = new SqlConnection(db.GetConnection());
                con.Open();
                DateTime dateReg = DateTime.Parse(dtpDate.Text);
                cmd = new SqlCommand("INSERT INTO tblCar (VehicleRegNo,Make,EngineSize,DateRegistered,RentalPerDay,Available) VALUES (@VehicleRegNo,@Make,@EngineSize,@DateRegistered,@RentalPerDay,@Available) ", con);
                cmd.Parameters.AddWithValue("@VehicleRegNo", txtRegNo.Text);
                cmd.Parameters.AddWithValue("@Make", txtMake.Text);
                cmd.Parameters.AddWithValue("@EngineSize", engsize);
                cmd.Parameters.AddWithValue("@DateRegistered", dateReg);
                cmd.Parameters.AddWithValue("@RentalPerDay", final_rent);

                if (cbAvailable.Checked)
                {
                    cmd.Parameters.AddWithValue("@Available", 1);
                }
                else { cmd.Parameters.AddWithValue("@Available", 0); }
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show($"The following car details has been added\nCar Registration Number:{txtRegNo.Text}" +
                    $"\nCar Make:{txtMake.Text}\nEngine Size:{txtEngSize.Text}\nRegistration Date:{dtpDate.Text}" +
                    $"\nRent per day:{txtRent.Text}", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                con.Close();
                MessageBox.Show("Registration Number already exists in the table.Or Error during adding","Oops..",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }          
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(db.GetConnection());
                con.Open();
                DateTime dateReg = DateTime.Parse(dtpDate.Text);
                cmd = new SqlCommand("DELETE FROM tblCar WHERE VehicleRegNo=@VehicleRegNo", con);                
                cmd.Parameters.AddWithValue("@VehicleRegNo", txtRegNo.Text);
                string query = cmd.CommandText;
                foreach(SqlParameter p in cmd.Parameters)
                {
                    query = query.Replace(p.ParameterName,p.Value.ToString());
                }
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record deleted");

                foreach(Control c in Controls)
                {
                    if(c is TextBox)
                    {
                        c.Text=String.Empty;
                    }
                }
                cbAvailable.Checked = false;
               
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRegNo.Text == "" || txtMake.Text == "" || txtRent.Text == "" || dtpDate.Text == "" || txtEngSize.Text == "")
                {
                    MessageBox.Show("Required fields", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool x, y, a;
                x = txtEngSize.Text.EndsWith("L");
                y = txtEngSize.Text.EndsWith("l");
                a = txtRent.Text.StartsWith("£");
                string engsize;
                string final_rent;

                //Checking if the input for engine size is with or without L or l
                if (!x && !y) { engsize = txtEngSize.Text + "L"; }
                else if (y)
                {
                    string[] eng_size = txtEngSize.Text.Split('l');
                    engsize = eng_size[0] + "L";
                }
                else { engsize = txtEngSize.Text; }

                //Checking whether input for Rent is with or without £
                if (a) { final_rent = txtRent.Text.Substring(1); }
                else { final_rent = txtRent.Text; }


                SqlConnection con = new SqlConnection(db.GetConnection());
                con.Open();
                DateTime dateReg = DateTime.Parse(dtpDate.Text);
                cmd = new SqlCommand("UPDATE tblCar SET VehicleRegno=@VehicleRegno,Make=@Make,EngineSize=@EngineSize,DateRegistered=@DateRegistered,RentalPerDay=@RentalPerDay,Available=@Available WHERE VehicleRegNo=@VehicleRegNo", con);
                cmd.Parameters.AddWithValue("@VehicleRegNo", txtRegNo.Text);
                cmd.Parameters.AddWithValue("@Make", txtMake.Text);
                cmd.Parameters.AddWithValue("@EngineSize", engsize);
                cmd.Parameters.AddWithValue("@DateRegistered", dateReg);
                cmd.Parameters.AddWithValue("@RentalPerDay", final_rent);

                if (cbAvailable.Checked)
                {
                    cmd.Parameters.AddWithValue("@Available", 1);
                }
                else { cmd.Parameters.AddWithValue("@Available", 0); }
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show($"The following car details has been updated\nCar Registration Number:{txtRegNo.Text}" +
                    $"\nCar Make:{txtMake.Text}\nEngine Size:{txtEngSize.Text}\nRegistration Date:{dtpDate.Text}" +
                    $"\nRent per day:{txtRent.Text}", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            rowno = 0;
            DataTable data = db.UploadData();
            displayData(data);
            tbRecordNo.Text = $"1 of {data.Rows.Count.ToString()}";
            MessageBox.Show("First Record Reached","Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            DataTable dt = db.UploadData();
            if (rowno > 0)
            {
                rowno--;
                displayData(dt);
                tbRecordNo.Text = $"{rowno + 1} of {dt.Rows.Count.ToString()}";
            }
            else
            {
                MessageBox.Show("This is the first record", "First Record", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DataTable dt = db.UploadData();
            if (rowno < dt.Rows.Count - 1)
            {
                rowno++;
                displayData(dt);
                tbRecordNo.Text = $"{rowno + 1} of {dt.Rows.Count.ToString()}";
            }
            else
            {
                MessageBox.Show("This is the last record", "Last Record", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            DataTable data = db.UploadData();
            rowno = data.Rows.Count - 1;
            displayData(data);
            tbRecordNo.Text = $"{(data.Rows.Count).ToString()} of {data.Rows.Count.ToString()}";
            MessageBox.Show("Last Record Reached", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}