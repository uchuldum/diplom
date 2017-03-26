using System;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace pisateli_tuvy
{
    /// <summary>
    /// Interaction logic for filewrite.xaml
    /// </summary>
    public partial class filewrite : Window
    {
        static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        static string connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "\\writers.accdb";
        OleDbConnection con = new OleDbConnection(connectionstring);
        public filewrite()
        {
            InitializeComponent();
        }
        private static BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path + imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            a1.Children.Clear();

            string name = autor.Text.Trim();
            try
            {
                con.Open();

                string count = "select count (*) from all_writers where ws_name like '" + name + "%'";
                string str = "select ws_id,ws_name,ws_img from all_writers where ws_name like '" + name + "%'";
                OleDbCommand com = new OleDbCommand();
                com.Connection = con;
                com.CommandText = count;
                int id = 0;
                string sname = "";
                string img = "";
                int k = Convert.ToInt32(com.ExecuteScalar());
                if (k > 0)
                {
                    for (int i = 0; i < k; ++i)
                    {
                        StackPanel stp = new StackPanel();
                        stp.Margin = new Thickness(0, 5, 0, 0);
                        stp.Background = Brushes.LightSteelBlue;
                        stp.Orientation = Orientation.Horizontal;
                        com.CommandText = str;
                        OleDbDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            id = reader.GetInt32(0);
                            sname = reader.GetValue(1).ToString();
                            img = reader.GetValue(2).ToString();
                        }
                        reader.Close();
                        Image image = new Image();
                        image.Source = GetImage(img);
                        TextBlock txb = new TextBlock();
                        txb.Text = sname;
                        txb.VerticalAlignment = VerticalAlignment.Center;
                        txb.FontFamily = new FontFamily("Arial");
                        txb.FontSize = 16;
                        a1.Children.Add(stp);
                        stp.Children.Add(image);
                        stp.Children.Add(txb);
                        stp.Width = a1.Width;
                        stp.Height = 60;
                        stp.Tag = id;
                        stp.MouseEnter += new System.Windows.Input.MouseEventHandler(stp_MouseEnter);
                        stp.MouseLeave += new System.Windows.Input.MouseEventHandler(stp_MouseLeave);
                        stp.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(stp_MouseUp);
                    }
                }
                else
                {
                    StackPanel stp = new StackPanel();
                    TextBlock txb = new TextBlock();
                    txb.Text = "Авторов пока нет с такой фамилией";
                    a1.Children.Add(stp);
                    stp.Margin = new Thickness(0, 5, 0, 0);
                    stp.Background = Brushes.LightSteelBlue;
                    stp.Children.Add(txb);
                    txb.VerticalAlignment = VerticalAlignment.Center;
                    txb.FontFamily = new FontFamily("Arial");
                    txb.FontSize = 16;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            finally
            {

                con.Close();
            }
        }
        private void stp_MouseEnter(object sender, System.EventArgs e)
        {
            StackPanel tvItem = (StackPanel)sender;
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void stp_MouseLeave(object sender, System.EventArgs e)
        {
            StackPanel tvItem = (StackPanel)sender;
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        private void stp_MouseUp(object sender, System.EventArgs e)
        {
            StackPanel stp = (StackPanel)sender;
            try
            {
                string path1 = "";
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    path1 = fbd.SelectedPath;
                }
                
                /// Folder "all"
                ZipFile.CreateFromDirectory(path+"\\all",path1+"tuvawriters.zip");
                ZipFile.ExtractToDirectory(path1 + "tuvawriters.zip", path1+"\\all");
                if (File.Exists(path1 + "tuvawriters.zip")) File.Delete(path1 + "tuvawriters.zip");
               

                /////create folder "pisateli"
                if (!Directory.Exists(path1 + "\\pisateli")) Directory.CreateDirectory(path1 + "\\pisateli");

                ///Folder pisatalya
                ///
                string pisatel = "";
                OleDbConnection con = new OleDbConnection(connectionstring);
                try
                {
                    
                    string str = "select ws_pisatel from all_writers where ws_id = "+stp.Tag;
                    OleDbCommand com = new OleDbCommand(str, con);
                    con.Open();
                    OleDbDataReader reader = com.ExecuteReader();
                    while(reader.Read())
                    {
                        pisatel = reader.GetValue(0).ToString();
                    }
                    reader.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex + "");
                }
                finally
                {
                    con.Close();
                }
                string pis_path = path1 + "\\pisateli\\" + pisatel + ".zip";
                ZipFile.CreateFromDirectory(path + "\\pisateli\\"+pisatel, pis_path);
                ZipFile.ExtractToDirectory(pis_path, path1+"\\pisateli\\"+pisatel);
                if (File.Exists(pis_path)) File.Delete(pis_path);
                ///////exe + accdb
              
                if(!File.Exists(path1 + "\\writers.accdb")) File.Copy(path + "\\writers.accdb", path1 + "\\writers.accdb");
                if (!File.Exists(path1 + "\\pisateli_tuvy.exe")) File.Copy(path + "\\pisateli_tuvy.exe", path1 + "\\pisateli_tuvy.exe");
                
                string connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + "\\writers.accdb";
                OleDbConnection conn = new OleDbConnection(connection);
                try
                {
                    string[] str = new string[6];
                     str[0] = "delete * from all_writers where ws_id <> " + stp.Tag;
                     str[1] = "delete * from about_writer where ab_ws <> " + stp.Tag;
                     str[2] = "delete * from books where b_wr <> " + stp.Tag;
                     str[3] = "delete * from per_other where po_ws <> " + stp.Tag;
                     str[4] = "delete * from per_tuv where pt_ws <> " + stp.Tag;
                     str[5] = "delete * from stih_pes where sp_ws <> " + stp.Tag;
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = conn;
                    conn.Open();
                    for(int i=0;i<6;++i)
                    {
                        command.CommandText = str[i];
                        command.ExecuteNonQuery();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex + "");
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            finally
            {
                MessageBox.Show("Скопировано");
            }
        }
        void CopyFile(string sourcefn, string destinfn)
        {
            FileInfo fn = new FileInfo(sourcefn);
            fn.CopyTo(destinfn, true);
        }
    }
}
