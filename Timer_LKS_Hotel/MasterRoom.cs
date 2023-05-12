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
    public partial class MasterRoom : Form
    {
        private int Condition, ID;
        public MasterRoom()
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
            RoomType();
            disable();
        }
        void RoomType()
        {
            string query = "select * from RoomType";
            comboBoxType.DataSource = Utility.getData(query);
            comboBoxType.DisplayMember = "Name";
            comboBoxType.ValueMember = "ID";
        }
        void Datagrid()
        {
            string query = "select Room.ID, Room.RoomTypeID, Room.RoomNumber, RoomType.Name as 'RoomType', Room.RoomFloor, Room.Description from Room join RoomType on Room.RoomTypeID = RoomType.ID";
            dataGridView1.DataSource = Utility.getData(query);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        void enabled()
        {
            textBoxNumber.Enabled = true;
            comboBoxType.Enabled = true;
            textBoxFloor.Enabled = true;
            richTextBoxDesc.Enabled = true;
            buttonIn.Cursor = Cursors.No;
            buttonUp.Cursor = Cursors.No;
            buttonDel.Cursor = Cursors.No;
            buttonSav.Cursor = Cursors.Arrow;
            buttonCan.Cursor = Cursors.Arrow;
            buttonCan.Click += buttonCan_Click;
        }

        void disable()
        {
            textBoxNumber.Enabled = false;
            comboBoxType.Enabled = false;
            textBoxFloor.Enabled = false;
            richTextBoxDesc.Enabled = false;
            buttonIn.Cursor = Cursors.Arrow;
            buttonUp.Cursor = Cursors.Arrow;
            buttonDel.Cursor = Cursors.Arrow;
            buttonSav.Cursor = Cursors.No;
            buttonCan.Cursor = Cursors.No;
        }
        void Clear()
        {
            textBoxNumber.Text = "";
            textBoxFloor.Text = "";
            richTextBoxDesc.Text = "";
        }
        private void MasterRoom_Load(object sender, EventArgs e)
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
            if(buttonDel.Cursor != Cursors.No)
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
            if(textBoxNumber.Text == "" || textBoxFloor.Text == "" || richTextBoxDesc.Text == "" || comboBoxType.SelectedValue == null)
            {
                MessageBox.Show("All Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand cmd = new SqlCommand("select * from Room where RoomNumber=@number", Utility.conn);
            cmd.Parameters.AddWithValue("@number", textBoxNumber.Text);
            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                Utility.conn.Close();
                MessageBox.Show("Room Number Name Has Been Used", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Utility.conn.Close();

            return true;
        }
        private void buttonSav_Click(object sender, EventArgs e)
        {
            if(buttonSav.Cursor != Cursors.No)
            {
                if (Condition == 1 && validate())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("insert into Room values(@type,@number,@floor,@desc, '1')", Utility.conn);
                        cmd.Parameters.AddWithValue("@type", comboBoxType.SelectedValue);
                        cmd.Parameters.AddWithValue("@number", textBoxNumber.Text);
                        cmd.Parameters.AddWithValue("@floor", textBoxFloor.Text);
                        cmd.Parameters.AddWithValue("@desc", richTextBoxDesc.Text);

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
                        SqlCommand cmd = new SqlCommand("update Room set RoomTypeID=@type,RoomNumber=@number,RoomFloor=@floor,Description=@desc where ID = " + ID + "", Utility.conn);
                        cmd.Parameters.AddWithValue("@type", comboBoxType.SelectedValue);
                        cmd.Parameters.AddWithValue("@number", textBoxNumber.Text);
                        cmd.Parameters.AddWithValue("@floor", textBoxFloor.Text);
                        cmd.Parameters.AddWithValue("@desc", richTextBoxDesc.Text);

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {

            }
            else
            {
                dataGridView1.CurrentRow.Selected = true;

                ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                textBoxNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                comboBoxType.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                comboBoxType.SelectedValue = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                textBoxFloor.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                richTextBoxDesc.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        private void textBoxNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxFloor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
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
