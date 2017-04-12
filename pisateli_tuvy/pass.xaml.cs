using System;
using System.Windows;
using System.Collections.Generic;
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
using System.IO;

namespace pisateli_tuvy
{
    /// <summary>
    /// Interaction logic for pass.xaml
    /// </summary>
    public partial class pass : Window
    {
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        static int uid = 0;//// ID автора для изменения 
        Connect con = new Connect();
        public pass()
        {
            InitializeComponent();
            Password.Visibility = Visibility.Visible;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            string passwrd = "1234";
            string log = "admin";
            if (login.Text == log && password.Password == passwrd)
            {
                this.WindowState = WindowState.Maximized;
                Password.Visibility = Visibility.Collapsed;
                admin();
            }
            else MessageBox.Show("Неверный пароль или логин!");
        }
        public void admin()
        {
            adminPanel.Visibility = Visibility.Visible;
        }


        private void no_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Add_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            adminPanel.Visibility = Visibility.Collapsed;
            addWR.Visibility = Visibility.Visible;
        }

        private void Change_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            adminPanel.Visibility = Visibility.Collapsed;
            addWR.Visibility = Visibility.Collapsed;
            searchWR.Visibility = Visibility.Visible;
        }

        private void Delete_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {

        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                string fam = Surname.Text.Trim();
                string imya = Name.Text.Trim();
                string otch = Patronymic.Text.Trim();
                string fio = (fam + imya + otch).Trim();
                int count = con.ExecuteScalar("select count (*) from chogaalchy");
                if (fam != "" && imya != "" && otch != "")
                {
                    string[] id = con.Reader_Array("select chog_id from chogaalchy", count);
                    string[] all_fam = con.Reader_Array("select chog_fam from chogaalchy", count);
                    string[] all_imya = con.Reader_Array("select chog_imya from chogaalchy", count);
                    string[] all_otch = con.Reader_Array("select chog_otch from chogaalchy", count);
                    string[] all_fio = new string[count];
                    for (int i = 0; i < count; ++i)
                    {
                        all_fio[i] = (all_fam[i]+all_imya[i]+all_otch[i]).Trim();
                    }
                    foreach(string s in all_fio)
                    {
                       
                        if(s==fio)
                        {
                            string messageBoxText = "Писатель с такими данными уже состит в базе хотите изменить писателя?";
                            string caption = "Существующий писатель";
                            MessageBoxButton button = MessageBoxButton.YesNo;
                            MessageBoxImage icon = MessageBoxImage.Warning;
                            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                   
                                    break;
                                case MessageBoxResult.No:
                                    break;
                            }
                            break;
                        }
                        else
                        {
                            MessageBox.Show("horosho");
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            }
        }
        //////ПОИСК ПИСАТЕЛЯ ПО ФАМИЛИИ
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            writers.Children.Clear();
            try
            {
                string fam = Familiya.Text.Trim();
                int count = con.ExecuteScalar("select count (*) from chogaalchy where chog_fam like '"+fam+"%'");
                string[] all_id = con.Reader_Array("select chog_id from chogaalchy where chog_fam like '" + fam + "%'", count);
                string[] all_img = con.Reader_Array("select chog_photo from chogaalchy where chog_fam like '" + fam + "%'", count);
                string[] all_fam = con.Reader_Array("select chog_fam from chogaalchy where chog_fam like '" + fam + "%'", count);
                string[] all_imya = con.Reader_Array("select chog_imya from chogaalchy where chog_fam like '" + fam + "%'", count);
                string[] all_otch = con.Reader_Array("select chog_otch from chogaalchy where chog_fam like '" + fam + "%'", count);
                for(int i=0;i<count;++i)
                {
                    StackPanel stp = new StackPanel();
                    stp.Width = 500;
                    stp.Height = 100;
                    stp.Orientation = Orientation.Horizontal;
                    stp.Background = Brushes.LightBlue;
                    stp.Margin = new Thickness (0,20,0,0);
                    Image img = new Image();
                    if(File.Exists(path+"all\\img\\"+all_img[i])) img.Source = con.GetImage(all_img[i]);
                    stp.Children.Add(img);
                    TextBlock text = new TextBlock();
                    text.FontSize = 14;
                    text.Text = all_fam[i] + " " + all_imya[i] + " " + all_otch[i];
                    stp.Children.Add(text);
                    stp.Tag = all_id[i];
                    writers.Children.Add(stp);
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

                        //MessageBox.Show(uid + "");
                    };
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            }
        }
    }
}
