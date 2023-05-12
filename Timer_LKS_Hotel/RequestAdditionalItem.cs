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
    public partial class RequestAdditionalItem : Form
    {
        private int IdReser;
        public RequestAdditionalItem()
        {
            InitializeComponent();
        }
        void onload()
        {
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.None;
            loadRoom();
            ItemGrid();
            loadItem();
        }
        void loadItem()
        {
            string query = "select * from Item";
            comboBoxItem.DataSource = Utility.getData(query);
            comboBoxItem.ValueMember = "ID";
            comboBoxItem.DisplayMember = "Name";
        }
        void ItemGrid()
        {
            dataGridViewItem.ColumnCount = 5;
            dataGridViewItem.Columns[0].Visible = false;
            dataGridViewItem.Columns[1].HeaderText = "Item";
            dataGridViewItem.Columns[2].HeaderText = "Quantity";
            dataGridViewItem.Columns[3].HeaderText = "Price";
            dataGridViewItem.Columns[4].HeaderText = "Subtotal";
            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            {
                button.Name = "button";
                button.Text = "Remove";
                button.HeaderText = "Option";
                button.UseColumnTextForButtonValue = true;
                dataGridViewItem.Columns.Add(button);
            }
            dataGridViewItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        void ClearItem()
        {
            numericUpDownItemQty.Value = 1;
            textBoxItemPrice.Text = "";
            textBoxItemSub.Text = "";
        }
        void loadRoom()
        {
            string query = "select * from Room where Status = '0'";

            comboBoxRoom.DataSource = Utility.getData(query);
            comboBoxRoom.DisplayMember = "RoomNumber";
            comboBoxRoom.ValueMember = "ID";

            if(comboBoxRoom.Items.Count != 0)
            {
                SqlCommand cmd = new SqlCommand("select top(1) id from reservationRoom where roomId = " + comboBoxRoom.SelectedValue + " order by id desc", Utility.conn);
                Utility.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    IdReser = reader.GetInt32(0);
                }
                Utility.conn.Close();
            }
        }
        void getPrice()
        {
            SqlCommand cmd = new SqlCommand("select RequestPrice from Item where ID=" + comboBoxItem.SelectedValue + "", Utility.conn);
            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            textBoxItemPrice.Text = reader["RequestPrice"].ToString();
            textBoxItemSub.Text = (reader.GetInt32(0) * numericUpDownItemQty.Value).ToString();
            Utility.conn.Close();
        }
        private void RequestAdditionalItem_Load(object sender, EventArgs e)
        {
            onload();
        }
        bool CheckItem()
        {
            for (int i = 0; i < dataGridViewItem.Rows.Count; i++)
            {
                if (dataGridViewItem.Rows[i].Cells[0].Value.ToString() == comboBoxItem.SelectedValue.ToString())
                {
                    int a = Convert.ToInt32(numericUpDownItemQty.Value);
                    dataGridViewItem.Rows[i].Cells[2].Value = a;
                    dataGridViewItem.Rows[i].Cells[4].Value = textBoxItemSub.Text;
                    ClearItem();

                    return false;
                }
            }
            return true;
        }
        void getTotal()
        {
            int item = 0;
            for (int x = 0; x < dataGridViewItem.Rows.Count; x++)
            {
                item += Convert.ToInt32(dataGridViewItem.Rows[x].Cells[4].Value);
            }
            labelPrice.Text = item.ToString();
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (numericUpDownItemQty.Value == null || textBoxItemPrice.Text == "" || textBoxItemSub.Text == "")
            {
                MessageBox.Show("All Field Item Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (CheckItem())
                {
                    string[] add = { comboBoxItem.SelectedValue.ToString(), comboBoxItem.Text, numericUpDownItemQty.Value.ToString(), textBoxItemPrice.Text, textBoxItemSub.Text };
                    dataGridViewItem.Rows.Add(add);
                    getTotal();
                    ClearItem();
                }
            }
        }

        private void comboBoxItem_SelectionChangeCommitted(object sender, EventArgs e)
        {
            getPrice();
        }

        private void numericUpDownItemQty_ValueChanged(object sender, EventArgs e)
        {
            getPrice();
        }

        private void dataGridViewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                dataGridViewItem.CurrentRow.Selected = true;
                dataGridViewItem.Rows.RemoveAt(dataGridViewItem.SelectedRows[0].Index);
                getTotal();
            }
        }

        private void comboBoxRoom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select top(1) id from reservationRoom where roomId = " + comboBoxRoom.SelectedValue + " order by id desc", Utility.conn);
            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            IdReser = reader.GetInt32(0);
            Utility.conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewItem.Rows.Count != 0)
            {
                for (int j = 0; j < dataGridViewItem.Rows.Count; j++)
                {
                    SqlCommand inItem = new SqlCommand("insert into ReservationRequestItem values(" + IdReser + ", " + Convert.ToInt32(dataGridViewItem.Rows[j].Cells[0].Value) + ", " + Convert.ToInt32(dataGridViewItem.Rows[j].Cells[2].Value) + ", " + Convert.ToInt32(dataGridViewItem.Rows[j].Cells[4].Value) + ")", Utility.conn);
                    Utility.conn.Open();
                    inItem.ExecuteNonQuery();
                    Utility.conn.Close();
                }

                MessageBox.Show("Request Item Success Added", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridViewItem.Rows.Clear();
            }
            else
            {
                MessageBox.Show("Please Fill At Least One Request Item", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
