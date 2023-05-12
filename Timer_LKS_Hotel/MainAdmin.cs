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
    public partial class MainAdmin : Form
    {
        public MainAdmin()
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

        private void foodsAndDrinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterEmployee form = new MasterEmployee()
            {
                TopLevel = false,
                TopMost = true
            };

            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void foodsAndDrinksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MasterFoodAndDrinks form = new MasterFoodAndDrinks()
            {
                TopLevel = false,
                TopMost = true
            };

            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterItem form = new MasterItem()
            {
                TopLevel = false,
                TopMost = true
            };

            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void roomTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterRoomType form = new MasterRoomType()
            {
                TopLevel = false,
                TopMost = true
            };

            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void roomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterRoom form = new MasterRoom()
            {
                TopLevel = false,
                TopMost = true
            };

            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Close Application?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void MainAdmin_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            labelTime.Text = dt.ToString();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Close Application?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}
