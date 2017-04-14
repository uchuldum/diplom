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
        static string autor_folder = "";
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
                stp.VerticalAlignment = VerticalAlignment.Center;
                
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
                stp.Height = 40;
                
                TextBlock txb = new TextBlock();
                txb.VerticalAlignment = VerticalAlignment.Center;
                txb.TextWrapping = TextWrapping.Wrap;
                txb.Width = stp.Width;
                txb.FontSize = 16;
                txb.Text = menu_item[i];
                Menu.Children.Add(stp);
                stp.Children.Add(txb);
                stp.PreviewMouseUp += new MouseButtonEventHandler(stp_MouseUp);
                stp.MouseEnter += new MouseEventHandler(stP_MouseEnter);
                stp.MouseLeave += new MouseEventHandler(stP_MouseLeave2);
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
                case 4: raboty_about(glob_autor);
                    break;
                case 5:
                    break;
                case 6:perevod_other(glob_autor);
                    break;
                case 7:perevod_tuv(glob_autor);
                    break;
                case 8:
                    break;
                case 9:stih_pes(glob_autor);
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
                if (File.Exists(path + "pisateli\\" + autor_folder + "\\biographia.pdf"))
                {
                    webbrowser.Visibility = Visibility.Visible;
                    webbrowser.Navigate(path + "pisateli\\" + autor_folder + "\\biographia.pdf");///Вывод биографии писателя
                }
                else
                {
                    webbrowser.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Биографию еще не внесли");
                }
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
                if (File.Exists(path + "pisateli\\" + autor_folder + "\\pisatel_rab.pdf"))
                {
                    webbrowser.Visibility = Visibility.Visible;
                    webbrowser.Navigate(path + "pisateli\\" + autor_folder + "\\pisatel_rab.pdf");///Вывод биографии писателя
                }
                else
                {
                    webbrowser.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Писательскую работу еще не внесли");
                }
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
            TVMenu.Items.Clear();
            noname.Children.Clear();
            try
            {
                noname.Visibility = Visibility.Visible;
                webbrowser.Visibility = Visibility.Collapsed;
                int count = Convert.ToInt32(con.ExecuteScalar("select count (*) from nomnary where nomnary_uid = " + id));
                string[] kniga_name = con.Reader_Array("select nomnary_title from nomnary where nomnary_uid = " + id+ " order by nomnary_title", count);
                string[] kniga_id = con.Reader_Array("select nomnary_id from nomnary where nomnary_uid = " + id + " order by nomnary_title", count);
                string[] kniga_janr = con.Reader_Array("select nomnary_zhanr from nomnary where nomnary_uid = " + id + " order by nomnary_title", count);
                string[] kniga_date = con.Reader_Array("select nomnary_god from nomnary where nomnary_uid = " + id + " order by nomnary_title", count);
                string[] kniga_tipograph = con.Reader_Array("select nomnary_tipograph from nomnary where nomnary_uid = " + id + " order by nomnary_title", count);
                string[] kniga_str = con.Reader_Array("select nomnary_stranisy from nomnary where nomnary_uid = " + id + " order by nomnary_title", count);
                string[] kniga_text = con.Reader_Array("select nomnary_text from nomnary where nomnary_uid = " + id + " order by nomnary_title", count);
                for (int i=0;i<count;i++)
                {
                    TreeViewItem TVitem = new TreeViewItem();
                    TVMenu.Items.Add(TVitem);
                }
                int k = 0;
                foreach(TreeViewItem item in TVMenu.Items)
                {
                    item.Header = (k+1)+". "+kniga_name[k]+". "+kniga_name[k]+" "+kniga_janr[k]+" .- "+kniga_tipograph[k]+", "+kniga_date[k];
                    item.Tag = kniga_id[k];
                    item.MouseEnter += new MouseEventHandler(tv_item_MouseEnter);
                    item.MouseLeave += new MouseEventHandler(tv_item_MouseLeave);
                    item.PreviewMouseUp += new MouseButtonEventHandler(item_MouseUp);
                    k++;
                }
                TextBlock text = new TextBlock();
                text.Text = "Парлаттынып үнген чогаалдары";
                text.FontSize = 16;
                text.Margin = new Thickness(0, 0, 0, 10);
                noname.Children.Add(text);
                noname.Children.Add(TVMenu);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        private void tv_item_MouseLeave(object sender, System.EventArgs e)
        {
            foreach (TreeViewItem item1 in TVMenu.Items)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                item1.Foreground = Brushes.Black;
            }
            foreach (TreeViewItem item1 in TVMenu2.Items)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                item1.Foreground = Brushes.Black;
            }
        }
        private void tv_item_MouseEnter(object sender, System.EventArgs e)
        {
            TreeViewItem tvItem = (TreeViewItem)sender;
            Mouse.OverrideCursor = Cursors.Hand;
            tvItem.Foreground = Brushes.Pink;
        }
       
        private void item_MouseUp(object sender, EventArgs e)
        {
            TreeViewItem Item = (TreeViewItem)sender;
            try
            {
                int nom_id = Convert.ToInt32(Item.Tag);
                string nom_name = con.Reader("select nomnary_text from nomnary where nomnary_id = "+nom_id);
                string localpath = path + "pisateli\\" + autor_folder + "\\nomnary\\" + nom_name;
                if (File.Exists(localpath))
                {
                    noname.Visibility = Visibility.Collapsed;
                    webbrowser.Visibility = Visibility.Visible;
                    webbrowser.Navigate(localpath);
                }
                else
                {
                    MessageBox.Show("Книги пока нет");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

        //////

        /////Работы о писателе
        public void raboty_about(string id)
        {
            webbrowser.Visibility = Visibility.Collapsed;
            TVMenu.Items.Clear();
            noname.Children.Clear();
            noname.Visibility = Visibility.Visible;
            try
            {
                /////DESIGN
                TextBlock azhyldar = new TextBlock();
                azhyldar.Text = "Чогаалчының дугайында ажылдар";
                azhyldar.FontSize = 20;
                azhyldar.Margin = new Thickness(0, 0, 0, 5);
                TextBlock tyvatyl = new TextBlock();
                tyvatyl.Text = "Тыва дылда";
                tyvatyl.FontSize = 16;
                TextBlock orustyl = new TextBlock();
                orustyl.Text = "Орус дылда";
                orustyl.FontSize = 16;
                orustyl.Margin = new Thickness(0, 0, 0, 5);
                ////ACTION
                noname.Children.Add(azhyldar);
                noname.Children.Add(tyvatyl);
                /////TUV LANGUAGE
                int count = Convert.ToInt32(con.ExecuteScalar("select count (*) from chog_dug_azhyldar where chdugazh_uid = " + id+" and chdugazh_log like 'tyv'"));
                string[] kniga = con.Reader_Array("select chdugazh_id from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'tyv'order by chdugazh_avtor", count);
                string[] autor = con.Reader_Array("select chdugazh_avtor from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'tyv'order by chdugazh_avtor", count);
                string[] title = con.Reader_Array("select chdugazh_title from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'tyv'order by chdugazh_avtor", count);
                string[] istochnik = con.Reader_Array("select chdugazh_istochnik from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'tyv'order by chdugazh_avtor", count);
                string[] year = con.Reader_Array("select chdugazh_god from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'tyv'order by chdugazh_avtor", count);
                for (int i = 0; i < count; ++i)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = (i+1)+". "+autor[i]+". "+title[i]+"// "+istochnik[i]+". - "+year[i]+".";
                    TVMenu.Items.Add(item);
                    item.Tag = kniga[i];
                    item.MouseEnter += new MouseEventHandler(tv_item_MouseEnter);
                    item.MouseLeave += new MouseEventHandler(tv_item_MouseLeave);
                    item.PreviewMouseUp += (s, e) =>
                    {
                        int localid = Convert.ToInt32(item.Tag);
                        string localtext = con.Reader("select chdugazh_text from chog_dug_azhyldar where chdugazh_id = "+localid);
                        string localpath = path + "pisateli\\" + autor_folder + "\\chog_dug_azhyldary\\" + localtext + ".pdf";
                        if (File.Exists(localpath))
                        {
                            noname.Visibility = Visibility.Collapsed;
                            webbrowser.Visibility = Visibility.Visible;
                            webbrowser.Navigate(localpath);
                        }
                        else
                        {
                            MessageBox.Show("Пока нет работ");
                        }
                    };
                }
                noname.Children.Add(TVMenu);
                TVMenu.Margin = new Thickness(0, 0, 0, 5);
                noname.Children.Add(orustyl);
                ///////ORUS LANGUAGE
                count = Convert.ToInt32(con.ExecuteScalar("select count (*) from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'rus'"));
                kniga = con.Reader_Array("select chdugazh_id from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'rus'order by chdugazh_avtor", count);
                autor = con.Reader_Array("select chdugazh_avtor from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'rus'order by chdugazh_avtor", count);
                title = con.Reader_Array("select chdugazh_title from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'rus'order by chdugazh_avtor", count);
                istochnik = con.Reader_Array("select chdugazh_istochnik from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'rus'order by chdugazh_avtor", count);
                year = con.Reader_Array("select chdugazh_god from chog_dug_azhyldar where chdugazh_uid = " + id + " and chdugazh_log like 'rus'order by chdugazh_avtor", count);
                for (int i = 0; i < count; ++i)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = (i + 1) + ". " + autor[i] + ". " + title[i] + "// " + istochnik[i] + ". - " + year[i] + ".";
                    TVMenu2.Items.Add(item);
                    item.Tag = kniga[i];
                    item.MouseEnter += new MouseEventHandler(tv_item_MouseEnter);
                    item.MouseLeave += new MouseEventHandler(tv_item_MouseLeave);
                    item.PreviewMouseUp += (s, e) =>
                    {
                        int localid = Convert.ToInt32(item.Tag);
                        string localtext = con.Reader("select chdugazh_text from chog_dug_azhyldar where chdugazh_id = " + localid);
                        string localpath = path + "pisateli\\" + autor_folder + "\\chog_dug_azhyldary\\" + localtext + ".pdf";
                        if (File.Exists(localpath))
                        {
                            noname.Visibility = Visibility.Collapsed;
                            webbrowser.Visibility = Visibility.Visible;
                            webbrowser.Navigate(localpath);
                        }
                        else MessageBox.Show("Пока нет работ");
                    };
                }
                noname.Children.Add(TVMenu2);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
       
        //////

        /////Переводы работ писателя на другие языки
        public void perevod_other(string id)
        {
            webbrowser.Visibility = Visibility.Collapsed;
            noname.Children.Clear();
            TVMenu.Items.Clear();
            TVMenu2.Items.Clear();
            noname.Visibility = Visibility.Visible;
            try
            {
                ////DESIGN
                TextBlock perevod = new TextBlock();
                perevod.Text = "Чогаалчының ажылдарының өске дылдарже очулгазы";
                perevod.FontSize = 20;
                perevod.Margin = new Thickness(0, 0, 0, 5);
                noname.Children.Add(perevod);
                //////ACTION
                int count = con.ExecuteScalar("select count (*) from ochulgalary where ochulgalary_uid = " + id + " and ochulgalary_log like 'oth'");
                string[] kniga = con.Reader_Array("select ochulgalary_id from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'oth' order by ochulgalary_title", count);
                string[] title = con.Reader_Array("select ochulgalary_title from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'oth' order by ochulgalary_title", count);
                string[] perevodchik = con.Reader_Array("select ochulgalary_perevodchik from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'oth' order by ochulgalary_title", count);
                string[] tipograph = con.Reader_Array("select ochulgalary_tipograph from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'oth' order by ochulgalary_title", count);
                string[] year = con.Reader_Array("select ochulgalary_god from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'oth' order by ochulgalary_title", count);
                for (int i = 0; i < count; ++i)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = (i + 1) + ". " + title[i] + ":/ " + perevodchik[i] + " - " + tipograph[i] + ", " + year[i] + ".";
                    TVMenu.Items.Add(item);
                    item.Tag = kniga[i];
                    item.MouseEnter += new MouseEventHandler(tv_item_MouseEnter);
                    item.MouseLeave += new MouseEventHandler(tv_item_MouseLeave);
                    item.PreviewMouseUp += (s, e) =>
                    {
                        int localid = Convert.ToInt32(item.Tag);
                        string localtext = con.Reader("select ochulgalary_text from ochulgalary where ochulgalary_id = " + localid);
                        string localpath = path + "pisateli\\" + autor_folder + "\\perevod_other\\" + localtext + ".pdf";
                        if (File.Exists(localpath))
                        {
                            noname.Visibility = Visibility.Collapsed;
                            webbrowser.Visibility = Visibility.Visible;
                            webbrowser.Navigate(localpath);
                        }
                        else
                        {
                            MessageBox.Show("Пока нет работ");
                        }
                    };
                }
                noname.Children.Add(TVMenu);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        //////

        /////Переводы с других языков на тувинский язык
        public void perevod_tuv(string id)
        {
            webbrowser.Visibility = Visibility.Collapsed;
            noname.Children.Clear();
            TVMenu.Items.Clear();
            TVMenu2.Items.Clear();
            noname.Visibility = Visibility.Visible;
            try
            {
                ////DESIGN
                TextBlock perevod = new TextBlock();
                perevod.Text = "Өске чогаалдарны тыва дылче чогаалчының очулдурганы";
                perevod.FontSize = 20;
                perevod.Margin = new Thickness(0, 0, 0, 5);
                noname.Children.Add(perevod);
                //////ACTION
                int count = con.ExecuteScalar("select count (*) from ochulgalary where ochulgalary_uid = " + id + " and ochulgalary_log like 'tyv'");
                string[] kniga = con.Reader_Array("select ochulgalary_id from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'tyv' order by ochulgalary_autor", count);
                string[] autor = con.Reader_Array("select ochulgalary_autor from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'tyv' order by ochulgalary_autor", count);
                string[] title = con.Reader_Array("select ochulgalary_title from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'tyv' order by ochulgalary_autor", count);
                string[] perevodchik = con.Reader_Array("select ochulgalary_perevodchik from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'tyv' order by ochulgalary_autor", count);
                string[] tipograph = con.Reader_Array("select ochulgalary_tipograph from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'tyv' order by ochulgalary_autor", count);
                string[] year = con.Reader_Array("select ochulgalary_god from ochulgalary where ochulgalary_uid = " + id + "  and ochulgalary_log like 'tyv' order by ochulgalary_autor", count);
                for (int i = 0; i < count; ++i)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = (i + 1) + ". "+autor[i]+" " + title[i] + ":/ " + perevodchik[i] + " - " + tipograph[i] + ", " + year[i] + ".";
                    TVMenu.Items.Add(item);
                    item.Tag = kniga[i];
                    item.MouseEnter += new MouseEventHandler(tv_item_MouseEnter);
                    item.MouseLeave += new MouseEventHandler(tv_item_MouseLeave);
                    item.PreviewMouseUp += (s, e) =>
                    {
                        int localid = Convert.ToInt32(item.Tag);
                        string localtext = con.Reader("select ochulgalary_text from ochulgalary where ochulgalary_id = " + localid);
                        string localpath = path + "pisateli\\" + autor_folder + "\\perevod_tuv\\" + localtext + ".pdf";
                        if (File.Exists(localpath))
                        {
                            noname.Visibility = Visibility.Collapsed;
                            webbrowser.Visibility = Visibility.Visible;
                            webbrowser.Navigate(localpath);
                        }
                        else
                        {
                            MessageBox.Show("Пока нет работ");
                        }
                    };
                }
                noname.Children.Add(TVMenu);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        //////

        /////Публицистика
        //////

        /////Стихотворения ставшие песнями
        public void stih_pes(string id)
        {
            webbrowser.Visibility = Visibility.Collapsed;
            noname.Children.Clear();
            TVMenu.Items.Clear();
            TVMenu2.Items.Clear();
            noname.Visibility = Visibility.Visible;
            try
            {
                ////DESIGN
                TextBlock perevod = new TextBlock();
                perevod.Text = "Ыры апарган шүлүктериниң сөзүлелдери";
                perevod.FontSize = 20;
                perevod.Margin = new Thickness(0, 0, 0, 5);
                TextBlock perevod1 = new TextBlock();
                perevod1.Text = "Ыры апарган шүлүктериниң даңзызы";
                perevod1.FontSize = 16;
                perevod1.Margin = new Thickness(0, 0, 0, 5);
                noname.Children.Add(perevod);
                noname.Children.Add(perevod1);
                //////ACTION
                int count = con.ExecuteScalar("select count (*) from yry_apargan_shulukteri where yry_apsh_uid = " + id);
                string[] yry_id = con.Reader_Array("select yry_apsh_id from yry_apargan_shulukteri where yry_apsh_uid  = " + id + "", count);
                string[] yry_music = con.Reader_Array("select  yry_apsh_muz from yry_apargan_shulukteri where yry_apsh_uid  = " + id + "", count);
                string[] yry_title = con.Reader_Array("select  yry_apsh_title from yry_apargan_shulukteri where yry_apsh_uid  = " + id + "", count);
                string[] yry_year = con.Reader_Array("select  yry_apsh_god from yry_apargan_shulukteri where yry_apsh_uid  = " + id + "", count);
                for (int i = 0; i < count; ++i)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = (i + 1) + ". " + yry_title[i] + "-" + yry_music[i]; 
                    TVMenu.Items.Add(item);
                    item.Tag = yry_id[i];
                    item.MouseEnter += new MouseEventHandler(tv_item_MouseEnter);
                    item.MouseLeave += new MouseEventHandler(tv_item_MouseLeave);
                    item.PreviewMouseUp += (s, e) =>
                    {
                        int localid = Convert.ToInt32(item.Tag);
                        string localtext = con.Reader("select yry_apsh_text from yry_apargan_shulukteri where yry_apsh_id = " + localid);
                        string localpath = path + "pisateli\\" + autor_folder + "\\pesni\\" + localtext + ".pdf";
                        if (File.Exists(localpath))
                        {
                            noname.Visibility = Visibility.Collapsed;
                            webbrowser.Visibility = Visibility.Visible;
                            webbrowser.Navigate(localpath);
                        }
                        else
                        {
                            MessageBox.Show("Пока нет работ");
                        }
                    };
                }
                noname.Children.Add(TVMenu);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
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
                    //map_pol.Fill = Brushes.Blue;
                    var brush = new SolidColorBrush(Color.FromArgb(255, 0, 150, 211));
                    map_pol.Fill = brush;
                    map_pol.StrokeThickness = 1;
                    string a = con.Reader("select coords from region where region_id = " + (i + 1).ToString());
                    map_pol.Tag = con.Reader("select r_name from region where region_id = " + (i + 1).ToString());
                    string centr = con.Reader("select r_links from region where region_id = " + (i + 1).ToString());
                    //map_pol.Name = ;
                    string[] split = a.Split(new Char[] { ',' });
                    string[] centr_arr = centr.Split(new Char[] { ',' });

                    int centr_X = Convert.ToInt32(centr_arr[0]);
                    int centr_Y = Convert.ToInt32(centr_arr[1]);

                    Ellipse el = new Ellipse();
                    el.Width = 10;
                    el.Height = 10;
                    el.StrokeThickness = 2;
                    el.Stroke = Brushes.Black;
                    el.Fill = Brushes.White;
                    if (centr_X == 547 || centr_X == 514 || centr_X == 397 || centr_X == 270 || centr_X == 172)
                    {
                        el.Width = 15;
                        el.Height = 15;
                        el.StrokeThickness = 3;
                    }
                    int k = split.Length;
                    for (int j = 0; j < k - 1; j = j + 2)
                    {
                        int x = Convert.ToInt32(split[j]);
                        int y = Convert.ToInt32(split[j + 1]);
                        map_pol.Points.Add(new Point(x, y));
                    }

                    Map.Children.Add(map_pol);
                    Map.Children.Add(el);
                    Canvas.SetLeft(el, centr_X);
                    Canvas.SetTop(el, centr_Y);
                    map_pol.MouseEnter += new MouseEventHandler(item_MouseEnter);
                    map_pol.MouseMove += new MouseEventHandler(item_MouseMove);
                    map_pol.MouseLeave += new MouseEventHandler(item_MouseLeave);
                    map_pol.PreviewMouseUp += new MouseButtonEventHandler(item_mouseUp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }

        }
        private void item_MouseMove(object sender, EventArgs e)
        {
            RoundedCornersPolygon p = (RoundedCornersPolygon)sender;
            Point position = Mouse.GetPosition(Map);
            TextBlock text = new TextBlock();
            text.Text = p.Tag.ToString();
            text.Name = "text_d";
            text.FontFamily = new FontFamily("Comic Sans MS");
            text.Foreground = Brushes.White;
            int length = text.Text.Length;
            Canvas.SetLeft(text, (position.X + 22));
            Canvas.SetTop(text, (position.Y + 25));
            Rectangle rect = new Rectangle();
            rect.StrokeThickness = 1;
            rect.Stroke = Brushes.Black;
            rect.Fill = Brushes.Black;
            rect.RadiusX = 5;
            rect.RadiusY = 5;
            rect.Name = "distr";
            rect.Width = length * 8;
            rect.Height = 30;
            Canvas.SetLeft(rect, (position.X + 20));
            Canvas.SetTop(rect, (position.Y + 20));
            var rectangle = (UIElement)LogicalTreeHelper.FindLogicalNode(Map, "distr"); ////ОЧИСТКА ПРЕДЫДУЩЕГО
            var text_dist = (UIElement)LogicalTreeHelper.FindLogicalNode(Map, "text_d");
            Map.Children.Remove(rectangle);
            Map.Children.Remove(text_dist);
            Map.Children.Add(rect);
            Map.Children.Add(text);
        }
        private void item_MouseEnter(object sender, System.EventArgs e)
        {
            RoundedCornersPolygon p = (RoundedCornersPolygon)sender;
            Mouse.OverrideCursor = Cursors.Hand;
            p.Fill = Brushes.Yellow;
        }
        private void item_MouseLeave(object sender, System.EventArgs e)
        {
            var rectangle = (UIElement)LogicalTreeHelper.FindLogicalNode(Map, "distr"); ////ОЧИСТКА ПРЕДЫДУЩЕГО
            var text_dist = (UIElement)LogicalTreeHelper.FindLogicalNode(Map, "text_d");
            Map.Children.Remove(rectangle);
            Map.Children.Remove(text_dist);
            RoundedCornersPolygon p = (RoundedCornersPolygon)sender;
            Mouse.OverrideCursor = Cursors.Arrow;
            var brush = new SolidColorBrush(Color.FromArgb(255, 0, 150, 211));
            p.Fill = brush;
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
            var text = stp.Children.OfType<TextBlock>().FirstOrDefault();
            text.FontStyle = FontStyles.Oblique;
            stp.Background = Brushes.LightBlue;
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void stP_MouseLeave(object sender, System.EventArgs e)
        {
            StackPanel stp = (StackPanel)sender;
            Mouse.OverrideCursor = Cursors.Arrow;
            var text = stp.Children.OfType<TextBlock>().FirstOrDefault();
            text.FontStyle = FontStyles.Normal;
            stp.Background = Brushes.LightSteelBlue;

        }
        private void stP_MouseLeave2(object sender, System.EventArgs e)
        {
            StackPanel stp = (StackPanel)sender;
            Mouse.OverrideCursor = Cursors.Arrow;
            var text = stp.Children.OfType<TextBlock>().FirstOrDefault();
            text.FontStyle = FontStyles.Normal;
            stp.Background = Brushes.White;

        }
        /////////////////////////////////////////////НАЖАТИЕ НА ПИСАТЕЛЯ В КОЖУУНАХ
        private void stP_MouseUp(object sender, System.EventArgs e)
        {

            StackPanel stP = (StackPanel)sender;
            pis_tuv.Visibility = Visibility.Collapsed;
            pisatel.Visibility = Visibility.Visible;
            stack_distr.Visibility = Visibility.Collapsed;
            main_tuv.Visibility = Visibility.Collapsed;
            glob_autor = stP.Tag.ToString().Trim();
            autor_folder = con.Reader("select folder from chogaalchy where chog_id = " + glob_autor);
            menu();
            biographia(glob_autor);

        }
        public static string path = System.AppDomain.CurrentDomain.BaseDirectory;
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
                string[] pis_id = con.Reader_Array("select chog_id from chogaalchy order by chog_fam", count);
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
                    stP2.Tag = pis_id[i];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
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
                                                                                                        ///////////ПОИСК ПИСАТЕЛЯ
        private void search_Click(object sender, RoutedEventArgs e)
        {
            main_tuv.Visibility = Visibility.Collapsed;
            stack_distr.Visibility = Visibility.Collapsed;
            pisatel.Visibility = Visibility.Collapsed;
            pis_tuv.Visibility = Visibility.Visible;

            writers2.Children.Clear();
            try
            {
                string distr_id = comboBox1.SelectedIndex.ToString().Trim();
                if (distr_id == "-1") MessageBox.Show("as");
                else
                { 
                    int count = con.ExecuteScalar("select count (*) from chogaalchy where chog_region_id = "+distr_id);
                    if (count > 0)
                    {
                        string[] pis_id = con.Reader_Array("select chog_id from chogaalchy  where chog_region_id = " + distr_id + " order by chog_fam", count);
                        string[] pis_surname = con.Reader_Array("select chog_fam from chogaalchy where chog_region_id = " + distr_id + " order by chog_fam", count);
                        string[] pis_name = con.Reader_Array("select chog_imya from chogaalchy where chog_region_id = " + distr_id + " order by chog_fam", count);
                        string[] pis_patronymic = con.Reader_Array("select chog_otch from chogaalchy where chog_region_id = " + distr_id + " order by chog_fam", count);
                        string[] pis_img = con.Reader_Array("select chog_photo from chogaalchy where chog_region_id = " + distr_id + " order by chog_fam", count);
                        for (int i = 0; i < count; ++i)
                        {
                            StackPanel stP2 = new StackPanel();
                            Image img2 = new Image();
                            TextBlock txtblk = new TextBlock();
                            if (File.Exists("all\\img\\" + pis_img[i])) img2.Source = GetImage2("all\\img\\" + pis_img[i]);
                            img2.HorizontalAlignment = HorizontalAlignment.Right;
                            txtblk.Text = pis_surname[i] + " " + pis_name[i] + " " + pis_patronymic[i];
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
                            stP2.Tag = pis_id[i];
                        }
                    }
                    else
                    {
                        StackPanel stP2 = new StackPanel();
                        TextBlock txtblk = new TextBlock();
                        txtblk.VerticalAlignment = VerticalAlignment.Center;
                        txtblk.Margin = new Thickness(5, 0, 0, 0);
                        txtblk.FontFamily = new FontFamily("Arial");
                        txtblk.FontSize = 16;
                        txtblk.Text = "Кожуунга хамаарышкан чогаалчы амдыызында чок-тур. Удавас долдуртуна бээр.";
                        stP2.Orientation = Orientation.Horizontal;
                        stP2.Background = Brushes.LightSteelBlue;
                        stP2.MaxWidth = 1024;
                        stP2.MinWidth = 800;
                        stP2.Height = 100;
                        stP2.Width = writers2.Width;
                        stP2.Margin = new Thickness(0, 20, 0, 0);
                        writers2.Children.Add(stP2);
                        stP2.Children.Add(txtblk);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            
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

