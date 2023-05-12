using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer_LKS_Hotel
{
    public partial class MasterEmployee : Form
    {
        private int Condition, ID;
        public MasterEmployee()
        {
            InitializeComponent();
        }
        void onload()
        {
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.None;
            Datagrid();
            Job();
            textBoxPassword.UseSystemPasswordChar = true;
            textBoxConf.UseSystemPasswordChar = true;
            disable();
        }
        void Job()
        {
            string query = "select * from Job";
            comboBoxJob.DataSource = Utility.getData(query);
            comboBoxJob.DisplayMember = "Name";
            comboBoxJob.ValueMember = "ID";
        }
        void enabled()
        {
            textBoxName.Enabled = true;
            textBoxUsername.Enabled = true;
            textBoxPassword.Enabled = true;
            textBoxConf.Enabled = true;
            textBoxEmail.Enabled = true;
            dateTimePicker1.Enabled = true;
            comboBoxJob.Enabled = true;
            richTextBoxAddress.Enabled = true;
            buttonBrow.Cursor = Cursors.Arrow;
            buttonIn.Cursor = Cursors.No;
            buttonUp.Cursor = Cursors.No;
            buttonDel.Cursor = Cursors.No;
            buttonSav.Cursor = Cursors.Arrow;
            buttonCan.Cursor = Cursors.Arrow;
        }

        void disable()
        {
            textBoxName.Enabled = false;
            textBoxUsername.Enabled = false;
            textBoxPassword.Enabled = false;
            textBoxConf.Enabled = false;
            textBoxEmail.Enabled = false;
            dateTimePicker1.Enabled = false;
            comboBoxJob.Enabled = false;
            richTextBoxAddress.Enabled = false;
            buttonBrow.Cursor = Cursors.No;
            buttonIn.Cursor = Cursors.Arrow;
            buttonUp.Cursor = Cursors.Arrow;
            buttonDel.Cursor = Cursors.Arrow;
            buttonSav.Cursor = Cursors.No;
            buttonCan.Cursor = Cursors.No;
        }
        void Clear()
        {
            textBoxName.Text = "";
            textBoxUsername.Text = "";
            textBoxPassword.Text = "";
            textBoxConf.Text = "";
            textBoxEmail.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            richTextBoxAddress.Text = "";
            pictureBox1.Image = null;
        }
        void Datagrid()
        {
            string query = "select Employee.*, Job.Name as 'Job' from Employee join Job on Employee.JobID = Job.ID";
            dataGridView1.DataSource = Utility.getData(query);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void MasterEmployee_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void buttonIn_Click(object sender, EventArgs e)
        {
            if(buttonIn.Cursor != Cursors.No)
            {
                Clear();
                Condition = 1;
                dataGridView1.Enabled = false;
                enabled();
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            if(buttonUp.Cursor != Cursors.No)
            {
                Condition = 2;
                if (ID == 0)
                {
                    MessageBox.Show("Please Select One Row To Update", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    enabled();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (buttonDel.Cursor != Cursors.No)
            {
                if (ID == 0)
                {
                    MessageBox.Show("Please Select One Row To Delete", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete " + dataGridView1.SelectedRows[0].Cells[3].Value.ToString() + " ?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        SqlCommand cmd = new SqlCommand("delete from Employee where ID=" + ID + "", Utility.conn);
                        Utility.conn.Open();
                        cmd.ExecuteNonQuery();
                        Utility.conn.Close();

                        MessageBox.Show("Data Success Deleted", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Datagrid();
                        ID = 0;
                        buttonCan.PerformClick();
                    }
                }
            }
        }
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
            }catch (Exception ex)
            {
                return false;
            }
        }
        bool validateIn()
        {
            if(textBoxName.Text == "" || textBoxEmail.Text == "" || textBoxName.Text == "" || richTextBoxAddress.Text == "" || dateTimePicker1.Value == null || comboBoxJob.SelectedValue == null)
            {
                MessageBox.Show("All Field Must Be Filled Except Photo", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Invalid Date Of Birth", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand cmd = new SqlCommand("select * from Employee where Username = @username", Utility.conn);
            cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);
            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                Utility.conn.Close();
                MessageBox.Show("Username Has Been Used", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Utility.conn.Close();

            if (textBoxPassword.Text != textBoxConf.Text)
            {
                MessageBox.Show("Password Must Be Same With Confirm Password", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (isValidEmail(textBoxEmail.Text) == false)
            {
                MessageBox.Show("Email Not Valid", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        bool validateUp()
        {
            if (textBoxName.Text == "" || textBoxEmail.Text == "" || textBoxName.Text == "" || richTextBoxAddress.Text == "" || dateTimePicker1.Value == null || comboBoxJob.SelectedValue == null)
            {
                MessageBox.Show("All Field Must Be Filled Except Photo", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(textBoxUsername.Text != dataGridView1.SelectedRows[0].Cells[1].Value.ToString())
            {
                SqlCommand cmd = new SqlCommand("select * from Employee where Username = @username", Utility.conn);
                cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);
                Utility.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    Utility.conn.Close();
                    MessageBox.Show("Username Has Been Used", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                Utility.conn.Close();
            }

            if(textBoxPassword.Text != "")
            {
                if (textBoxPassword.Text != textBoxConf.Text)
                {
                    MessageBox.Show("Password Must Be Same With Confirm Password", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            if (isValidEmail(textBoxEmail.Text) == false)
            {
                MessageBox.Show("Email Not Valid", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void buttonSav_Click(object sender, EventArgs e)
        {
            if(buttonSav.Cursor != Cursors.No)
            {
                if (Condition == 1 && validateIn())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("insert into Employee values(@username,@password,@name,@email,@address,@Dob,@job,@photo)", Utility.conn);
                        cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);
                        cmd.Parameters.AddWithValue("@password", Utility.EncPass(textBoxPassword.Text));
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);
                        cmd.Parameters.AddWithValue("@address", richTextBoxAddress.Text);
                        cmd.Parameters.AddWithValue("@Dob", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@job", comboBoxJob.SelectedValue);
                        if (pictureBox1.Image == null)
                        {
                            cmd.Parameters.AddWithValue("@photo", null);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@photo", Utility.EncImage(pictureBox1.Image));
                        }
                        Utility.conn.Open();
                        cmd.ExecuteNonQuery();
                        Utility.conn.Close();

                        MessageBox.Show("Data Success Inserted", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        buttonCan.PerformClick();

                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                if (Condition == 2 && validateUp())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("update Employee set Username=@username,Password=@password,Name=@name,Email=@email,Address=@address,DateOfBirth=@Dob,JobID=@job,Photo=@photo where ID = " + ID + "", Utility.conn);
                        cmd.Parameters.AddWithValue("@username", textBoxUsername.Text);
                        cmd.Parameters.AddWithValue("@password", Utility.EncPass(textBoxPassword.Text));
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@email", textBoxEmail.Text);
                        cmd.Parameters.AddWithValue("@address", richTextBoxAddress.Text);
                        cmd.Parameters.AddWithValue("@Dob", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@job", comboBoxJob.SelectedValue);
                        if (pictureBox1.Image == null)
                        {
                            cmd.Parameters.AddWithValue("@photo", null);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@photo", Utility.EncImage(pictureBox1.Image));
                        }
                        Utility.conn.Open();
                        cmd.ExecuteNonQuery();
                        Utility.conn.Close();

                        MessageBox.Show("Data Success Inserted", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        buttonCan.PerformClick();

                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0)
            {

            }
            else
            {
                dataGridView1.CurrentRow.Selected = true;

                ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                textBoxUsername.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBoxName.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBoxEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                richTextBoxAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
                comboBoxJob.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                comboBoxJob.SelectedValue = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[7].Value);

                if(dataGridView1.Rows[e.RowIndex].Cells[8].Value == DBNull.Value)
                {
                    pictureBox1.Image = null;
                }
                else
                {
                    MemoryStream stream = new MemoryStream((byte[])dataGridView1.Rows[e.RowIndex].Cells[8].Value);
                    Image img = Image.FromStream(stream);
                    pictureBox1.Image = img;
                }
            }
        }

        private void buttonBrow_Click(object sender, EventArgs e)
        {
            if (buttonBrow.Cursor != Cursors.No)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Images|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(ofd.FileName);
                    Bitmap bmp = (Bitmap)img;
                    pictureBox1.Image = bmp;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        private void buttonCan_Click(object sender, EventArgs e)
        {
            if(buttonCan.Cursor != Cursors.No)
            {
                disable();
                Clear();
                dataGridView1.Enabled = true;
                ID = 0;
                Condition = 0;
                Datagrid();
            }
        }
    }
}
