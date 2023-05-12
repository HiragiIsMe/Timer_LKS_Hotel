using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer_LKS_Hotel
{
    public partial class MainLogin : Form
    {
        public MainLogin()
        {
            InitializeComponent();
        }
        void onload()
        {
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.None;
            this.CenterToScreen();
            textBox2.UseSystemPasswordChar = true;
            textBox1.Focus();
        }
        private void MainLogin_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Close Application?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(result == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("All Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("select * from Employee where Username=@username and Password=@password", Utility.conn);
                cmd.Parameters.AddWithValue("@username", textBox1.Text);
                cmd.Parameters.AddWithValue("@password", Utility.EncPass(textBox2.Text));
                Utility.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                if (reader.HasRows)
                {
                    Model.id = Convert.ToInt32(reader["ID"]);
                    Model.Name = Convert.ToString(reader["Name"]);
                    Model.JobID = Convert.ToInt32(reader["JobID"]);

                    if(Model.JobID == 1)
                    {
                        Utility.conn.Close();
                        MainAdmin admin = new MainAdmin();
                        this.Hide();
                        admin.Show();
                    }

                    if(Model.JobID == 3)
                    {
                        Utility.conn.Close();
                        MainOffice office = new MainOffice();
                        this.Hide();
                        office.Show();
                    }
                }
                else
                {
                    Utility.conn.Close();
                    MessageBox.Show("User Not Found", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
