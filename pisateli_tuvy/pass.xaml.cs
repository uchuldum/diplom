using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;

namespace pisateli_tuvy
{
    /// <summary>
    /// Interaction logic for pass.xaml
    /// </summary>
    public partial class pass : Window
    {
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        static int uid;//// ID автора для изменения 
        static string folder_path;//// Путь до папки писателя
        static int Del_Add = 0;
        static string PAGES = "";
        Connect con = new Connect();
        public pass()
        {
            InitializeComponent();
            Password.Visibility = Visibility.Visible;
           
            foreach (TreeViewItem item in MaterTV.Items)
            {
                item.PreviewMouseUp += (s, j) =>
                {
                    int k = Convert.ToInt32(item.Tag);
                    //MessageBox.Show(k + "");
                    /////ADD
                    if(Del_Add == 1)
                    {
                        switch (k)
                        {
                            case 1:////BIOGRAPHIA
                                {
                                    if(File.Exists(folder_path+"biographia.pdf"))
                                    {
                                        string Exist = exist("Биография этого автора есть. Хотите заменить другим файлом?");
                                        if(Exist == "Yes")
                                        {
                                            File.Delete(folder_path + "biographia.pdf");
                                            File.Copy(COPY(true), folder_path + "biographia.pdf");
                                            MessageBox.Show("Файл изменен");
                                        }
                                    }
                                    else
                                    {
                                        string input = message("Вы хотите добавить биографию писателя?", "Биография");
                                        File.Copy(input, folder_path + "biographia.pdf");
                                        MessageBox.Show("Биография добавлена");
                                    }
                                           //MessageBox.Show(message("Вы хотите добавить биографию писателя", "Биография"));

                                }break;
                                
                            case 2://///Писательская работа
                                {
                                    if (File.Exists(folder_path + "pisatel_rab.pdf"))
                                    {
                                        string Exist = exist("Писательская работа этого автора есть. Хотите заменить другим файлом?");
                                        if (Exist == "Yes")
                                        {
                                            File.Delete(folder_path + "pisatel_rab.pdf");
                                            File.Copy(COPY(true), folder_path + "pisatel_rab.pdf");
                                            MessageBox.Show("Файл изменен");
                                        }
                                    }
                                    else
                                    {
                                        string input = message("Вы хотите добавить писательскую работу для писателя?", "Биография");
                                        File.Copy(input, folder_path + "pisatel_rab.pdf");
                                        MessageBox.Show("Писательская работа добавлена");
                                    }
                                } break;
                            case 3://///Выпущенные книги
                                {

                                } break;
                            case 4:////Работы о писателе
                                {
                                }
                                break;
                            case 5:////Перевод произведений писателя на другие языки
                                {
                                } break;
                            case 6:////Переводы произведений на тувинский язык
                                {
                                } break;
                            case 7:////Стихотворения ставшие песнями
                                {
                                } break;
                        }
                    }
                    /////DELETE
                    else if (Del_Add == 2)
                    {
                        switch (k)
                        {
                            case 1: { MessageBox.Show("DEL" + k); } break;
                            case 2: {;} break;
                            case 3: {;} break;
                            case 4: {;} break;
                            case 5: {;} break;
                            case 6: {;} break;
                            case 7: {;} break;
                        }
                    }
                };

            }
        }
        public string COPY(bool k)
        {

            string s = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "Pdf Files|*.pdf";
            if (k == true)
            {
                if (ofd.ShowDialog() == true)
                {
                    s = ofd.FileName;
                }
                return s;
            }
            else
            {
                if (ofd.ShowDialog() == true)
                {
                    s = ofd.SafeFileName;
                }
                return s;
            }
        }
        public string message(string a ,string b)
        {
            string s = "";
            MessageBoxResult result = MessageBox.Show(a, b, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.OK:
                    {
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.InitialDirectory = @"C:\";
                        ofd.Filter = "Pdf Files|*.pdf";
                        if (ofd.ShowDialog() == true)
                        {
                            s = ofd.FileName;
                        }
                    }
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }
            return s;
        }
        public string exist(string a)
        {
            MessageBoxResult result = MessageBox.Show(a, "Файл существует", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.OK:
                    {
                        return "Yes";
                    }break;
                    
                case MessageBoxResult.Cancel:
                    {
                        return "No";
                    }break;
            }
            return "";
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
            Back.Visibility = Visibility.Collapsed;
            adminPanel.Visibility = Visibility.Visible;
            PAGES = "1";
        }


        private void no_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Add_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Back.Visibility = Visibility.Visible;
            adminPanel.Visibility = Visibility.Collapsed;
            addWR.Visibility = Visibility.Visible;
            PAGES += "2";
        }

        private void Change_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PAGES += "3";
            Back.Visibility = Visibility.Visible;
            adminPanel.Visibility = Visibility.Collapsed;
            addWR.Visibility = Visibility.Collapsed;
            searchWR.Visibility = Visibility.Visible;
            writers.Children.Clear();
            try
            {
                int count = con.ExecuteScalar("select count (*) from chogaalchy");
                string[] all_id = con.Reader_Array("select chog_id from chogaalchy order by chog_fam", count);
                string[] all_img = con.Reader_Array("select chog_photo from chogaalchy order by chog_fam", count);
                string[] all_fam = con.Reader_Array("select chog_fam from chogaalchy order by chog_fam", count);
                string[] all_imya = con.Reader_Array("select chog_imya from chogaalchy order by chog_fam", count);
                string[] all_otch = con.Reader_Array("select chog_otch from chogaalchy order by chog_fam", count);
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
                        searchWR.Visibility = Visibility.Collapsed;
                        editWR.Visibility = Visibility.Visible;
                        folder_path = path + "pisateli\\" + con.Reader("select folder from chogaalchy where chog_id = " + uid)+"\\";
                        PAGES += "4";
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            }
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
                        searchWR.Visibility = Visibility.Collapsed;
                        editWR.Visibility = Visibility.Visible;
                        folder_path = path +"pisateli\\" + con.Reader("select folder from chogaalchy where chog_id = "+uid)+"\\";
                        //MessageBox.Show(uid + "");
                    };
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            }
        }
        //////Добавление материала
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Del_Add = 1;
            MaterText.Text = "Добавление материала";
            DeleteMater.Visibility = Visibility.Visible;
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Del_Add = 2;
            MaterText.Text = "Удаление материала";
            DeleteMater.Visibility = Visibility.Visible;
        }
        public string perevod(string a)
        {
            string AlphRus = "АБВГДЕЁЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзиклмнопрстуфхцчшщъыьэюя ";
            string AlphEng = "ABVGDEEJZIKLMNOPRSTUFHCCHSQYQEUAabvgdeejziklmnoprstufhcchsqyqeua_";
            string output = "";
            foreach (char s in a)
            {
                for(int i=0;i<AlphRus.Length;++i)
                {
                    if (s == AlphRus[i])
                    {
                        output += AlphEng[i];
                    }
                }
            }
            return output;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string Name = Book_Name.Text.Trim();
            string Janr = Book_Janr.Text.Trim();
            string Year = Book_Year.Text.Trim();
            string Tipograph = Book_Tipogr.Text.Trim();
            string Pages = Book_Pages.Text.Trim();
            int k = 0;
            string _Input = "";
            string _Output = "";
            string _FileName = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "Pdf Files|*.pdf";
            if (ofd.ShowDialog() == true)
            {
                _Input = ofd.FileName;
                _FileName = perevod(ofd.SafeFileName)+".pdf";
                _Output = folder_path + "nomnary\\" + _FileName;

            }
            if (k == 0)
            {
                con.ExecuteNonQuery("insert into nomnary (nomnary_title,nomnary_zhanr,nomnary_god,nomnary_tipograph,nomnary_stranisy,nomnary_text,nomnary_uid,nomnary_pozisiya) values ('" + Name + "','" + Janr + "'," + Year + ",'" + Tipograph + "'," + Pages + ",'" + _FileName + "'," + uid + ",0)");
                k = 1;
            }
            if (k==1)
            {
                File.Copy(_Input, _Output);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (PAGES.Length>1)
            {
                int _now = Convert.ToInt32(PAGES.Substring(PAGES.Length - 1, 1));
                PAGES = PAGES.Substring(0, PAGES.Length - 1);
                int _before = Convert.ToInt32(PAGES.Substring(PAGES.Length - 1, 1));
                switch (_now)
                {
                    case 2: addWR.Visibility = Visibility.Collapsed; break;
                    case 3: searchWR.Visibility = Visibility.Collapsed; break;
                    case 4: editWR.Visibility = Visibility.Collapsed;break;
                }
                switch (_before)
                {
                    case 1:
                        {
                            admin();
                        } break;
                    case 3:
                        {
                            searchWR.Visibility = Visibility.Visible;
                        }break;
                }
            }
        }
    }
}
