using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer_LKS_Hotel
{
    public partial class MainOffice : Form
    {
        public MainOffice()
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
            timer1.Start();
        }
        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Logout?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.Close();
                MainLogin login = new MainLogin();
                login.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Close Application?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Close Application?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            labelTime.Text = dt.ToString();
        }

        private void MainOffice_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void foodsAndDrinksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckIn form = new CheckIn()
            {
                TopLevel = false,
                TopMost = true,
            };
            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RequestAdditionalItem form = new RequestAdditionalItem()
            {
                TopLevel = false,
                TopMost = true,
            };
            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void roomTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckOut form = new CheckOut()
            {
                TopLevel = false,
                TopMost = true,
            };
            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportCheckIn form = new ReportCheckIn()
            {
                TopLevel = false,
                TopMost = true,
            };
            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void guestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportGuest form = new ReportGuest()
            {
                TopLevel = false,
                TopMost = true,
            };
            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void reservationCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reservation form = new Reservation()
            {
                TopLevel = false,
                TopMost = true,
            };
            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void reservationCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReservationCompany form = new ReservationCompany()
            {
                TopLevel = false,
                TopMost = true,
            };
            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }
    }
}
