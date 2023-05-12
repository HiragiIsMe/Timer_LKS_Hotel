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
    public partial class ReservationCompany : Form
    {
        int id_cust;
        bool IsSelectedAvd, IsSelectedSel;
        AddCompany add = new AddCompany()
        {
            TopLevel = false,
            TopMost = true,
        };
        public ReservationCompany()
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
            loadRoomType();
            loadgridSel();
            loadCompany();
        }
        void loadCompany()
        {
            string query = "select * from Company";
            dataGridViewCustomer.DataSource = Utility.getData(query);
            dataGridViewCustomer.Columns[0].Visible = false;
            dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        void loadgridSel()
        {
            dataGridViewSel.ColumnCount = 8;
            dataGridViewSel.Columns[0].Visible = false;
            dataGridViewSel.Columns[1].HeaderText = "RoomNumber";
            dataGridViewSel.Columns[2].HeaderText = "RoomFloor";
            dataGridViewSel.Columns[3].HeaderText = "RoomPrice";
            dataGridViewSel.Columns[4].HeaderText = "Description";
            dataGridViewSel.Columns[5].Visible = false;
            dataGridViewSel.Columns[6].Visible = false;
            dataGridViewSel.Columns[7].Visible = false;
        }
        void loadRoomType()
        {
            string query = "select * from RoomType";
            comboBoxType.DataSource = Utility.getData(query);

            comboBoxType.DisplayMember = "Name";
            comboBoxType.ValueMember = "ID";
        }
        private void ReservationCompany_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();

            label2.Show();
            textBox1.Show();
            panelMain.Controls.Add(dataGridViewCustomer);
            dataGridViewCustomer.Show();
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

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string query = "select Room.ID, Room.RoomNumber, Room.RoomFloor, RoomType.RoomPrice, Room.Description from Room join RoomType on Room.RoomTypeID = RoomType.ID where Room.RoomTypeID = " + comboBoxType.SelectedValue + " and Room.Status = '1'";
            dataGridViewAvd.DataSource = Utility.getData(query);
            dataGridViewAvd.Columns[0].Visible = false;
            dataGridViewAvd.Columns[5].Visible = false;
            dataGridViewAvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        void getTotal()
        {
            int room = 0;
            for (int i = 0; i < dataGridViewSel.Rows.Count; i++)
            {
                room += Convert.ToInt32(dataGridViewSel.Rows[i].Cells[3].Value);
            }

            label16.Text = room.ToString();
        }
        bool CheckRoom()
        {
            for (int i = 0; i < dataGridViewSel.Rows.Count; i++)
            {
                if (dataGridViewSel.Rows[i].Cells[0].Value.ToString() == dataGridViewAvd.SelectedRows[0].Cells[0].Value.ToString())
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
                    string[] add = { dataGridViewAvd.SelectedRows[0].Cells[0].Value.ToString(), dataGridViewAvd.SelectedRows[0].Cells[1].Value.ToString(), dataGridViewAvd.SelectedRows[0].Cells[2].Value.ToString(), dataGridViewAvd.SelectedRows[0].Cells[3].Value.ToString(), dataGridViewAvd.SelectedRows[0].Cells[4].Value.ToString(), dateTimePickerIn.Value.ToString("yyyy-MM-dd HH:mm:ss"), textBoxStaying.Text, dateTimePickerOut.Value.Date.ToString("yyyy-MM-dd HH:mm:ss") };
                    dataGridViewSel.Rows.Add(add);
                }
                else
                {
                    MessageBox.Show("Room Has Been Added", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            getTotal();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IsSelectedSel)
            {
                dataGridViewSel.Rows.RemoveAt(dataGridViewSel.SelectedRows[0].Index);
            }
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

        private void dataGridViewAvd_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {

            }
            else
            {
                dataGridViewAvd.CurrentRow.Selected = true;
                IsSelectedAvd = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string query = "select * from Company where CompanyName like'%" + textBox1.Text + "%'";
            dataGridViewCustomer.DataSource = Utility.getData(query);
            dataGridViewCustomer.Columns[0].Visible = false;
            dataGridViewCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            if (validate())
            {
                if (radioButton2.Checked)
                {
                    SqlCommand inResr = new SqlCommand("insert into Reservation values(getdate(), " + Model.id + ", NULL, '" + getCode() + "', " + id_cust + ")", Utility.conn);
                    Utility.conn.Open();
                    inResr.ExecuteNonQuery();
                    Utility.conn.Close();
                }

                if (radioButton1.Checked)
                {
                    SqlCommand cmd = new SqlCommand("insert into Company values(@company,@name,@phone)", Utility.conn);
                    cmd.Parameters.AddWithValue("@company", add.textBoxName.Text);
                    cmd.Parameters.AddWithValue("@name", add.textBoxLeader.Text);
                    cmd.Parameters.AddWithValue("@phone", add.textBoxPhoneNumber.Text);

                    Utility.conn.Open();
                    cmd.ExecuteNonQuery();
                    Utility.conn.Close();

                    SqlCommand getCust = new SqlCommand("select top(1) id from Customer order by id desc", Utility.conn);
                    Utility.conn.Open();
                    SqlDataReader reader = getCust.ExecuteReader();
                    reader.Read();
                    int idCust = reader.GetInt32(0);
                    Utility.conn.Close();

                    SqlCommand inResr = new SqlCommand("insert into Reservation values(getdate(), " + Model.id + ", NULL, '" + getCode() + "', " + idCust + ")", Utility.conn);
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
                    inResrRoom.Parameters.AddWithValue("@night", dataGridViewSel.Rows[i].Cells[6].Value);
                    inResrRoom.Parameters.AddWithValue("@start", dataGridViewSel.Rows[i].Cells[5].Value);
                    inResrRoom.Parameters.AddWithValue("@in", dataGridViewSel.Rows[i].Cells[5].Value);
                    inResrRoom.Parameters.AddWithValue("@out", dataGridViewSel.Rows[i].Cells[7].Value);
                    Utility.conn.Open();
                    inResrRoom.ExecuteNonQuery();
                    Utility.conn.Close();

                    SqlCommand upStatus = new SqlCommand("update Room set status='0' where ID=" + Convert.ToInt32(dataGridViewSel.Rows[i].Cells[0].Value) + "", Utility.conn);
                    Utility.conn.Open();
                    upStatus.ExecuteNonQuery();
                    Utility.conn.Close();
                }

                MessageBox.Show("Reservation Success", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                id_cust = 0;
                radioButton2.Checked = true;
                textBoxStaying.Text = "";
                add.textBoxName.Text = "";
                add.textBoxLeader.Text = "";
                add.textBoxPhoneNumber.Text = "";
                dataGridViewAvd.DataSource = null;
                dataGridViewAvd.Rows.Clear();
                dataGridViewSel.Rows.Clear();
            }
        }
        bool validate()
        {
            if (dateTimePickerIn.Value == null || dateTimePickerOut.Value == null || textBoxStaying.Text == "")
            {
                MessageBox.Show("Reservation's Information Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (radioButton1.Checked)
            {
                if (add.textBoxName.Text == "" || add.textBoxLeader.Text == "" || add.textBoxPhoneNumber.Text == "")
                {
                    MessageBox.Show("Customer Field Must Be Filled", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void dataGridViewCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
    }
}
