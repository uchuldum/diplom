using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;

namespace pisateli_tuvy
{
    class Connect
    {
        static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        string connectionString = "Data Source=" + path + "\\pisateli.db";
        public void ExecuteNonQuery(string query)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.CommandText = query;
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        public void ExecuteNonQueryPath(string query, string DBPath)
        {

            using (var connection = new SQLiteConnection("Data Source=" + DBPath))
            {
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.CommandText = query;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public int ExecuteScalar(string query)
        {
            int Output = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.CommandText = query;
                    connection.Open();
                    Output = Convert.ToInt32(cmd.ExecuteScalar());
                    connection.Close();
                }
            }
            return Output;
        }


        public string Reader(string query)
        {
            string Output = "";
            using (var connection = new SQLiteConnection(connectionString))
            {
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.CommandText = query;
                    connection.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Output += reader.GetValue(0).ToString();
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return Output;
        }
        public string[] Reader_Array(string query, int count)
        {
            string[] Output = new string[count];
            using (var connection = new SQLiteConnection(connectionString))
            {
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.CommandText = query;
                    connection.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    int k = 0;
                    while (reader.Read())
                    {
                        Output[k] += reader.GetValue(0).ToString();
                        k++;
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return Output;
        }
        public int Reader_Int(string query)
        {
            int Output = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.CommandText = query;
                    connection.Open();
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Output = reader.GetInt32(0);
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return Output;
        }
       /*public StackPanel Search(string Name, string Table, StackPanel stack)
        {
            stack.Children.Clear();
            int count = ExecuteScalar("select count (*) from chogaalchy where chog_fam like '" + Name + "%'");
            string[] all_id = Reader_Array("select chog_id from chogaalchy where chog_fam like '" + Name + "%'", count);
            string[] all_img = Reader_Array("select chog_photo from chogaalchy where chog_fam like '" + Name + "%'", count);
            string[] all_fam = Reader_Array("select chog_fam from chogaalchy where chog_fam like '" + Name + "%'", count);
            string[] all_imya = Reader_Array("select chog_imya from chogaalchy where chog_fam like '" + Name + "%'", count);
            string[] all_otch = Reader_Array("select chog_otch from chogaalchy where chog_fam like '" + Name + "%'", count);
            
        }*/
        public BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path + "\\all\\img\\" + imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }

    }
}
