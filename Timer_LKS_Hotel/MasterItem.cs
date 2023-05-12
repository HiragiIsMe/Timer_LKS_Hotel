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
    public partial class MasterItem : Form
    {
        private int Condition, ID;
        public MasterItem()
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
        }
        void Datagrid()
        {
            string query = "select * from Item";
            dataGridView1.DataSource = Utility.getData(query);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        void enabled()
        {
            textBoxName.Enabled = true;
            textBoxReq.Enabled = true;
            textBoxFee.Enabled = true;
            buttonIn.Cursor = Cursors.No;
            buttonUp.Cursor = Cursors.No;
            buttonDel.Cursor = Cursors.No;
            buttonSav.Cursor = Cursors.Arrow;
            buttonCan.Cursor = Cursors.Arrow;
        }

        void disable()
        {
            textBoxName.Enabled = false;
            textBoxReq.Enabled = false;
            textBoxFee.Enabled = false;
            buttonIn.Cursor = Cursors.Arrow;
            buttonUp.Cursor = Cursors.Arrow;
            buttonDel.Cursor = Cursors.Arrow;
            buttonSav.Cursor = Cursors.No;
            buttonCan.Cursor = Cursors.No;
        }
        void Clear()
        {
            textBoxName.Text = "";
            textBoxReq.Text = "";
            textBoxFee.Text = "";
        }
        private void MasterItem_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void buttonIn_Click(object sender, EventArgs e)
        {
           if (buttonIn.Cursor != Cursors.No)
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
                    DialogResult result = MessageBox.Show("Are You Sure To Delete " + dataGridView1.SelectedRows[0].Cells[2].Value.ToString() + " ?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        SqlCommand cmd = new SqlCommand("delete from Room where ID=" + ID + "", Utility.conn);
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
        bool validate()
        {
            if(textBoxName.Text == "" || textBoxReq.Text == "" || textBoxFee.Text == "")
            {
                MessageBox.Show("All Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private void buttonSav_Click(object sender, EventArgs e)
        {
            if (buttonSav.Cursor != Cursors.No)
            {
                if (Condition == 1 && validate())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("insert into Item values(@name,@req,@fee)", Utility.conn);
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@req", textBoxReq.Text);
                        cmd.Parameters.AddWithValue("@fee", textBoxFee.Text);

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
                        SqlCommand cmd = new SqlCommand("update Item set Name=@name,RequestPrice=@req,CompensationFee=@fee where ID = "+ ID +"", Utility.conn);
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                        cmd.Parameters.AddWithValue("@req", textBoxReq.Text);
                        cmd.Parameters.AddWithValue("@fee", textBoxFee.Text);

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
        private void textBoxFee_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
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
                textBoxReq.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBoxFee.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }

        private void textBoxReq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
