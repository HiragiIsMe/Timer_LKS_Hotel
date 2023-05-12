using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Timer_LKS_Hotel
{
    class Utility
    {
        public static string connection = @"Data Source=DESKTOP-HUJGH1E\SQLEXPRESS;Initial Catalog=timerHotel;Integrated Security=True";

        public static SqlConnection conn = new SqlConnection(connection);

        public static string EncPass(string pass)
        {
            using(SHA256Managed sha256 = new SHA256Managed())
            {
                byte[] password = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                string getPass = Convert.ToBase64String(password);

                return getPass;
            }
        }

        public static byte[] EncImage(Image img)
        {
            ImageConverter imageConverter = new ImageConverter();
            byte[] image = (byte[])imageConverter.ConvertTo(img, typeof(byte[]));

            return image;
        }
        public static DataTable getData(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        public static int getNumData(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            
            int a = reader.GetInt32(0);
            conn.Close();

            return a;
        }
        public static string getStrData(string query)
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            string a  = reader.GetString(0);
            conn.Close();

            return a;
        }
    }

    class Model
    {
        public static int id { set; get; }
        public static string Name { set; get; }
        public static int JobID { set; get; }
    }
}
//1a1d26
//ffdf00
