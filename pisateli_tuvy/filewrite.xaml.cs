using System;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
        static int uid;
        static string fio;
        Connect con = new Connect();
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void FolderCopy(string OTKUDA, string KUDA, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(OTKUDA);
            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory does not exist, create it.
            if (!Directory.Exists(KUDA))
            {
                Directory.CreateDirectory(KUDA);
            }
            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(KUDA, file.Name);
                // Copy the file.
                file.CopyTo(temppath, false);
            }
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(KUDA, subdir.Name);

                    // Copy the subdirectories.
                    FolderCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        public filewrite()
        {
            
            WindowState = WindowState.Maximized;
            InitializeComponent();
            writers("");
        }
        private static BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path + imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        public string PreparationCopy(string trialdirectory)
        {
            string str = "";
            try
            {
                string folder = con.Reader("select folder from chogaalchy where chog_id = " + uid); /////Путь до папки определенного писателя
                FolderCopy(path + "all", trialdirectory + "\\all\\", true); //Закинули папку all
                FolderCopy(path, trialdirectory, false); //// Закинули все файлы из корневой папки
                FolderCopy(path + "x86", trialdirectory + "\\x86\\", false); //// Закинули x86]
                FolderCopy(path + "x64", trialdirectory + "\\x64\\", false); //// Закинули x64
                FolderCopy(path + "pisateli\\" + folder, trialdirectory + "\\pisateli\\" + folder, true);////Закинули папку с писателем
                return trialdirectory;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            }
            return str;
        }
        public void RemoveDataBase(string path)
        {
            try
            {
                con.ExecuteNonQueryPath("delete from chogaalchy where chog_id <> "+uid, path);  ////// Удаление остальных писателей
                con.ExecuteNonQueryPath("delete from chog_dug_azhyldar where chdugazh_uid <> " + uid, path); //// Удаление работ о писателе
                con.ExecuteNonQueryPath("delete from nomnary where nomnary_uid <> " + uid, path); //// Удаление книг писателя
                con.ExecuteNonQueryPath("delete from ochulgalary where ochulgalary_uid <> " + uid, path); //// Удаление переводов писателя
                con.ExecuteNonQueryPath("delete from yry_apargan_shulukteri where yry_apsh_uid <> " + uid, path); //// Удаление стихотворений ставших песнями писателя
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        public void FolderDelete(string DelPath)
        {
            DirectoryInfo di = new DirectoryInfo(DelPath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            Directory.Delete(DelPath);
        }
        public void writers(string txt)
        {
            a1.Children.Clear();//stackpanel где выводятся писатели
            string name = autor.Text.Trim();
            try
            {
               
                int count = con.ExecuteScalar("select count (*) from chogaalchy where chog_fam like '" + txt + "%'");
                string[] all_id = con.Reader_Array("select chog_id from chogaalchy where chog_fam like '" + txt + "%'", count);
                string[] all_img = con.Reader_Array("select chog_photo from chogaalchy where chog_fam like '" + txt + "%'", count);
                string[] all_fam = con.Reader_Array("select chog_fam from chogaalchy where chog_fam like '" + txt + "%'", count);
                string[] all_imya = con.Reader_Array("select chog_imya from chogaalchy where chog_fam like '" + txt + "%'", count);
                string[] all_otch = con.Reader_Array("select chog_otch from chogaalchy where chog_fam like '" + txt + "%'", count);
                if (count > 0)
                {
                    for (int i = 0; i < count; ++i)
                    {
                        StackPanel stp = new StackPanel();
                        stp.Width = 500;
                        stp.Height = 100;
                        stp.Orientation = Orientation.Horizontal;
                        stp.Background = Brushes.LightBlue;
                        stp.Margin = new Thickness(0, 20, 0, 0);
                        Image img = new Image();
                        if (File.Exists(path + "all\\img\\" + all_img[i])) img.Source = con.GetImage(all_img[i]);
                        stp.Children.Add(img);
                        TextBlock text = new TextBlock();
                        text.FontSize = 14;
                        text.Text = all_fam[i] + " " + all_imya[i] + " " + all_otch[i];
                        text.Tag = text.Text;
                        stp.Children.Add(text);
                        stp.Tag = all_id[i];
                        a1.Children.Add(stp);
                        stp.MouseEnter += (s, j) =>
                        {
                            Mouse.OverrideCursor = Cursors.Hand;
                            stp.Background = Brushes.Aqua;
                        };
                        stp.MouseLeave += (s, j) =>
                        {
                            Mouse.OverrideCursor = Cursors.Arrow;
                            stp.Background = Brushes.LightBlue;
                        };
                        stp.MouseUp += (s, j) =>
                        {
                            uid = Convert.ToInt32(stp.Tag);
                            fio = text.Tag.ToString().Trim();
                            string messageBoxText = "Вы точно хотите записать данные "+fio+" на носитель?";
                            string caption = "Запись писателя на носитель";
                            MessageBoxButton button = MessageBoxButton.YesNo;
                            MessageBoxImage icon = MessageBoxImage.Warning;
                            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {
                                        string path_nositel = ""; ////Путь КУДА БУДЕТ ЗАПИСАНА ПРОГРАММА
                                        System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                                        System.Windows.Forms.DialogResult d_result = fbd.ShowDialog();
                                        if (d_result == System.Windows.Forms.DialogResult.OK)
                                        {
                                            path_nositel = fbd.SelectedPath;
                                        }
                                      
                                        
                                        string trialdirectory = path + "TRIALDIRS\\" + RandomString(10);
                                        if (!Directory.Exists(trialdirectory)) Directory.CreateDirectory(trialdirectory);
                                        if (!Directory.Exists(trialdirectory + "\\pisateli")) Directory.CreateDirectory(trialdirectory + "\\pisateli");
                                        string path_trial = PreparationCopy(trialdirectory);
                                        RemoveDataBase(path_trial+"\\pisateli.db");
                                        FolderCopy(path_trial,path_nositel,true);
                                        FolderDelete(path_trial);
                                        
                                    }
                                    break;
                                case MessageBoxResult.No:
                                    break;
                            }
                        };
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
        }


       
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            string fam = autor.Text.Trim();
            writers(fam);
        }
      /*  private void stp_MouseEnter(object sender, System.EventArgs e)
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
        */
        void CopyFile(string sourcefn, string destinfn)
        {
            FileInfo fn = new FileInfo(sourcefn);
            fn.CopyTo(destinfn, true);
        }
    }
}
