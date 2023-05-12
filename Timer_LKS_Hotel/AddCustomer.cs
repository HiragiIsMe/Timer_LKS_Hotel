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
    public partial class AddCustomer : Form
    {
        public AddCustomer()
        {
            InitializeComponent();
        }
        void onload()
        {
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.None;
            loadGender();
        }
        private void AddCustomer_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void textBoxPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxNIK_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        void loadGender()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("L", "Male");
            data.Add("P", "Female");

            comboBoxGender.DataSource = new BindingSource(data, null);
            comboBoxGender.ValueMember = "Key";
            comboBoxGender.DisplayMember = "Value";
        }
    }
}
