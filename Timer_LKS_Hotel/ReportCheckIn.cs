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
    public partial class ReportCheckIn : Form
    {
        Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
        public ReportCheckIn()
        {
            InitializeComponent();
        }
        void onload()
        {
            ControlBox = false;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.None;
            radioButton1.Checked = true;
            dateTimePicker1.Value = DateTime.Now;
        }
        private void ReportCheckIn_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string query = "";
            if (radioButton1.Checked)
            {
                query = "select Reservation.BookingCode, ReservationRoom.RoomPrice, ReservationRoom.CheckInDateTime, ReservationRoom.DurationNights, ReservationRoom.CheckOutDateTime, Room.RoomNumber, Room.RoomFloor, Room.Description from Reservation join ReservationRoom on Reservation.ID = ReservationRoom.ReservationID join Room on ReservationRoom.RoomID = Room.ID where CONVERT(date, CheckInDateTime) = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            }
            else
            {
                if (dateTimePicker2.Value > dateTimePicker3.Value)
                {
                    MessageBox.Show("Invalid Date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    query = "select Reservation.BookingCode, ReservationRoom.RoomPrice, ReservationRoom.CheckInDateTime, ReservationRoom.DurationNights, ReservationRoom.CheckOutDateTime, Room.RoomNumber, Room.RoomFloor, Room.Description from Reservation join ReservationRoom on Reservation.ID = ReservationRoom.ReservationID join Room on ReservationRoom.RoomID = Room.ID where CONVERT(date, CheckInDateTime) between '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "'";
                }
            }

            dataGridView1.DataSource = Utility.getData(query);
            button1.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                excel.Workbooks.Add(Type.Missing);

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    excel.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int x = 0; x < dataGridView1.Columns.Count; x++)
                    {
                        excel.Cells[i + 2, x + 1] = dataGridView1.Rows[i].Cells[x].Value.ToString();
                    }
                }

                excel.Columns.AutoFit();
                excel.Visible = true;
            }
        }
    }
}
