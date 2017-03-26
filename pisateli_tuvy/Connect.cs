using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace pisateli_tuvy
{
    class Connect
    {
        static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        string connectionString = "Data Source="+ path + "\\pisateli.db";
        public void ExecuteNonQuery(string query)
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
        public int ExecuteScalar(string query)
        {
            int Output = 0;
            using (var connection = new SQLiteConnection(connectionString))
            {
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.CommandText = query;
                    connection.Open();
                    Output= Convert.ToInt32(cmd.ExecuteScalar());
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
                    while(reader.Read())
                    {
                        Output += reader.GetValue(0).ToString();
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return Output;
        }
        public string[] Reader_Array(string query,int count)
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
    }
}
