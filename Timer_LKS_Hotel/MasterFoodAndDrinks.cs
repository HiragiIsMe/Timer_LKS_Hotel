using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer_LKS_Hotel
{
    public partial class MasterFoodAndDrinks : Form
    {
        private int Condition, ID;
        public MasterFoodAndDrinks()
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
            disable();
            loadtype();
        }
        void Datagrid()
        {
            string query = "select * from FoodsAndDrinks";
            dataGridView1.DataSource = Utility.getData(query);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if(dataGridView1.Rows[i].Cells[2].Value.ToString() == "f")
                {
                    dataGridView1.Rows[i].Cells[2].Value = "Food";
                }
                else
                {
                    dataGridView1.Rows[i].Cells[2].Value = "Drink";
                }
            }
        }
        void enabled()
        {
            textBoxName.Enabled = true;
            comboBox1.Enabled = true;
            textBoxPrice.Enabled = true;
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
            comboBox1.Enabled = false;
            textBoxPrice.Enabled = false;
            buttonBrow.Cursor = Cursors.No;
            buttonIn.Cursor = Cursors.Arrow;
            buttonUp.Cursor = Cursors.Arrow;
            buttonDel.Cursor = Cursors.Arrow;
            buttonSav.Cursor = Cursors.No;
            buttonCan.Cursor = Cursors.No;
        }
        void loadtype()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("f", "Food");
            data.Add("d", "Drink");

            comboBox1.DataSource = new BindingSource(data, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }
        void Clear()
        {
            textBoxName.Text = "";
            textBoxPrice.Text = "";
            pictureBox1.Image = null;
        }
        private void MasterFoodAndDrinks_Load(object sender, EventArgs e)
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
        bool validate()
        {
            if(textBoxName.Text == "" || comboBox1.SelectedValue == null || textBoxPrice.Text == "" || pictureBox1.Image == null)
            {
                MessageBox.Show("All Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
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
                    DialogResult result = MessageBox.Show("Are You Sure To Delete " + dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + " ?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        SqlCommand cmd = new SqlCommand("delete from FoodsAndDrinks where ID=" + ID + "", Utility.conn);
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

        private void buttonSav_Click(object sender, EventArgs e)
        {
            if (buttonSav.Cursor != Cursors.No)
            {
                if (Condition == 1 && validate())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("insert into FoodsAndDrinks values(@name,@type,@price,@image)", Utility.conn);
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@type", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@price", textBoxPrice.Text);
                        cmd.Parameters.AddWithValue("@image", Utility.EncImage(pictureBox1.Image));

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
                if (Condition == 2 && validate())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("update  FoodsAndDrinks set Name=@name,Type=@type,Price=@price,Photo=@image where ID = " + ID + "", Utility.conn);
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@type", comboBox1.SelectedValue);
                        cmd.Parameters.AddWithValue("@price", textBoxPrice.Text);
                        cmd.Parameters.AddWithValue("@image", Utility.EncImage(pictureBox1.Image));

                        Utility.conn.Open();
                        cmd.ExecuteNonQuery();
                        Utility.conn.Close();

                        MessageBox.Show("Data Success Updated", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        buttonCan.PerformClick();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0)
            {

            }
            else
            {
                dataGridView1.CurrentRow.Selected = true;

                ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                textBoxName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                if(comboBox1.Text == "Food")
                {
                    comboBox1.SelectedValue = "f";
                }
                else
                {
                    comboBox1.SelectedValue = "d";
                }

                textBoxPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                if (dataGridView1.Rows[e.RowIndex].Cells[4].Value == DBNull.Value)
                {
                    pictureBox1.Image = null;
                }
                else
                {
                    MemoryStream stream = new MemoryStream((byte[])dataGridView1.Rows[e.RowIndex].Cells[4].Value);
                    Image img = Image.FromStream(stream);
                    pictureBox1.Image = img;
                }

            }
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
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
    }
}
