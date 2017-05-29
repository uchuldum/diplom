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
using System.Diagnostics;

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
            AUTORS.Visibility = Visibility.Visible;
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
                string[] all_folder = con.Reader_Array("select folder from chogaalchy where chog_fam like '" + txt + "%'", count);
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
                        if (File.Exists(path + "pisateli\\" + all_folder[i] + "\\" + all_img[i])) img.Source = con.GetImage("\\pisateli\\" + all_folder[i] + "\\" + all_img[i]);
                        else img.Source = con.GetImage("\\all\\img\\default.png");
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
                                        AUTORS.Visibility = Visibility.Collapsed;
                                        record.Visibility = Visibility.Visible;
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
        void CopyFile(string sourcefn, string destinfn)
        {
            FileInfo fn = new FileInfo(sourcefn);
            fn.CopyTo(destinfn, true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string trialdirectory = path + "TRIALDIRS\\" + RandomString(10);
            if (!Directory.Exists(trialdirectory)) Directory.CreateDirectory(trialdirectory);
            if (!Directory.Exists(trialdirectory + "\\pisateli")) Directory.CreateDirectory(trialdirectory + "\\pisateli");
            string path_trial = PreparationCopy(trialdirectory);
            RemoveDataBase(path_trial + "\\pisateli.db");
            /////ЗАПИСЬ НА КОМПАКТ ДИСК
            if (Disk.IsChecked == true)
            {
                string progr = path + "BurnAware\\DataDisc.exe",
                           arg = "udf " + trialdirectory;
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                    if (d.IsReady == true)
                    {
                        if (d.DriveType == DriveType.CDRom)
                        {
                            arg = arg + (d.Name).Substring(0, 1) + " -b";
                        }
                    }
                /* if (File.Exists(progr)) Process.Start(progr, arg);
                 else MessageBox.Show("NOFILE");*/
                MessageBox.Show(progr+"  "+arg);
            }
            if (Flash.IsChecked == true)
            {
                ProgressManager pm = new ProgressManager();
               
                string path_nositel = ""; ////Путь КУДА БУДЕТ ЗАПИСАНА ПРОГРАММА
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult d_result = fbd.ShowDialog();
                if (d_result == System.Windows.Forms.DialogResult.OK)
                {
                    path_nositel = fbd.SelectedPath;
                }
                pm.BeginWaiting();
                pm.ChangeStatus("Loading...");
                pm.SetProgressMaxValue(41);
                for(int i = 0;i<41;i++) pm.ChangeProgress(i);
                FolderCopy(path_trial, path_nositel, true);
                FolderDelete(path_trial);
                pm.EndWaiting();

            }

        }
    }
}
