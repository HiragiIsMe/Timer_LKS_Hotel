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
    public partial class CheckOut : Form
    {
        private int IdReser;
        public CheckOut()
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
            loadItem();
            loadItemStatus();
            loadItemgrid();
            loadFD();
            labelPrice2.Text = countFD().ToString();
        }
        void loadItemgrid()
        {
            dataGridViewItem.ColumnCount = 6;
            dataGridViewItem.Columns[0].Visible = false;
            dataGridViewItem.Columns[1].Visible = false;
            dataGridViewItem.Columns[2].HeaderText = "Item";
            dataGridViewItem.Columns[3].HeaderText = "Quantity";
            dataGridViewItem.Columns[4].HeaderText = "Compentation Fee";
            dataGridViewItem.Columns[5].HeaderText = "Sub Total";
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
        void loadFD()
        {
            string com = "select FoodsAndDrinks.Name, FoodsAndDrinks.Type, FoodsAndDrinks.Price, FDCheckOut.* from FDCheckout join foodsandDrinks on FDCheckout.fdid = foodsanddrinks.id where reservationRoomId = " + IdReser + "";
            dataGridViewFD.DataSource = Utility.getData(com);
            dataGridViewFD.Columns[3].Visible = false;
            dataGridViewFD.Columns[4].Visible = false;
            dataGridViewFD.Columns[5].Visible = false;
            dataGridViewFD.Columns[8].Visible = false;
            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            {
                button.Name = "button";
                button.Text = "Remove";
                button.HeaderText = "Option";
                button.UseColumnTextForButtonValue = true;
                dataGridViewFD.Columns.Add(button);
            }

            dataGridViewFD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < dataGridViewFD.Rows.Count; i++)
            {
                if (dataGridViewFD.Rows[i].Cells[1].Value.ToString() == "1")
                {
                    dataGridViewFD.Rows[i].Cells[1].Value = "Food";
                }
                else
                {
                    dataGridViewFD.Rows[i].Cells[1].Value = "Drink";
                }
            }
            int a = 0;
            for (int i = 0; i < dataGridViewFD.Rows.Count; i++)
            {
                a += Convert.ToInt32(dataGridViewFD.Rows[i].Cells[7].Value);
            }

            labelPrice3.Text = a.ToString();
        }
        int countItem()
        {
            int a = 0;
            for (int i = 0; i < dataGridViewItem.Rows.Count; i++)
            {
                a += Convert.ToInt32(dataGridViewItem.Rows[i].Cells[5].Value);
            }

            return a;
        }
        int countFD()
        {
            int a = 0;
            for (int i = 0; i < dataGridViewFD.Rows.Count; i++)
            {
                a += Convert.ToInt32(dataGridViewFD.Rows[i].Cells[7].Value);
            }

            return a;
        }
        void loadItem()
        {
            string query = "select Item.ID, Item.Name from ReservationRequestItem join Item on ReservationRequestItem.ItemID = Item.ID where ReservationRoomID = " + IdReser + "";
            comboBoxItem.DataSource = Utility.getData(query);
            comboBoxItem.ValueMember = "ID";
            comboBoxItem.DisplayMember = "Name";
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
            SqlCommand cmd = new SqlCommand("select RequestPrice, CompensationFee from Item where ID=" + comboBoxItem.SelectedValue + "", Utility.conn);
            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            textBoxFee.Text = reader.GetInt32(1).ToString();
            textBoxSubtotal.Text = (reader.GetInt32(0) * numericUpDownQty.Value).ToString();
            Utility.conn.Close();
        }
        void loadItemStatus()
        {
            string query = "select * from ItemStatus";
            comboBoxStatus.DataSource = Utility.getData(query);
            comboBoxStatus.ValueMember = "ID";
            comboBoxStatus.DisplayMember = "Name";
        }
        private void CheckOut_Load(object sender, EventArgs e)
        {
            onload();
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

        private void comboBoxItem_SelectionChangeCommitted(object sender, EventArgs e)
        {
            getPrice();
        }

        private void numericUpDownQty_ValueChanged(object sender, EventArgs e)
        {
            getPrice();
        }
        bool CheckItem()
        {
            for (int i = 0; i < dataGridViewItem.Rows.Count; i++)
            {
                if (dataGridViewItem.Rows[i].Cells[0].Value.ToString() == comboBoxItem.SelectedValue.ToString())
                {
                    if (comboBoxStatus.Text == "GOOD")
                    {
                        int a = Convert.ToInt32(numericUpDownQty.Value);
                        dataGridViewItem.Rows[i].Cells[3].Value = a;
                        dataGridViewItem.Rows[i].Cells[5].Value = textBoxSubtotal.Text;
                    }
                    else
                    {
                        int a = Convert.ToInt32(numericUpDownQty.Value);
                        int b = Convert.ToInt32(textBoxFee.Text);
                        int c = Convert.ToInt32(textBoxSubtotal.Text);
                        int Sub = c + b;
                        dataGridViewItem.Rows[i].Cells[3].Value = a;
                        dataGridViewItem.Rows[i].Cells[5].Value = Sub;
                    }

                    return false;
                }
            }
            return true;
        }
        void TotalPrice()
        {
            int a = Convert.ToInt32(labelPrice1.Text);
            int b = Convert.ToInt32(labelPrice2.Text);

            int c = a + b;
            labelPrice3.Text = a.ToString();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (CheckItem())
            {
                if (comboBoxStatus.Text == "GOOD")
                {
                    string[] addgrid = { comboBoxItem.SelectedValue.ToString(), comboBoxStatus.SelectedValue.ToString(), comboBoxItem.Text, numericUpDownQty.Value.ToString(), textBoxFee.Text, textBoxSubtotal.Text };
                    dataGridViewItem.Rows.Add(addgrid);
                }
                else
                {
                    int a = Convert.ToInt32(textBoxSubtotal.Text);
                    int b = Convert.ToInt32(textBoxFee.Text);
                    int Sub = a + b;
                    string[] addgrid = { comboBoxItem.SelectedValue.ToString(), comboBoxStatus.SelectedValue.ToString(), comboBoxItem.Text, numericUpDownQty.Value.ToString(), textBoxFee.Text, Sub.ToString() };
                    dataGridViewItem.Rows.Add(addgrid);
                }
                numericUpDownQty.Value = 0;
                textBoxFee.Text = "";
                textBoxSubtotal.Text = "";

                labelPrice1.Text = countItem().ToString();
                labelPrice3.Text = (countItem() + countFD()).ToString();
            }
        }

        private void dataGridViewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                dataGridViewItem.CurrentRow.Selected = true;
                dataGridViewItem.Rows.RemoveAt(dataGridViewItem.SelectedRows[0].Index);
                countItem();
                TotalPrice();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxItem.SelectedValue.ToString() != "0")
            {
                try
                {
                    for (int i = 0; i < dataGridViewItem.Rows.Count; i++)
                    {
                        SqlCommand cmd = new SqlCommand("insert into ReservationCheckOut values(" + IdReser + ", " + dataGridViewItem.Rows[i].Cells[0].Value + ", " + dataGridViewItem.Rows[i].Cells[1].Value + ", " + dataGridViewItem.Rows[i].Cells[3].Value + ", " + dataGridViewItem.Rows[i].Cells[5].Value + ")", Utility.conn);
                        Utility.conn.Open();
                        cmd.ExecuteNonQuery();
                        Utility.conn.Close();

                        SqlCommand cmd1 = new SqlCommand("update Room set Status='1' where ID=" + comboBoxItem.SelectedValue + "", Utility.conn);
                        Utility.conn.Open();
                        cmd1.ExecuteNonQuery();
                        Utility.conn.Close();

                        SqlCommand cmd2 = new SqlCommand("update ReservationRoom set CheckoutDateTime=getdate() where ID=" + IdReser + "", Utility.conn);
                        Utility.conn.Open();
                        cmd2.ExecuteNonQuery();
                        Utility.conn.Close();

                        MessageBox.Show("CheckOut Success", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridViewItem.Rows.Clear();
                        dataGridViewFD.DataSource = null;
                        dataGridViewFD.Rows.Clear();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
