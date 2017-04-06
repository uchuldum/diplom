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
            string[] menu_item_parent = con.Reader_Array("select menu_id from menu  where menuparent_id<>0", 7);
            string[] menu_item = con.Reader_Array("select menu_name from menu ", 15);
            string[] menu_id = con.Reader_Array("select menu_id from menu", 15);

            Menu.Children.Clear();

            //string[] menu_item = new string[count];
            for (int i=0;i<15;++i)
            {
                StackPanel stp = new StackPanel();
                
                stp.Background = Brushes.LightBlue;
                stp.Margin = new Thickness(0,0,0,10);
                stp.Width = Menu.Width;
                stp.Tag = menu_id[i].Trim();
                
                foreach(var a in menu_item_parent)
                {
                    if(stp.Tag.ToString() == a)
                    {
                        stp.Margin = new Thickness(20, 0, 0, 10);
                    }
                }
                stp.Height = 15;
                TextBlock txb = new TextBlock();
                txb.Width = stp.Width;
                txb.Text = menu_item[i];
                Menu.Children.Add(stp);
                stp.Children.Add(txb);
                stp.PreviewMouseUp += new MouseButtonEventHandler(stp_MouseUp);
                stp.MouseEnter += new MouseEventHandler(stP_MouseEnter);
                stp.MouseLeave += new MouseEventHandler(stP_MouseLeave);
            }
        }
        private void stp_MouseUp(object sender, System.EventArgs e)
        {
            StackPanel stp = (StackPanel)sender;
            int number = Convert.ToInt32(stp.Tag);
            
            // MessageBox.Show(number + "");
            switch (number)
            {
                case 1: biographia(glob_autor);
                    break;
                case 2: pisatelskaya_rabota(glob_autor);
                    break;
                case 3:knigi(glob_autor);
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;
            }
        }

                                          ////////// ФУНКЦИИ ДЛЯ МЕНЮ
        /////БИОГРАФИЯ Писателя
        public void biographia(string id)
        {
            try
            {
                webbrowser.Visibility = Visibility.Visible;
                string biographia = con.Reader("select chog_biografiya from chogaalchy where chog_id =" + id);
                webbrowser.Navigate(path + biographia);///Вывод биографии писателя
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }
        //////

        /////Писательская работа
        public void pisatelskaya_rabota(string id)
        {
            try
            {
                webbrowser.Visibility = Visibility.Visible;
                string pis_rab = con.Reader("select azhyldary_text from azhyldary where azhyldary_uid =" + id);
                webbrowser.Navigate(path+"pisateli\\"+pis_rab+"\\pisatel_rab.pdf");///Вывод биографии писателя
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        //////

        /////Выпущенные книги
        public void knigi(string id)
        {
            try
            {
                TVMenu.Visibility = Visibility.Visible;
                webbrowser.Visibility = Visibility.Collapsed;
                int count = Convert.ToInt32(con.ExecuteScalar("select count (*) from nomnary where nomnary_uid = " + glob_autor));
                string[] kniga_name = con.Reader_Array("select nomnary_title from nomnary where nomnary_uid = " + glob_autor+ " order by nomnary_title", count);
                string[] kniga_id = con.Reader_Array("select nomnary_id from nomnary where nomnary_uid = " + glob_autor + " order by nomnary_title", count);
                string[] kniga_janr = con.Reader_Array("select nomnary_zhanr from nomnary where nomnary_uid = " + glob_autor + " order by nomnary_title", count);
                string[] kniga_date = con.Reader_Array("select nomnary_god from nomnary where nomnary_uid = " + glob_autor + " order by nomnary_title", count);
                string[] kniga_tipograph = con.Reader_Array("select nomnary_tipograph from nomnary where nomnary_uid = " + glob_autor + " order by nomnary_title", count);
                string[] kniga_str = con.Reader_Array("select nomnary_stranisy from nomnary where nomnary_uid = " + glob_autor + " order by nomnary_title", count);
                string[] kniga_text = con.Reader_Array("select nomnary_text from nomnary where nomnary_uid = " + glob_autor + " order by nomnary_title", count);
                for (int i=0;i<count;i++)
                {
                    TreeViewItem TVitem = new TreeViewItem();
                    TVMenu.Items.Add(TVitem);
                }
                int k = 0;
                foreach(TreeViewItem item in TVMenu.Items)
                {
                    item.Header = kniga_name[k]+". "+kniga_name[k]+" "+kniga_janr[k]+" .- "+kniga_tipograph[k]+", "+kniga_date[k];
                    item.Tag = kniga_id[k];



                    item.MouseEnter += new MouseEventHandler(tv_item_MouseEnter);
                    item.MouseLeave += new MouseEventHandler(tv_item_MouseLeave);
                    item.PreviewMouseUp += new MouseButtonEventHandler(item_MouseUp);
                    k++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        private void tv_item_MouseEnter(object sender, System.EventArgs e)
        {
            TreeViewItem tvItem = (TreeViewItem)sender;
            Mouse.OverrideCursor = Cursors.Hand;
            tvItem.Foreground = Brushes.Pink;
        }
        private void tv_item_MouseLeave(object sender, System.EventArgs e)
        {
            foreach (TreeViewItem item in TVMenu.Items)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                item.FontStyle = FontStyles.Normal;
                item.Foreground = Brushes.Black;
            }
        }
        private void item_MouseUp(object sender, EventArgs e)
        {
            TreeViewItem Item = (TreeViewItem)sender;
            MessageBox.Show(Item.Tag+"");
        }

        //////

        /////Работы о писателе
        //////

        /////Переводы работ писателя на другие языки
        //////

        /////Переводы с других языков на тувинский язык
        //////

        /////Публицистика
        //////

        /////Стихотворения ставшие песнями
        //////

        /////Иллюстрации к произведениям
        //////

        /////Фотографии
        //////

        /////Аудиоматериалы
        //////

        /////Видеоматериалы
        //////

        /////Презентация
        //////
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
                main_tuv.Visibility = Visibility.Visible;
                                                                              /////////////// КАРТА ////////
                for (int i = 0; i < 18; ++i)
                {
                    RoundedCornersPolygon map_pol = new RoundedCornersPolygon();
                    map_pol.ArcRoundness = 25;
                 //   Polygon map_pol = new Polygon();
                    map_pol.Stroke = Brushes.White;
                    map_pol.Fill = Brushes.Blue;
                    map_pol.StrokeThickness = 1;
                    string a = con.Reader("select coords from region where region_id = " + (i + 1).ToString());
                    map_pol.Tag = con.Reader("select r_name from region where region_id = " + (i + 1).ToString());
                    //map_pol.Name = ;
                    string[] split = a.Split(new Char[] { ',' });
                    int k = split.Length;
                    for (int j = 0; j < k - 1; j = j + 2)
                    {
                        int x = Convert.ToInt32(split[j]);
                        int y = Convert.ToInt32(split[j + 1]);
                        map_pol.Points.Add(new Point(x, y));
                    }
                    Map.Children.Add(map_pol);
                    map_pol.MouseEnter += new MouseEventHandler(item_MouseEnter);
                    map_pol.MouseLeave += new MouseEventHandler(item_MouseLeave);
                    map_pol.PreviewMouseUp += new MouseButtonEventHandler(item_mouseUp);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }

        }
        private void item_MouseEnter(object sender, System.EventArgs e)
        {
            RoundedCornersPolygon tvItem = (RoundedCornersPolygon)sender;
            Point position = Mouse.GetPosition(Map);
            Mouse.OverrideCursor = Cursors.Hand;
            tvItem.Fill = Brushes.Yellow;
        }
        private void item_MouseLeave(object sender, System.EventArgs e)
        {

            RoundedCornersPolygon tvItem = (RoundedCornersPolygon)sender;
            Mouse.OverrideCursor = Cursors.Arrow;
            tvItem.Fill = Brushes.Blue;
        }


        /////////СОБЫТИЕ НА НАЖИТИЕ КОЖУУНА
        private void item_mouseUp(object sender, System.EventArgs e)
        {
            main_tuv.Visibility = Visibility.Collapsed;
            RoundedCornersPolygon p = (RoundedCornersPolygon)sender;
            stack_distr.Visibility = Visibility.Visible;
            writers.Children.Clear();
            try
             {
                int region_id = Convert.ToInt32(con.Reader("select region_id from region where r_name like '" + p.Tag+"'"));
                string region_photo =  "\\all\\dist_img\\" + region_id+".png";
                string region_text = con.Reader("select r_info from region where region_id = " + region_id);
                 //////////////////Загрузили изображения кожуунов
                 distr_img.Source = GetImage2(region_photo);

                 /////Загрузили тексты кожуунов
                 dist_txt.Document.Blocks.Clear();
                 dist_txt.Document.Blocks.Add(new Paragraph(new Run(region_text)));



                 ////////Писатели В КОЖУУНАХ    

                 int count = con.ExecuteScalar("select count (*) from chogaalchy where chog_region_id=" + region_id); ///КОЛИЧЕСТВО ПИСАТЕЛЕЙ В ДАННОМ РЕГИОНЕ
                 if(count!=0)
                 for (int i = 0; i < count; ++i)
                 {
                     StackPanel stP = new StackPanel();
                     string[] pis_surname = con.Reader_Array("select chog_fam from chogaalchy where chog_region_id=" + region_id, count);
                     string[] pis_name = con.Reader_Array("select chog_imya from chogaalchy where chog_region_id=" + region_id, count);
                     string[] pis_patronymic = con.Reader_Array("select chog_otch from chogaalchy where chog_region_id=" + region_id, count);
                     string[] pis_img =con.Reader_Array("select chog_photo from chogaalchy where chog_region_id=" + region_id, count);
                     string[] pis_id = con.Reader_Array("select chog_id from chogaalchy where chog_region_id=" + region_id, count);

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
        //////Изменение курсора на STACKPANEL
        private void stP_MouseEnter(object sender, System.EventArgs e)
        {
            StackPanel stp = (StackPanel)sender;
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void stP_MouseLeave(object sender, System.EventArgs e)
        {
            StackPanel stp = (StackPanel)sender;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /////////////////////////////////////////////НАЖАТИЕ НА ПИСАТЕЛЯ В КОЖУУНАХ
        private void stP_MouseUp(object sender, System.EventArgs e)
        {
            StackPanel stP = (StackPanel)sender;
            pisatel.Visibility = Visibility.Visible;
            stack_distr.Visibility = Visibility.Collapsed;
            main_tuv.Visibility = Visibility.Collapsed;
            glob_autor = stP.Tag.ToString().Trim();
            menu();
            biographia(glob_autor);

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
                                                                         /////////////////НАЖАТИЕ НА КНОПКУ ПИСАТЕЛИ
        private void button2_Click(object sender, RoutedEventArgs e)
        {

            ////////VISIBILITY
            main_tuv.Visibility = Visibility.Collapsed;
            stack_distr.Visibility = Visibility.Collapsed;
            pisatel.Visibility = Visibility.Collapsed;

            pis_tuv.Visibility = Visibility.Visible;
           
            writers2.Children.Clear();
            try
            {
                ////////Писатель
                int count = con.ExecuteScalar("select count (*) from chogaalchy");
                string[] pis_surname = con.Reader_Array("select chog_fam from chogaalchy order by chog_fam", count);
                string[] pis_name = con.Reader_Array("select chog_imya from chogaalchy order by chog_fam", count);
                string[] pis_patronymic = con.Reader_Array("select chog_otch from chogaalchy order by chog_fam", count);
                string[] pis_img = con.Reader_Array("select chog_photo from chogaalchy order by chog_fam", count);

                for (int i = 0; i < count; ++i)
                {
                    
                    StackPanel stP2 = new StackPanel();
                    Image img2 = new Image();
                    TextBlock txtblk = new TextBlock();
                    if(File.Exists("all\\img\\"+pis_img[i]))  img2.Source = GetImage2("all\\img\\"+pis_img[i]);
                    img2.HorizontalAlignment = HorizontalAlignment.Right;
                    txtblk.Text = pis_surname[i]+" "+pis_name[i]+" "+pis_patronymic[i];
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
                    stP2.Tag = i+1;
                }
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

