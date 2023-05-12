using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer_LKS_Hotel
{
    public partial class CheckIn : Form
    {
        public CheckIn()
        {
            InitializeComponent();
        }
        void onload()
        {
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.None;
        }
        private void CheckIn_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select * from Reservation where BookingCode='"+ textBoxCode.Text +"'", Utility.conn);
            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                Utility.conn.Close();
                string query = "select ReservationRoom.ID, Room.RoomNumber, Room.RoomFloor, RoomType.Name as 'RoomType', ReservationRoom.StartDateTime from Reservation join ReservationRoom on Reservation.ID = ReservationRoom.ReservationID join Room on ReservationRoom.RoomID = Room.ID join RoomType on Room.RoomTypeID = RoomType.ID  where Reservation.BookingCode = '" + textBoxCode.Text + "' AND CheckInDateTime > getdate()";
                dataGridViewCustomer.DataSource = Utility.getData(query);
                dataGridViewCustomer.Columns[0].Visible = false;

                if(dataGridViewCustomer.Rows.Count == 0)
                {
                    MessageBox.Show("Booking Code Has Been Checked In", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Utility.conn.Close();
                MessageBox.Show("Booking Code Not Found", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        int id_cust = 0;
        int Condition;
        bool isValidEmail(string email)
        {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var mail = new MailAddress(email);
                return mail.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        bool validateIn()
        {
            if (textBoxName.Text == "" || textBoxEmail.Text == "" || textBoxAge.Text == "" || textBoxNIK.Text == "")
            {
                MessageBox.Show("Customer Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (radioButton1.Checked && radioButton2.Checked)
            {
                MessageBox.Show("Gender Must Be Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (textBoxNIK.TextLength != 16)
            {
                MessageBox.Show("NIK Must Be 16 Digit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (isValidEmail(textBoxEmail.Text) == false)
            {
                MessageBox.Show("Customer Email Doesn't Valid", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand nik = new SqlCommand("select * from Customer where NIK=@nik", Utility.conn);
            nik.Parameters.AddWithValue("@nik", textBoxNIK.Text);
            Utility.conn.Open();
            SqlDataReader reader = nik.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                Utility.conn.Close();
                MessageBox.Show("NIK Has Been Used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Utility.conn.Close();

            SqlCommand phone = new SqlCommand("select * from Customer where PhoneNumber=@phone", Utility.conn);
            phone.Parameters.AddWithValue("@phone", textBoxPhoneNumber.Text);
            Utility.conn.Open();
            SqlDataReader read = phone.ExecuteReader();
            read.Read();
            if (read.HasRows)
            {
                Utility.conn.Close();
                MessageBox.Show("Phone Number Has Been Used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Utility.conn.Close();

            return true;
        }

        bool validateUp()
        {
            if (textBoxName.Text == "" || textBoxEmail.Text == "" || textBoxAge.Text == "" || textBoxNIK.Text == "")
            {
                MessageBox.Show("Customer Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (radioButton1.Checked && radioButton2.Checked)
            {
                MessageBox.Show("Gender Must Be Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (textBoxNIK.TextLength != 16)
            {
                MessageBox.Show("NIK Must Be 16 Digit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (isValidEmail(textBoxEmail.Text) == false)
            {
                MessageBox.Show("Customer Email Doesn't Valid", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand nik = new SqlCommand("select * from Customer where NIK=@nik", Utility.conn);
            nik.Parameters.AddWithValue("@nik", textBoxNIK.Text);
            Utility.conn.Open();
            SqlDataReader reader = nik.ExecuteReader();
            reader.Read();
            if (reader.HasRows && id_cust != reader.GetInt32(0))
            {
                Utility.conn.Close();
                MessageBox.Show("NIK Has Been Used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Utility.conn.Close();

            SqlCommand phone = new SqlCommand("select * from Customer where PhoneNumber=@phone", Utility.conn);
            phone.Parameters.AddWithValue("@phone", textBoxPhoneNumber.Text);
            Utility.conn.Open();
            SqlDataReader read = phone.ExecuteReader();
            read.Read();
            if (read.HasRows && id_cust != read.GetInt32(0))
            {
                Utility.conn.Close();
                MessageBox.Show("Phone Number Has Been Used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Utility.conn.Close();

            return true;
        }
        private void textBoxPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select top(1) * from Customer where PhoneNumber like '%"+ textBoxPhoneNumber.Text + "%'", Utility.conn);

            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                Condition = 2; 
                id_cust = reader.GetInt32(0);
                textBoxName.Text = reader.GetString(1);
                textBoxNIK.Text = reader.GetString(2);
                textBoxEmail.Text = reader.GetString(3);
                if(reader["Gender"].ToString() == "L")
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
                textBoxAge.Text = reader.GetInt32(6).ToString();

                Utility.conn.Close();
            }
            else
            {
                id_cust = 0;
                textBoxName.Text = "";
                textBoxNIK.Text = "";
                textBoxEmail.Text = "";
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                textBoxAge.Text = "";
                Utility.conn.Close();
                Condition = 1;
            }
            Utility.conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            char Gender;
            if (radioButton1.Checked)
            {
                Gender = 'L';
            }
            else
            {
                Gender = 'P';
            }
            if (Condition == 1)
            {
                if (validateIn())
                {
                    SqlCommand cmd = new SqlCommand("insert into Customer values(@name,@nik,@email,@gender,@phone,@dob)", Utility.conn);
                    cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                    cmd.Parameters.AddWithValue("@nik", textBoxNIK.Text);
                    cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);
                    cmd.Parameters.AddWithValue("@gender", Gender);
                    cmd.Parameters.AddWithValue("@phone", textBoxPhoneNumber.Text);
                    cmd.Parameters.AddWithValue("@dob", textBoxAge.Text);

                    Utility.conn.Open();
                    cmd.ExecuteNonQuery();
                    Utility.conn.Close();

                    for (int i = 0; i < dataGridViewCustomer.Rows.Count; i++)
                    {
                        SqlCommand cmd1 = new SqlCommand("update ReservationRoom set CheckInDateTime=getdate(), StartDateTime=getdate() where ID=" + dataGridViewCustomer.Rows[i].Cells[0].Value + "", Utility.conn);
                        Utility.conn.Open();
                        cmd1.ExecuteNonQuery();
                        Utility.conn.Close();

                        MessageBox.Show("Success", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.Close();
                }
            }

            if (Condition == 2)
            {
                if (validateUp())
                {
                    SqlCommand cmd = new SqlCommand("update Customer set Name=@name,NIK=@nik,Email=@email,Gender=@gender,PhoneNumber=@phone,Age=@dob where ID=" + id_cust + "", Utility.conn);
                    cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                    cmd.Parameters.AddWithValue("@nik", textBoxNIK.Text);
                    cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);
                    cmd.Parameters.AddWithValue("@gender", Gender);
                    cmd.Parameters.AddWithValue("@phone", textBoxPhoneNumber.Text);
                    cmd.Parameters.AddWithValue("@dob", textBoxAge.Text);

                    Utility.conn.Open();
                    cmd.ExecuteNonQuery();
                    Utility.conn.Close();

                    for (int i = 0; i < dataGridViewCustomer.Rows.Count; i++)
                    {
                        SqlCommand cmd1 = new SqlCommand("update ReservationRoom set CheckInDateTime=getdate(), StartDateTime=getdate() where ID=" + dataGridViewCustomer.Rows[i].Cells[0].Value + "", Utility.conn);
                        Utility.conn.Open();
                        cmd1.ExecuteNonQuery();
                        Utility.conn.Close();

                        MessageBox.Show("Success", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxNIK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
