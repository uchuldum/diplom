using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data.OleDb;
using System.Data;

namespace pisateli_tuvy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Connect con = new Connect();
        static string glob_autor = "";   ///// НОМЕР АВТОРА КОТОРОГО СМОТРЯТ
        private static BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path + "\\all\\img\\" + imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        private static BitmapImage GetImage2(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path + imageUri, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        ///Визулизация МЕНЮ
        public void menu()
        {
            int count = Convert.ToInt32(con.ExecuteScalar("select count (*) from menu where menuparent_id=0"));
            string[] menu_item = con.Reader_Array("select menu_name from menu where menuparent_id=0 ", count);
            Menu.Children.Clear();

            //string[] menu_item = new string[count];
            for (int i=0;i<count;++i)
            {
                StackPanel stp = new StackPanel();
                stp.Width = Menu.Width;
                stp.Height = 15;
                TextBlock txb = new TextBlock();
                txb.Width = stp.Width;
                txb.Text = menu_item[i];
                Menu.Children.Add(stp);
                stp.Children.Add(txb);
            }
        }
        public void vis()
        {
            stack_distr.Visibility = Visibility.Hidden;
            this.main_tuv.Visibility = Visibility.Hidden;
            this.pis_tuv.Visibility = Visibility.Hidden;
            this.pisatel.Visibility = Visibility.Visible;
            //richTextBox3.Visibility = Visibility.Visible;
        }
        public MainWindow()
        {
            InitializeComponent();
            int index = 1;
            try
            {

                /////////////// ТУТ ДОЛЖНА БЫТЬ КАРТА
                for (int i = 0; i < 18; ++i)
                {
                    TreeViewItem tw2Header = new TreeViewItem();
                    tw2.Items.Add(tw2Header);
                }
                foreach (TreeViewItem item in tw2.Items)
                {
                    //////ЗАПОЛНЕНИЕ TREEVIEW///////////////////////
                    string kniga = "";
                    kniga = con.Reader("select r_name from region where region_id = " + index.ToString());
                    item.Header = kniga;
                    item.Name = "distr" + index.ToString();                                                                         ///////////////////DONE
                    item.Tag = index.ToString();
                    index++;
                    ////////////////////////////////////////////////////
                    item.MouseEnter += new System.Windows.Input.MouseEventHandler(item_MouseEnter);
                    item.MouseLeave += new System.Windows.Input.MouseEventHandler(item_MouseLeave);
                    item.PreviewMouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(item_MouseDoubleClick);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }

        }


        private void item_MouseEnter(object sender, System.EventArgs e)
        {
            TreeViewItem tvItem = (TreeViewItem)sender;
            Mouse.OverrideCursor = Cursors.Hand;
            tvItem.FontStyle = FontStyles.Oblique;
            tvItem.Foreground = Brushes.Pink;
        }
        private void item_MouseLeave(object sender, System.EventArgs e)
        {
            foreach (TreeViewItem item in tw2.Items)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                item.FontStyle = FontStyles.Normal;
                item.Foreground = Brushes.Black;
            }
        }


        /////////СОБЫТИЕ НА НАЖИТИЕ КОЖУУНА
        private void item_MouseDoubleClick(object sender, System.EventArgs e)
        {

            
            main_tuv.Visibility = Visibility.Hidden;
            TreeViewItem tvItem = (TreeViewItem)sender;
            stack_distr.Visibility = Visibility.Visible;
            writers.Children.Clear();
            try
            { ///////КАРТА+ТЕКСТ
                string region_photo =  "\\all\\dist_img\\" + con.Reader("select region_id from region where region_id = " + tvItem.Tag)+".png";
                string region_text = con.Reader("select r_info from region where region_id = " + tvItem.Tag);
              
                //////////////////Загрузили изображения кожуунов
                distr_img.Source = GetImage2(region_photo);

                /////Загрузили тексты кожуунов
                dist_txt.Document.Blocks.Clear();
                dist_txt.Document.Blocks.Add(new Paragraph(new Run(region_text)));



                ////////Писатели В КОЖУУНАХ    

                int count = con.ExecuteScalar("select count (*) from chogaalchy where chog_region_id=" + tvItem.Tag); ///КОЛИЧЕСТВО ПИСАТЕЛЕЙ В ДАННОМ РЕГИОНЕ
                if(count!=0)
                for (int i = 0; i < count; ++i)
                {
                    StackPanel stP = new StackPanel();
                    string[] pis_surname = con.Reader_Array("select chog_fam from chogaalchy where chog_region_id=" + tvItem.Tag, count);
                    string[] pis_name = con.Reader_Array("select chog_imya from chogaalchy where chog_region_id=" + tvItem.Tag,count);
                    string[] pis_patronymic = con.Reader_Array("select chog_otch from chogaalchy where chog_region_id=" + tvItem.Tag,count);
                    string[] pis_img =con.Reader_Array("select chog_photo from chogaalchy where chog_region_id=" + tvItem.Tag,count);
                    string[] pis_id = con.Reader_Array("select chog_id from chogaalchy where chog_region_id=" + tvItem.Tag,count);

                    Image img = new Image();
                    TextBlock txtB = new TextBlock();
                    if(File.Exists(path+ "\\all\\img\\" + pis_img[i])) img.Source = GetImage2("\\all\\img\\" + pis_img[i]);
                    txtB.Text = pis_surname[i]+" "+ pis_name[i] + " " + pis_patronymic[i];
                    txtB.VerticalAlignment = VerticalAlignment.Center;  
                    txtB.Margin = new Thickness(5, 0, 0, 0);
                    stP.Orientation = Orientation.Horizontal;
                    stP.Background = Brushes.LightSteelBlue;
                    txtB.FontFamily = new FontFamily("Arial");
                    txtB.FontSize = 16;
                    stP.MaxWidth = 1024;
                    stP.MinWidth = 800;
                    stP.Height = 100;
                    stP.Width = writers.Width;
                    stP.Margin = new Thickness(0, 20, 0, 0);
                    writers.Children.Add(stP);
                    stP.Children.Add(img);
                    stP.Children.Add(txtB);
                    stP.MouseEnter += new System.Windows.Input.MouseEventHandler(stP_MouseEnter);
                    stP.MouseLeave += new System.Windows.Input.MouseEventHandler(stP_MouseLeave);
                    stP.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(stP_MouseUp);
                    stP.Tag = pis_id[i];
                }
                else
                {
                    StackPanel stP = new StackPanel();
                    TextBlock txtB = new TextBlock();
                    txtB.Text = "К сожаления в базе нет писателей по этому кожууну";
                    txtB.VerticalAlignment = VerticalAlignment.Center;
                    txtB.Margin = new Thickness(5, 0, 0, 0);
                    stP.Orientation = Orientation.Horizontal;
                    stP.Background = Brushes.LightSteelBlue;
                    txtB.FontFamily = new FontFamily("Arial");
                    txtB.FontSize = 16;
                    stP.MaxWidth = 1024;
                    stP.MinWidth = 800;
                    stP.Height = 100;
                    stP.Width = writers.Width;
                    stP.Margin = new Thickness(0, 20, 0, 0);
                    writers.Children.Add(stP);
                    stP.Children.Add(txtB);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        private void stP_MouseEnter(object sender, System.EventArgs e)
        {
            StackPanel tvItem = (StackPanel)sender;
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void stP_MouseLeave(object sender, System.EventArgs e)
        {
            StackPanel tvItem = (StackPanel)sender;
            Mouse.OverrideCursor = Cursors.Arrow;
        }



        /////////////////////////////////////////////НАЖАТИЕ НА ПИСАТЕЛЯ В КОЖУУНАХ
        

        
        private void stP_MouseUp(object sender, System.EventArgs e)
        {
            StackPanel stP = (StackPanel)sender;
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();

            // treeView1.Visibility = Visibility.Visible;
            menu();



            webbrowser.Visibility = Visibility.Visible;
            try
            {
                string biographia = con.Reader("select chog_biografiya from chogaalchy where chog_id =" + stP.Tag.ToString()); 
                webbrowser.Navigate(path + biographia);///Вывод биографии писателя
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }

        }
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory;
        static DataGrid[] a = new DataGrid[5];
        string connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "\\writers.accdb";
        ////////WINDOW_Loaded
        public void main()
        {
            stack_distr.Visibility = Visibility.Hidden;
            this.pis_tuv.Visibility = Visibility.Hidden;
            this.pisatel.Visibility = Visibility.Hidden;
            this.main_tuv.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Maximized;
            TextRange range;
            FileStream fStream;

            if (File.Exists(path + "\\all\\rtf\\1.rtf"))
            {
                range = new TextRange(this.richTextBox1.Document.ContentStart, this.richTextBox1.Document.ContentEnd);
                fStream = new FileStream(path + "\\all\\rtf\\1.rtf", FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }
            if (File.Exists(path + "\\all\\rtf\\2.rtf"))
            {
                range = new TextRange(this.richTextBox2.Document.ContentStart, this.richTextBox2.Document.ContentEnd);
                fStream = new FileStream(path + "\\all\\rtf\\2.rtf", FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }

        }
        /////////////////////////////////////Видимость нужного DATAGRID
        public void vis_datagrid(DataGrid b)
        {
            vis();
            for (int i = 0; i < 5; i++)
            {
                if (b == a[i]) b.Visibility = Visibility.Visible;
                else a[i].Visibility = Visibility.Hidden;
            }

        }
        private void LoadTextDocument(string fileName, RichTextBox rtbMain)
        {
            if (File.Exists(fileName))
            {
                var range = new TextRange(rtbMain.Document.ContentStart, rtbMain.Document.ContentEnd);
                var fStream = new FileStream(fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Text);
                fStream.Close();
            }
        }
        
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main();
            a[0] = dataGrid1;
            a[1] = dataGrid2;
            a[2] = dataGrid3;
            a[3] = dataGrid4;
            a[4] = dataGrid5;
            main_img.Source = GetImage("logo_main.png");
        }
       
        private void button2_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void button2_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void button3_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void button3_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void button4_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void button4_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void button5_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void button5_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
            pis_tuv.Visibility = Visibility.Visible;
            pisatel.Visibility = Visibility.Hidden;
            writers2.Children.Clear();
            try
            {
                OleDbConnection con = new OleDbConnection(connectionstring);
                OleDbCommand com = new OleDbCommand();
                com.Connection = con;
                ////////Писатель
                int count = 0;
                string pis_name = "";
                string pis_img = "";
                string pis_id = "";
                string str1 = "select count (*) from all_writers";
                com.CommandText = str1;
                con.Open();
                count = Convert.ToInt32(com.ExecuteScalar());
                for (int i = 0; i < count; ++i)
                {
                    string str2 = "select ws_id, ws_name, ws_img from all_writers where ws_id = " + (i + 1).ToString();
                    StackPanel stP2 = new StackPanel();
                    com.CommandText = str2;
                    OleDbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        pis_id = reader.GetValue(0).ToString();
                        pis_name = reader.GetValue(1).ToString();
                        pis_img = reader.GetValue(2).ToString();
                    }
                    reader.Close();
                    Image img2 = new Image();
                    TextBlock txtblk = new TextBlock();
                    img2.Source = GetImage2(pis_img);
                    img2.HorizontalAlignment = HorizontalAlignment.Right;
                    txtblk.Text = pis_name;
                    txtblk.VerticalAlignment = VerticalAlignment.Center;
                    txtblk.Margin = new Thickness(5, 0, 0, 0);
                    txtblk.FontFamily = new FontFamily("Arial");
                    txtblk.FontSize = 16;
                    stP2.Orientation = Orientation.Horizontal;
                    stP2.Background = Brushes.LightSteelBlue;
                    stP2.MaxWidth = 1024;
                    stP2.MinWidth = 800;
                    stP2.Height = 100;
                    stP2.Width = writers2.Width;
                    stP2.Margin = new Thickness(0, 20, 0, 0);
                    writers2.Children.Add(stP2);
                    stP2.Children.Add(img2);
                    stP2.Children.Add(txtblk);
                    stP2.MouseEnter += new System.Windows.Input.MouseEventHandler(stP_MouseEnter);
                    stP2.MouseLeave += new System.Windows.Input.MouseEventHandler(stP_MouseLeave);
                    stP2.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(stP_MouseUp);
                    stP2.Tag = pis_id;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
            //treeView1.Visibility = Visibility.Visible;
         //   richTextBox3.Visibility = Visibility.Visible;
            string str = "select w_biog  from writer where w_ws = 1";
            string biog = "";
            using (OleDbConnection con = new OleDbConnection(connectionstring))
            {
                OleDbCommand com = new OleDbCommand(str, con);
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    biog = reader.GetValue(0).ToString();
                }
                reader.Close();
                con.Close();
            }

            TextRange range;
            FileStream fStream;
          /*  if (File.Exists(path + biog))
            {
                range = new TextRange(this.richTextBox3.Document.ContentStart, this.richTextBox3.Document.ContentEnd);
                fStream = new FileStream(path + biog, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }*/
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.pis_tuv.Visibility = Visibility.Hidden;
            this.pisatel.Visibility = Visibility.Hidden;
            this.main_tuv.Visibility = Visibility.Visible;
        }

        private void dataGrid1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
            string str = "select b_path from books where b_id = " + (dataGrid1.SelectedIndex+1);
            string kniga = "";
            try
            {
                using (OleDbConnection con = new OleDbConnection(connectionstring))
                {
                    OleDbCommand com = new OleDbCommand(str, con);
                    con.Open();
                    OleDbDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        kniga = reader.GetValue(0).ToString();
                    }
                    reader.Close();
                    con.Close();
                }

             /*   TextRange range;
                FileStream fStream;
                if (File.Exists(path + kniga))
                {
                    range = new TextRange(this.richTextBox3.Document.ContentStart, this.richTextBox3.Document.ContentEnd);
                    fStream = new FileStream(path + kniga, FileMode.OpenOrCreate);
                    range.Load(fStream, DataFormats.Rtf);
                    fStream.Close();
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        private void dataGrid2_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
            string str = "select ab_path from about_writer where ab_id = " + (dataGrid2.SelectedIndex + 1);
            string kniga = "";
            using (OleDbConnection con = new OleDbConnection(connectionstring))
            {
                OleDbCommand com = new OleDbCommand(str, con);
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    kniga = reader.GetValue(0).ToString();
                }
                reader.Close();
                con.Close();
            }

        /*    TextRange range;
            FileStream fStream;
            if (File.Exists(path + kniga))
            {
                range = new TextRange(this.richTextBox3.Document.ContentStart, this.richTextBox3.Document.ContentEnd);
                fStream = new FileStream(path + kniga, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }*/
        }
        private void dataGrid3_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
            string str = "select pt_path from per_tuv where pt_id = " + (dataGrid3.SelectedIndex + 1);
            string kniga = "";
            using (OleDbConnection con = new OleDbConnection(connectionstring))
            {
                OleDbCommand com = new OleDbCommand(str, con);
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    kniga = reader.GetValue(0).ToString();
                }
                reader.Close();
                con.Close();
            }

          /*  TextRange range;
            FileStream fStream;
            if (File.Exists(path + kniga))
            {
                range = new TextRange(this.richTextBox3.Document.ContentStart, this.richTextBox3.Document.ContentEnd);
                fStream = new FileStream(path + kniga, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }*/
        }
        private void dataGrid5_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
            string str = "select sp_path from stih_pes where sp_id = " + (dataGrid5.SelectedIndex + 1);
            string kniga = "";
            using (OleDbConnection con = new OleDbConnection(connectionstring))
            {
                OleDbCommand com = new OleDbCommand(str, con);
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    kniga = reader.GetValue(0).ToString();
                }
                reader.Close();
                con.Close();
            }

          /*  TextRange range;
            FileStream fStream;
            if (File.Exists(path + kniga))
            {
                range = new TextRange(this.richTextBox3.Document.ContentStart, this.richTextBox3.Document.ContentEnd);
                fStream = new FileStream(path + kniga, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }*/
        }

        private void dataGrid4_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
            string str = "select po_path from per_other where po_id = " + (dataGrid4.SelectedIndex + 1);
            string kniga = "";
            using (OleDbConnection con = new OleDbConnection(connectionstring))
            {
                OleDbCommand com = new OleDbCommand(str, con);
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    kniga = reader.GetValue(0).ToString();
                }
                reader.Close();
                con.Close();
            }

          /*  TextRange range;
            FileStream fStream;
            if (File.Exists(path + kniga))
            {
                range = new TextRange(this.richTextBox3.Document.ContentStart, this.richTextBox3.Document.ContentEnd);
                fStream = new FileStream(path + kniga, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }*/
        }
        private void Image_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            main();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
             /*try
             {
                 if(comboBox1.SelectedIndex>-1 && textBox1.Text.Trim()!="")

             }*/
        }

        private void image2_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void image2_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void image3_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void image3_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void image4_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void image4_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void image2_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            main_img.Source = GetImage("logo_main.png");
            button2.Content = "Чогаалчылар";
            button3.Content = "Программалар";
            button4.Content = "Дузалал";
            button5.Content = "Кирери";
            label1.Content = "Чогаалчылар";
            search.Content = "Дилээр";
           /* t1.Header = "Чогаалчының намдары";
            t2.Header = "Чогаадыкчы ажыл-чорудулгазы";
            t3.Header = "Үндүрген номнары";
            t4.Header = "Чогаалчының дугайында ажылдар";
            t5.Header = "Очулгалары";
            t6.Header = "Публицистика";
            t7.Header = "Ыры апарган шүлүктери";
            t51.Header = "Чогаалчының ажылдарының өске дылдарже очулгазы";
            t52.Header = "Өске чогаалдарны тыва дылче чогаалчының очулдурганы";
            t8.Header = "Галерея";
            t81.Header = "Чогаалдарга иллюстрациялар";
            t82.Header = "Чуруктар";
            t83.Header = "Аудиоматериалдар";
            t84.Header = "Видеоматериалдар";
            t85.Header = "Презентация";*/

        }

        private void image3_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            main_img.Source = GetImage("logo_rus.png");
            button2.Content = "Писатели";
            button3.Content = "Программы";
            button4.Content = "Помощь";
            button5.Content = "Вход";
            label1.Content = "Писатели";
            search.Content = "Поиск";
            /*t1.Header = "Биография писателя";
            t2.Header = "Писательская работа";
            t3.Header = "Выпущенные книги";
            t4.Header = "Работы о писателе";
            t5.Header = "Переводы";
            t6.Header = "Публицистика";
            t7.Header = "Стихотворения ставшие песнями";
            t51.Header = "Перевод произведений писателя на другие языки";
            t52.Header = "Перевод других произведений на тувинский язык";
            t8.Header = "Галерея";
            t81.Header = "Иллюстрации к произведениям";
            t82.Header = "Фотографии";
            t83.Header = "Аудиоматериалы";
            t84.Header = "Видеоматериалы";
            t85.Header = "Презентация";*/
        }

        private void image4_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            main_img.Source = GetImage("logo_eng.png");
            button2.Content = "Writers";
            button3.Content = "Programs";
            button4.Content = "Support";
            button5.Content = "Log in";
            label1.Content = "Writers";
            search.Content = "Search";
            /*t1.Header = "Writer biography";
            t2.Header = "Literary work";
            t3.Header = "Issued books";
            t4.Header = "The works about the writer";
            t5.Header = "Translations";
            t6.Header = "Publicism";
            t7.Header = "The poems become songs";
            t51.Header = "Translation of the writer's works into other languages";
            t52.Header = "Translation of other works in the Tuvan language";
            t8.Header = "Gallery";
            t81.Header = "Illustrations to the works";
            t82.Header = "Photographs";
            t83.Header = "Audio materials";
            t84.Header = "Video materials";
            t85.Header = "Presentation";*/
        }


        private void TreeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            //t1.FontStyle = FontStyles.Oblique;
        }

        private void TreeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
           // t1.FontStyle = FontStyles.Normal;
        }
        private void t1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
           // richTextBox3.Visibility = Visibility.Visible;
            string str = "select ws_biog from all_writers where ws_id = " + glob_autor;
            string biog = "";
            using (OleDbConnection con = new OleDbConnection(connectionstring))
            {
                OleDbCommand com = new OleDbCommand(str, con);
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    biog = reader.GetValue(0).ToString();
                }
                reader.Close();
                con.Close();
            }

           /* TextRange range;
            FileStream fStream;
            if (File.Exists(path + biog))
            {
                range = new TextRange(this.richTextBox3.Document.ContentStart, this.richTextBox3.Document.ContentEnd);
                fStream = new FileStream(path + biog, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }*/

        }
        private void t2_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < 5; i++) a[i].Visibility = Visibility.Hidden;
            vis();
          //  richTextBox3.Visibility = Visibility.Visible;
            string str = "select ws_piswork from all_writers where ws_id =" + glob_autor;
            string biog = "";
            using (OleDbConnection con = new OleDbConnection(connectionstring))
            {
                OleDbCommand com = new OleDbCommand(str, con);
                con.Open();
                OleDbDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    biog = reader.GetValue(0).ToString();
                }
                reader.Close();
                con.Close();
            }

          /*  TextRange range;
            FileStream fStream;
            if (File.Exists(path + biog))
            {
                range = new TextRange(this.richTextBox3.Document.ContentStart, this.richTextBox3.Document.ContentEnd);
                fStream = new FileStream(path + biog, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.Rtf);
                fStream.Close();
            }*/
        }

        private void t4_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vis_datagrid(dataGrid2);
            string str = "select ab_id as Номер, ab_name as Название_работы,ab_wr as Кто_написал, ab_book as Издательство, ab_date as Дата_выпуска from about_writer where ab_ws= " + glob_autor;
            OleDbConnection con = new OleDbConnection(connectionstring);
            con.Open();
            OleDbCommand com = new OleDbCommand(str, con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(str, con);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "about_writer");
            dataGrid2.ItemsSource = ds.Tables["about_writer"].DefaultView;
            con.Close();
        }

        private void t52_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vis_datagrid(dataGrid3);
            string str = "select pt_id as Номер, pt_pis as Писатель, pt_name as Название_произведения, pt_tema as Жанр, pt_city as Город_издания, pt_god as Год_издания, pt_izdatelstvo as Издательство, pt_str as Количество_страниц from per_tuv where pt_ws=" + glob_autor;
            OleDbConnection con = new OleDbConnection(connectionstring);
            con.Open();
            OleDbCommand com = new OleDbCommand(str, con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(str, con);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "per_tuv");
            dataGrid3.ItemsSource = ds.Tables["per_tuv"].DefaultView;
            con.Close();
        }

        private void t51_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vis_datagrid(dataGrid4);
            string str = "select po_id as Номер, po_name as Название_произведения, po_tema as Жанр, po_pis as Перевод, po_god as Год_издания, po_izdat as Издательство, po_str as Количество_страниц from per_other where po_ws =" + glob_autor;
            OleDbConnection con = new OleDbConnection(connectionstring);
            con.Open();
            OleDbCommand com = new OleDbCommand(str, con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(str, con);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "per_other");
            dataGrid4.ItemsSource = ds.Tables["per_other"].DefaultView;
            con.Close();
        }

        private void t7_PreviewMouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            vis_datagrid(dataGrid5);
            string str = "select sp_id as Номер, sp_name as Название_произведения, sp_singer as Мелодия from stih_pes where sp_ws = " + glob_autor;
            OleDbConnection con = new OleDbConnection(connectionstring);
            con.Open();
            OleDbCommand com = new OleDbCommand(str, con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(str, con);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "stih_pes");
            dataGrid5.ItemsSource = ds.Tables["stih_pes"].DefaultView;
            con.Close();
        }

        private void t3_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vis_datagrid(dataGrid1);
            string str = "select b_id as Номер, b_name as Название_книги,b_city as Город_издания, b_izdat as Издательство, b_year as Год_выпуска,b_str_count as Количество_страниц from books where b_wr=" + glob_autor;
            OleDbConnection con = new OleDbConnection(connectionstring);
            con.Open();
            OleDbCommand com = new OleDbCommand(str, con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(str, con);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "books");
            dataGrid1.ItemsSource = ds.Tables["books"].DefaultView;
            con.Close();

        }

        private void t2_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            //t2.FontStyle = FontStyles.Oblique;
        }

        private void t2_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
           // t2.FontStyle = FontStyles.Normal;
        }

        private void t3_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
          //  t3.FontStyle = FontStyles.Oblique;
        }

        private void t3_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
          //  t3.FontStyle = FontStyles.Normal;
        }

        private void t4_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
           // t4.FontStyle = FontStyles.Oblique;
        }

        private void t4_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
         //   t4.FontStyle = FontStyles.Normal;
        }

        private void t5_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
           // t5.FontStyle = FontStyles.Oblique;
        }

        private void t5_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
           // t5.FontStyle = FontStyles.Normal;
        }

        private void t51_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
          //  t51.FontStyle = FontStyles.Oblique;
        }

        private void t51_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
           // t51.FontStyle = FontStyles.Normal;
        }

        private void t52_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
          //  t52.FontStyle = FontStyles.Oblique;
        }

        private void t52_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
          //  t52.FontStyle = FontStyles.Normal;
        }

        private void t6_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            //t6.FontStyle = FontStyles.Oblique;
        }

        private void t6_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
          //  t6.FontStyle = FontStyles.Normal;
        }

        private void t7_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
          //  t7.FontStyle = FontStyles.Oblique;
        }

        private void t7_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
          //  t7.FontStyle = FontStyles.Normal;
        }

        private void t8_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
           // t8.FontStyle = FontStyles.Oblique;
        }

        private void t8_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
          //  t8.FontStyle = FontStyles.Normal;
        }

        private void t81_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
          //  t81.FontStyle = FontStyles.Oblique;
        }

        private void t81_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
           // t81.FontStyle = FontStyles.Normal;
        }

        private void t82_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
          //  t82.FontStyle = FontStyles.Oblique;
        }

        private void t82_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
          //  t82.FontStyle = FontStyles.Normal;
        }

        private void t83_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
         //   t83.FontStyle = FontStyles.Oblique;
        }

        private void t83_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
          //  t83.FontStyle = FontStyles.Normal;
        }

        private void t84_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
           // t84.FontStyle = FontStyles.Oblique;
        }

        private void t84_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
           // t84.FontStyle = FontStyles.Normal;
        }

        private void t85_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
           // t85.FontStyle = FontStyles.Oblique;
        }

        private void t85_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
           // t85.FontStyle = FontStyles.Normal;
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            pass p = new pass();
            p.Show();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            filewrite fw = new filewrite();
            fw.Show();
        }
    
    }
}

