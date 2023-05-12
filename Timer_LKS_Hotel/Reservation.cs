using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Timer_LKS_Hotel
{
    public partial class Reservation : Form
    {
        int id_cust;
        bool IsSelectedAvd, IsSelectedSel;
        AddCustomer add = new AddCustomer()
        {
            TopLevel = false,
            TopMost = true
        };
        public Reservation()
        {
            InitializeComponent();
        }
        void onload()
        {
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.None;
            radioButton2.Checked = true;
            loadCustomer();
            loadRoomType();
            loadgridSel();
            loadItem();
            gridItem();
        }
        void loadItem()
        {
            string query = "select * from Item";
            comboBoxItem.DataSource = Utility.getData(query);

            comboBoxItem.DisplayMember = "Name";
            comboBoxItem.ValueMember = "ID";
        }
        void loadRoomType()
        {
            string query = "select * from RoomType";
            comboBoxType.DataSource = Utility.getData(query);

            comboBoxType.DisplayMember = "Name";
            comboBoxType.ValueMember = "ID";
        }
        void loadgridSel()
        {
            dataGridViewSel.ColumnCount = 5;
            dataGridViewSel.Columns[0].Visible = false;
            dataGridViewSel.Columns[1].HeaderText = "RoomNumber";
            dataGridViewSel.Columns[2].HeaderText = "RoomFloor";
            dataGridViewSel.Columns[3].HeaderText = "RoomPrice";
            dataGridViewSel.Columns[4].HeaderText = "Description";
        }
        private void Reservation_Load(object sender, EventArgs e)
        {
            onload();
        }
        void loadCustomer()
        {
            string query = "select * from Customer";
            dataGridViewCustomer.DataSource = Utility.getData(query);
            dataGridViewCustomer.Columns[0].Visible = false;
            dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewCustomer.Hide();
            label2.Hide();
            textBox1.Hide();

            panelMain.Controls.Clear();
            panelMain.Controls.Add(add);
            add.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            label2.Show();
            textBox1.Show();
            panelMain.Controls.Add(dataGridViewCustomer);
            dataGridViewCustomer.Show();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string query = "select Room.ID, Room.RoomNumber, Room.RoomFloor, RoomType.RoomPrice, Room.Description from Room join RoomType on Room.RoomTypeID = RoomType.ID where Room.RoomTypeID = " + comboBoxType.SelectedValue + " and Room.Status = '1'";
            dataGridViewAvd.DataSource = Utility.getData(query);
            dataGridViewAvd.Columns[0].Visible = false;
            dataGridViewAvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        bool CheckRoom()
        {
            for(int i = 0; i < dataGridViewSel.Rows.Count; i++)
            {
                if(dataGridViewSel.Rows[i].Cells[0].Value.ToString() == dataGridViewAvd.SelectedRows[0].Cells[0].Value.ToString())
                {
                    return false;
                }
            }
            return true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (IsSelectedAvd)
            {
                if (CheckRoom())
                {
                    string[] add = { dataGridViewAvd.SelectedRows[0].Cells[0].Value.ToString(), dataGridViewAvd.SelectedRows[0].Cells[1].Value.ToString(), dataGridViewAvd.SelectedRows[0].Cells[2].Value.ToString(), dataGridViewAvd.SelectedRows[0].Cells[3].Value.ToString(), dataGridViewAvd.SelectedRows[0].Cells[4].Value.ToString() };
                    dataGridViewSel.Rows.Add(add);
                }
                else
                {
                    MessageBox.Show("Room Has Been Added", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            getTotal();
        }

        private void dataGridViewAvd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0)
            {

            }
            else
            {
                dataGridViewAvd.CurrentRow.Selected = true;
                IsSelectedAvd = true;
            }
        }

        private void dataGridViewCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {

            }
            else
            {
                dataGridViewCustomer.CurrentRow.Selected = true;
                id_cust = Convert.ToInt32(dataGridViewCustomer.Rows[e.RowIndex].Cells[0].Value);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IsSelectedSel)
            {
                dataGridViewSel.Rows.RemoveAt(dataGridViewSel.SelectedRows[0].Index);
            }
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select RequestPrice from Item where ID=" + comboBoxItem.SelectedValue + "", Utility.conn);
            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            textBoxItemPrice.Text = reader.GetInt32(0).ToString();
            Utility.conn.Close();
        }

        private void numericUpDownItemQty_ValueChanged(object sender, EventArgs e)
        {
            if(textBoxItemPrice.Text == "")
            {
                textBoxItemSub.Text = "";
            }
            else
            {
                int a = Convert.ToInt32(numericUpDownItemQty.Value);
                int b = Convert.ToInt32(textBoxItemPrice.Text);

                textBoxItemSub.Text = (a * b).ToString();
            }
        }

        private void comboBoxItem_SelectionChangeCommitted(object sender, EventArgs e)
        {
            textBoxItemPrice.Text = Utility.getNumData("select RequestPrice from Item where ID=" + comboBoxItem.SelectedValue + "").ToString();
        }
        bool checkItem()
        {
            for(int i = 0; i < dataGridViewItem.Rows.Count; i++)
            {
                if(dataGridViewItem.Rows[i].Cells[0].Value == comboBoxItem.SelectedValue)
                {
                    dataGridViewItem.Rows[i].Cells[2].Value = numericUpDownItemQty.Value.ToString();
                    dataGridViewItem.Rows[i].Cells[3].Value = textBoxItemPrice.Text;
                    dataGridViewItem.Rows[i].Cells[4].Value = textBoxItemSub.Text;
                    return false;
                }
            }

            return true;
        }
        void getTotal()
        {
            int room = 0;
            int item = 0;
            int total = 0;
            for (int i = 0; i < dataGridViewSel.Rows.Count; i++)
            {
                room += Convert.ToInt32(dataGridViewSel.Rows[i].Cells[3].Value);
            }

            for (int x = 0; x < dataGridViewItem.Rows.Count; x++)
            {
                item += Convert.ToInt32(dataGridViewItem.Rows[x].Cells[4].Value);
            }
            total = room + item;
            label16.Text = total.ToString();
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (checkItem())
            {
                string[] add = { comboBoxItem.SelectedValue.ToString(), comboBoxItem.Text, numericUpDownItemQty.Value.ToString(), textBoxItemPrice.Text, textBoxItemSub.Text };
                dataGridViewItem.Rows.Add(add);
            }
            getTotal();
        }
        void gridItem()
        {
            dataGridViewItem.ColumnCount = 5;
            dataGridViewItem.Columns[0].Visible = false;
            dataGridViewItem.Columns[1].HeaderText = "Item";
            dataGridViewItem.Columns[2].HeaderText = "Quantity";
            dataGridViewItem.Columns[3].HeaderText = "Price";
            dataGridViewItem.Columns[4].HeaderText = "SubTotal";

            DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            {
                button.Text = "Remove";
                button.HeaderText = "Action";
                button.Name = "Remove";
                button.UseColumnTextForButtonValue = true;
                dataGridViewItem.Columns.Add(button);
            }
        }

        private void dataGridViewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 5)
            {
                dataGridViewItem.CurrentRow.Selected = true;
                dataGridViewItem.Rows.RemoveAt(dataGridViewItem.SelectedRows[0].Index);
            }
        }
        bool Validate()
        {
            if (dateTimePickerIn.Value == null || dateTimePickerOut.Value == null || textBoxStaying.Text == "")
            {
                MessageBox.Show("Reservation's Information Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (radioButton1.Checked)
            {
                if (add.textBoxName.Text == "" || add.textBoxNIK.Text == "" || add.textBoxEmail.Text == "" || add.textBoxPhoneNumber.Text == "")
                {
                    MessageBox.Show("Customer Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
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
                    }
                    catch
                    {
                        return false;
                    }
                }
                if (isValidEmail(add.textBoxEmail.Text) == false)
                {
                    MessageBox.Show("Customer Email Doesn't Valid", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (add.textBoxNIK.TextLength != 16)
                {
                    MessageBox.Show("Customer NIK Must Be 16 Digit", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (add.dateTimePickerDob.Value > DateTime.Now)
                {
                    MessageBox.Show("Customer Birth Is Not Valid", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            if (radioButton2.Checked)
            {
                if (dataGridViewCustomer.CurrentRow.Selected == false)
                {
                    MessageBox.Show("Please Select One Customer", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            if (dataGridViewSel.Rows.Count == 0)
            {
                MessageBox.Show("Please Select At Least One Room", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        string getCode()
        {
            string code;

            SqlCommand cmd = new SqlCommand("select top(1) BookingCode from Reservation order by ID desc", Utility.conn);
            Utility.conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                string getCode = reader.GetString(0);
                Utility.conn.Close();

                int a = 2;
                int b = getCode.Length - a;
                int c = Convert.ToInt32(getCode.Substring(a, b)) + 1;
                code = "BK" + c;

                return code;
            }
            else
            {

                Utility.conn.Close();
                code = "BK1";

                return code;
            }
            Utility.conn.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                if (radioButton2.Checked)
                {
                    SqlCommand inResr = new SqlCommand("insert into Reservation values(getdate(), " + Model.id + ", " + id_cust + ", '" + getCode() + "', NULL)", Utility.conn);
                    Utility.conn.Open();
                    inResr.ExecuteNonQuery();
                    Utility.conn.Close();
                }

                if (radioButton1.Checked)
                {
                    SqlCommand cmd = new SqlCommand("insert into Customer values(@name,@nik,@email,@gender,@phone,@dob)", Utility.conn);
                    cmd.Parameters.AddWithValue("@name", add.textBoxName.Text);
                    cmd.Parameters.AddWithValue("@nik", add.textBoxNIK.Text);
                    cmd.Parameters.AddWithValue("@email", add.textBoxEmail.Text);
                    cmd.Parameters.AddWithValue("@gender", add.comboBoxGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@phone", add.textBoxPhoneNumber.Text);
                    int i = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(add.dateTimePickerDob.Value.ToString("yyyy"));
                    cmd.Parameters.AddWithValue("@dob", i);

                    Utility.conn.Open();
                    cmd.ExecuteNonQuery();
                    Utility.conn.Close();

                    SqlCommand getCust = new SqlCommand("select top(1) id from Customer order by id desc", Utility.conn);
                    Utility.conn.Open();
                    SqlDataReader reader = getCust.ExecuteReader();
                    reader.Read();
                    int idCust = reader.GetInt32(0);
                    Utility.conn.Close();

                    SqlCommand inResr = new SqlCommand("insert into Reservation values(getdate(), " + Model.id + ", " + idCust + ", '" + getCode() + "', NULL)", Utility.conn);
                    Utility.conn.Open();
                    inResr.ExecuteNonQuery();
                    Utility.conn.Close();
                }

                SqlCommand getResrId = new SqlCommand("select top(1) id from Reservation order by id desc", Utility.conn);
                Utility.conn.Open();
                SqlDataReader read = getResrId.ExecuteReader();
                read.Read();
                int idRest = read.GetInt32(0);
                Utility.conn.Close();

                label1.Text = idRest.ToString();
                for (int i = 0; i < dataGridViewSel.Rows.Count; i++)
                {
                    SqlCommand inResrRoom = new SqlCommand("insert into ReservationRoom values(" + idRest + ", " + Convert.ToInt32(dataGridViewSel.Rows[i].Cells[0].Value) + ", @start, @night, " + Convert.ToInt32(dataGridViewSel.Rows[i].Cells[3].Value) + ", @in, @out)", Utility.conn);
                    inResrRoom.Parameters.AddWithValue("@night", textBoxStaying.Text);
                    inResrRoom.Parameters.AddWithValue("@start", dateTimePickerIn.Value.Date);
                    inResrRoom.Parameters.AddWithValue("@in", dateTimePickerIn.Value);
                    inResrRoom.Parameters.AddWithValue("@out", dateTimePickerOut.Value);
                    Utility.conn.Open();
                    inResrRoom.ExecuteNonQuery();
                    Utility.conn.Close();

                    SqlCommand upStatus = new SqlCommand("update Room set status='0' where ID=" + Convert.ToInt32(dataGridViewSel.Rows[i].Cells[0].Value) + "", Utility.conn);
                    Utility.conn.Open();
                    upStatus.ExecuteNonQuery();
                    Utility.conn.Close();
                }

                if (dataGridViewItem.Rows.Count > 0)
                {
                    SqlCommand getRoomID = new SqlCommand("select top(1) id from ReservationRoom order by id desc", Utility.conn);
                    Utility.conn.Open();
                    SqlDataReader reader = getRoomID.ExecuteReader();
                    reader.Read();
                    int RoomID = reader.GetInt32(0);
                    Utility.conn.Close();

                    for (int j = 0; j < dataGridViewItem.Rows.Count; j++)
                    {
                        SqlCommand inItem = new SqlCommand("insert into ReservationRequestItem values(" + RoomID + ", " + Convert.ToInt32(dataGridViewItem.Rows[j].Cells[0].Value) + ", " + Convert.ToInt32(dataGridViewItem.Rows[j].Cells[2].Value) + ", " + Convert.ToInt32(dataGridViewItem.Rows[j].Cells[4].Value) + ")", Utility.conn);
                        Utility.conn.Open();
                        inItem.ExecuteNonQuery();
                        Utility.conn.Close();
                    }
                }

                MessageBox.Show("Reservation Success", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                id_cust = 0;
                radioButton2.Checked = true;
                textBoxStaying.Text = "";
                add.textBoxName.Text = "";
                add.textBoxEmail.Text = "";
                add.textBoxNIK.Text = "";
                add.textBoxPhoneNumber.Text = "";
                dataGridViewAvd.DataSource = null;
                dataGridViewAvd.Rows.Clear();
                dataGridViewSel.Rows.Clear();
                dataGridViewItem.Rows.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string query = "select * from Customer where name like'%"+ textBox1.Text +"%'";
            dataGridViewCustomer.DataSource = Utility.getData(query);
            dataGridViewCustomer.Columns[0].Visible = false;
            dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dataGridViewSel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {

            }
            else
            {
                dataGridViewSel.CurrentRow.Selected = true;
                IsSelectedSel = true;
            }
        }
    }
}
