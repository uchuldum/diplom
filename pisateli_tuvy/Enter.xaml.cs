using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace pisateli_tuvy
{
    /// <summary>
    /// Interaction logic for Enter.xaml
    /// </summary>
    public partial class Enter : Window
    {
        static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        static string connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "\\writers.accdb";
        OleDbConnection con = new OleDbConnection(connectionstring);
        static string image = "";
        static string biog = "";
        static string p_work = "";
        int num = 0;

        OpenFileDialog ofd = new OpenFileDialog();
        public Enter()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = "select distr_name from district where distr_id = ";
                OleDbCommand com = new OleDbCommand();
                com.Connection = con;
                con.Open();
                for (int i = 1; i <= 18; i++)
                {
                    com.CommandText = str + i;
                    OleDbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader.GetValue(0).ToString());
                    }
                    reader.Close();
                }
                str = "select max(ws_id) from all_writers";
                com.CommandText = str;
                OleDbDataReader reader2 = com.ExecuteReader();
                while (reader2.Read())
                {
                    num = Convert.ToInt32(reader2.GetValue(0));
                }
                num++;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Fam.Text != "" && Name.Text != "" && Otch.Text != "" && image != "" && num != 0)
            {
                string str = "insert into all_writers values (" + num + ", '" + Fam.Text + " " + Name.Text + " " + Otch.Text + "','" + image + "','" + comboBox1.SelectedItem.ToString() + "','" + biog + "','" + p_work + "')";
                OleDbCommand com = new OleDbCommand();
                try
                {
                    com.Connection = con;
                    con.Open();
                    com.CommandText = str;
                    com.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex + "");
                }
                finally
                {
                    con.Close();
                    MessageBox.Show("DONE");
                }
            }
            else MessageBox.Show("Заполните пустые поля и выберите изображение");
        }

        private void select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == true)
                {
                    image = path + "\\img\\" + ofd.SafeFileName;
                    if (File.Exists(image))
                    {
                        MessageBox.Show("Файл с таким именем уже существует. Пожалуйста переименуйте данный файл.");
                    }
                    else File.Copy(ofd.FileName, image);
                    image = "\\img\\" + ofd.SafeFileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            finally
            {
                MessageBox.Show("Выполнено");
            }
        }

        private void bio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dir = path + "\\rtf\\" + Fam.Text + Name.Text;
                bool exist = Directory.Exists(dir);
                if (!exist) Directory.CreateDirectory(dir);
                ofd.Filter = "rtf files (*.rtf) | *.rtf";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == true)
                {
                    biog = dir + "\\" + ofd.SafeFileName;
                    if (File.Exists(biog))
                    {
                        MessageBox.Show("Файл с таким именем уже существует. Пожалуйста переименуйте данный файл.");
                    }
                    else File.Copy(ofd.FileName, biog);
                    biog = "\\rtf\\" + Fam.Text + Name.Text + "\\" + ofd.SafeFileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            finally
            {
                MessageBox.Show("Выполнено");
            }
        }

        private void pis_work_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dir = path + "\\rtf\\" + Fam.Text + Name.Text;
                ofd.Filter = "rtf files (*.rtf) | *.rtf";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == true)
                {
                    p_work = dir + "\\" + ofd.SafeFileName;
                    if (File.Exists(p_work))
                    {
                        MessageBox.Show("Файл с таким именем уже существует. Пожалуйста переименуйте данный файл.");
                    }
                    else File.Copy(ofd.FileName, p_work);
                    p_work = "\\rtf\\" + Fam.Text + Name.Text + "\\" + ofd.SafeFileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            finally
            {
                MessageBox.Show("Выполнено");
            }
        }

        private void book_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int k = 0;
                string books = "";
                string dir = path + "\\rtf\\" + Fam.Text + Name.Text + "\\books";
                bool exist = Directory.Exists(dir);
                if (!exist) Directory.CreateDirectory(dir);
                ofd.Filter = "rtf files (*.rtf) | *.rtf";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == true)
                {
                    books = dir + "\\" + ofd.SafeFileName;
                    if (File.Exists(books))
                    {
                        MessageBox.Show("Файл с таким именем уже существует. Пожалуйста переименуйте данный файл.");
                    }
                    else File.Copy(ofd.FileName, books);
                    books = "\\rtf\\" + Fam.Text + Name.Text + "\\books\\" + ofd.SafeFileName;
                }
                if (books != "")
                {
                    try
                    {
                        string s = "select max (b_id) from books";
                        OleDbCommand com = new OleDbCommand(s, con);
                        con.Open();
                        OleDbDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            k = reader.GetInt32(0);
                        }
                        reader.Close();
                        k++;
                        s = "insert into books values (" + k + ",'" + name.Text + "','" + city.Text + "','" + izdat.Text + "'," + date.Text + "," + count.Text + ",'" + books + "'," + num + ")";
                        com.CommandText = s;
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex + "");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            finally
            {
                MessageBox.Show("Выполнено");
            }
        }

        private void ab_book_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int k = 0;
                string books = "";
                string dir = path + "\\rtf\\" + Fam.Text + Name.Text + "\\about";
                bool exist = Directory.Exists(dir);
                if (!exist) Directory.CreateDirectory(dir);
                ofd.Filter = "rtf files (*.rtf) | *.rtf";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == true)
                {
                    books = dir + "\\" + ofd.SafeFileName;
                    if (File.Exists(books))
                    {
                        MessageBox.Show("Файл с таким именем уже существует. Пожалуйста переименуйте данный файл.");
                    }
                    else File.Copy(ofd.FileName, books);
                    books = "\\rtf\\" + Fam.Text + Name.Text + "\\about\\" + ofd.SafeFileName;
                }
                if (books != "")
                {
                    try
                    {
                        string s = "select max (ab_id) from about_writer";
                        OleDbCommand com = new OleDbCommand(s, con);
                        con.Open();
                        OleDbDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            k = reader.GetInt32(0);
                        }
                        reader.Close();
                        k++;
                        s = "insert into about_writer values (" + k + ",'" + ab_name.Text + "','" + ab_autor.Text + "','" + ab_izdat.Text + "','" + ab_date.SelectedDate + "','" + books + "'," + num + ")";
                        com.CommandText = s;
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex + "");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            finally
            {
                MessageBox.Show("Выполнено");
            }
        }
    }
}
