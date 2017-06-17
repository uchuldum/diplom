using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

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
        static bool Open = false;
        static string Writer_Photo;
        static string pis_image = "";
        static int Menu_Admin;
        static bool ChangeBtn = false;
        ////////УДАЛЕНИЕ ДОБАВЛЕНИЕ ИЗМЕНЕНИЕ МАТЕРИАЛА В БД
        Button nazad = new Button();
        Button btn = new Button();
        static string deleteme = "";
        public void Delete_File(string Table,string Pole,int id,string Folder, int add)
        {
            try
            {
               string kniganame = con.Reader("select " + Pole + "_title from "+Table+" where " + Pole + "_id = " + id);
               string knigapath = con.Reader("select " + Pole + "_text from "+Table+" where " + Pole + "_id = " + id);
                //////УДАЛЕНИЕ
                if (add == 1)
                {
                    MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить \"" + kniganame + "\"?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            {
                                if (File.Exists(folder_path + Folder + "\\" + kniganame))
                                {
                                    File.Delete(folder_path + Folder + "\\" + kniganame);
                                }
                                con.ExecuteNonQuery("delete from " + Table + " where " + Pole + "_id = " + id);
                                MessageBox.Show("Удалено.");
                            }
                            break;
                        case MessageBoxResult.No: break;
                    }
                }
                //////ДОБАВЛЕНИЕ
                else if (add == 2)
                {
                    MessageBoxResult result = MessageBox.Show("Вы хотите добавить материал \"" + kniganame + "\"?", "Добавление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            {
                                string s = "";
                                string _output = "";
                                OpenFileDialog ofd = new OpenFileDialog();
                                ofd.InitialDirectory = @"C:\";
                                ofd.Title = "as";
                                ofd.Filter = "Pdf Files|*.pdf";
                                if(ofd.ShowDialog() == true)
                                {
                                    s = ofd.FileName;
                                    _output = ofd.SafeFileName;
                                }
                                if (_output != "")
                                {
                                    _output = perevod(_output);
                                    string p = _output.Substring(0, _output.Length - 4);
                                    int i = 1;
                                    while (File.Exists(folder_path + Folder + "\\" + p + ".pdf"))
                                    {
                                        p += i;
                                        i++;
                                    }
                                    File.Copy(s, folder_path + Folder + "\\" + p + ".pdf");
                                    con.ExecuteNonQuery("update " + Table + " set " + Pole + "_text = '" + p + ".pdf' where " + Pole + "_id = " + id);
                                    MessageBox.Show("Добавлено.");
                                }
                            }
                            break;
                        case MessageBoxResult.No: break;
                    }
                }
                //////ИЗМЕНЕНИЕ
                else if (add == 0)
                {
                    TV.Visibility = Visibility.Collapsed;
                    webbrowser.Visibility = Visibility.Visible;
                    string deleteme1 = folder_path + Folder + "\\" + knigapath;
                    con.OpenPdf(folder_path + Folder + "\\" + knigapath, webbrowser);
                    nazad.Content = "Назад к списку";
                    Web_Tree.Children.Add(nazad);
                    nazad.Margin = new Thickness(0,0,0,10);
                    nazad.Width = 150;
                    nazad.Height = 20;
                    nazad.Click += (q,j) =>
                    {

                        TV.Visibility = Visibility.Visible;
                        webbrowser.Visibility = Visibility.Collapsed;
                        Web_Tree.Children.Remove(btn);
                        Web_Tree.Children.Remove(nazad);
                        ChangeBtn = false;
                        if(File.Exists(deleteme)) File.Delete(deleteme);
                    };
                    btn.Content = "Загрузить другой файл";
                    Web_Tree.Children.Add(btn);
                    btn.Width = 150;
                    btn.Height = 20;
                    btn.Click += (q, j) =>
                    {
                        MessageBoxResult result = MessageBox.Show("Вы хотите изменить материал \"" + kniganame + "\"?", "Изменение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        switch (result)
                        {
                            case MessageBoxResult.Yes:
                                {
                                    string s = "";
                                    string _output = "";
                                    OpenFileDialog ofd = new OpenFileDialog();
                                    ofd.Title = "as";
                                    ofd.InitialDirectory = @"C:\";
                                    ofd.Filter = "Pdf Files|*.pdf";
                                    if (ofd.ShowDialog() == true)
                                    {
                                        s = ofd.FileName;
                                        _output = ofd.SafeFileName;
                                    }
                                    if (_output != "")
                                    {
                                        _output = perevod(_output);
                                        string p = _output.Substring(0, _output.Length - 4);
                                        int i = 1;
                                        while (File.Exists(folder_path + Folder + "\\" + p + ".pdf"))
                                        {
                                            p += i;
                                            i++;
                                        }
                                        File.Copy(s, folder_path + Folder + "\\" + p + ".pdf");
                                        con.ExecuteNonQuery("update " + Table + " set " + Pole + "_text = '" + p + ".pdf' where " + Pole + "_id = " + id);
                                        webbrowser.Navigate(folder_path + Folder + "\\" + p + ".pdf");
                                        deleteme = deleteme1;
                                        MessageBox.Show("Изменено.");
                                        
                                    }
                                }
                                break;
                            case MessageBoxResult.No: break;
                        }
                    };
                    ChangeBtn = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        public void Vypush_knigi()
        {
            try
            {
                TV.Items.Clear();
                webbrowser.Visibility = Visibility.Collapsed;
                TV.Visibility = Visibility.Visible;
                int count = Convert.ToInt32(con.ExecuteScalar("select count (*) from nomnary where nomnary_uid = " + uid));
                string[] kniga_name = con.Reader_Array("select nomnary_title from nomnary where nomnary_uid = " + uid + " order by nomnary_title", count);
                string[] kniga_id = con.Reader_Array("select nomnary_id from nomnary where nomnary_uid = " + uid + " order by nomnary_title", count);
                string[] kniga_janr = con.Reader_Array("select nomnary_zhanr from nomnary where nomnary_uid = " + uid + " order by nomnary_title", count);
                string[] kniga_date = con.Reader_Array("select nomnary_god from nomnary where nomnary_uid = " + uid + " order by nomnary_title", count);
                string[] kniga_tipograph = con.Reader_Array("select nomnary_tipograph from nomnary where nomnary_uid = " + uid + " order by nomnary_title", count);
                string[] kniga_str = con.Reader_Array("select nomnary_stranisy from nomnary where nomnary_uid = " + uid + " order by nomnary_title", count);
                string[] kniga_text = con.Reader_Array("select nomnary_text from nomnary where nomnary_uid = " + uid + " order by nomnary_title", count);
                for (int i = 0; i < count; i++)
                {
                    TreeViewItem TVitem = new TreeViewItem();
                    TV.Items.Add(TVitem);
                }
                int l = 0;
                foreach (TreeViewItem tvitem in TV.Items)
                {
                    tvitem.FontFamily = new FontFamily("Arial");
                    tvitem.FontSize = 16;
                    if (kniga_text[l] != "") tvitem.Foreground = Brushes.Brown;
                    else tvitem.Foreground = Brushes.Black;
                    
                    tvitem.Header = (l + 1) + ". " + kniga_name[l] + ". " + kniga_name[l] + " " + kniga_janr[l] + " .- " + kniga_tipograph[l] + ", " + kniga_date[l];
                    tvitem.Tag = kniga_id[l];
                    l++;
                    if(Del_Add==2)
                    {
                        tvitem.PreviewMouseUp += (s, j) =>
                        {
                            Delete_File("nomnary","nomnary", Convert.ToInt32(tvitem.Tag), "nomnary",1);
                            Vypush_knigi();
                        };
                    }
                    else if(Del_Add==1)
                    {
                        tvitem.PreviewMouseUp += (s, j) =>
                        {
                            if (tvitem.Foreground == Brushes.Black)
                            {
                                Delete_File("nomnary", "nomnary", Convert.ToInt32(tvitem.Tag), "nomnary", 2);
                                Vypush_knigi();
                            }
                            else
                            {
                                Delete_File("nomnary", "nomnary", Convert.ToInt32(tvitem.Tag), "nomnary", 0);
                               
                            }
                        };
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        public void raboty_o_pisatele()
        {
            try
            {
                webbrowser.Visibility = Visibility.Collapsed;
                TV.Visibility = Visibility.Visible;
                TV.Items.Clear();
                int count = Convert.ToInt32(con.ExecuteScalar("select count (*) from chog_dug_azhyldar where chdugazh_uid = " + uid));
                string[] kniga = con.Reader_Array("select chdugazh_id from chog_dug_azhyldar where chdugazh_uid = " + uid, count);
                string[] autor = con.Reader_Array("select chdugazh_avtor from chog_dug_azhyldar where chdugazh_uid = " + uid, count);
                string[] title = con.Reader_Array("select chdugazh_title from chog_dug_azhyldar where chdugazh_uid = " + uid, count);
                string[] istochnik = con.Reader_Array("select chdugazh_istochnik from chog_dug_azhyldar where chdugazh_uid = " + uid, count);
                string[] year = con.Reader_Array("select chdugazh_god from chog_dug_azhyldar where chdugazh_uid = " + uid, count);
                string[] kniga_text = con.Reader_Array("select chdugazh_text from chog_dug_azhyldar where chdugazh_uid = " + uid, count);
                for (int i = 0; i < count; i++)
                {
                    TreeViewItem tvitem = new TreeViewItem();
                    tvitem.FontFamily = new FontFamily("Arial");
                    tvitem.FontSize = 16;
                    tvitem.Header = (i + 1) + ". " + autor[i] + ". " + title[i] + "// " + istochnik[i] + ". - " + year[i] + ".";
                    TV.Items.Add(tvitem);
                    if (kniga_text[i] != "") tvitem.Foreground = Brushes.Brown;
                    else tvitem.Foreground = Brushes.Black;
                    tvitem.Tag = kniga[i];
                    if (Del_Add == 2)
                    {
                        tvitem.PreviewMouseUp += (s, j) =>
                        {
                            Delete_File("chog_dug_azhyldar", "chdugazh", Convert.ToInt32(tvitem.Tag), "chog_dug_azhyldary", 1);
                            raboty_o_pisatele();
                        };
                    }
                    else if (Del_Add == 1)
                    {
                        tvitem.PreviewMouseUp += (s, j) =>
                        {
                            if (tvitem.Foreground == Brushes.Black)
                            {
                                Delete_File("chog_dug_azhyldar", "chdugazh", Convert.ToInt32(tvitem.Tag), "chog_dug_azhyldary", 2);
                                raboty_o_pisatele();
                            }
                           /* else
                            {
                                Delete_File("nomnary", "nomnary", Convert.ToInt32(tvitem.Tag), "nomnary", 0);

                            }*/
                        };
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        public void stih_pes()
        {
            try
            {
                webbrowser.Visibility = Visibility.Collapsed;
                TV.Visibility = Visibility.Visible;
                TV.Items.Clear();
                int count = con.ExecuteScalar("select count (*) from yry_apargan_shulukteri where yry_apsh_uid = " + uid);
                string[] yry_id = con.Reader_Array("select yry_apsh_id from yry_apargan_shulukteri where yry_apsh_uid  = " + uid + "", count);
                string[] yry_music = con.Reader_Array("select  yry_apsh_muz from yry_apargan_shulukteri where yry_apsh_uid  = " + uid + "", count);
                string[] yry_title = con.Reader_Array("select  yry_apsh_title from yry_apargan_shulukteri where yry_apsh_uid  = " + uid + "", count);
                string[] yry_year = con.Reader_Array("select  yry_apsh_god from yry_apargan_shulukteri where yry_apsh_uid  = " + uid + "", count);
                string[] kniga_text = con.Reader_Array("select  yry_apsh_text from yry_apargan_shulukteri where yry_apsh_uid  = " + uid + "", count);
                for (int i = 0; i < count; i++)
                {
                    TreeViewItem TVitem = new TreeViewItem();
                    TVitem.FontFamily = new FontFamily("Arial");
                    TVitem.FontSize = 16;
                    TVitem.Header = (i + 1) + ". " + yry_title[i] + "-" + yry_music[i];
                    TV.Items.Add(TVitem);
                    if (kniga_text[i] != "") TVitem.Foreground = Brushes.Brown;
                    else TVitem.Foreground = Brushes.Black;
                    TVitem.Tag = yry_id[i];
                    if (Del_Add == 2)
                    {
                        TVitem.PreviewMouseUp += (s, j) =>
                        {
                            Delete_File("yry_apargan_shulukteri", "yry_apsh", Convert.ToInt32(TVitem.Tag), "pesni", 1);
                            stih_pes();
                        };
                    }
                    else if (Del_Add == 1)
                    {
                        TVitem.PreviewMouseUp += (s, j) =>
                        {
                            if (TVitem.Foreground == Brushes.Black)
                            {
                                Delete_File("yry_apargan_shulukteri", "yry_apsh", Convert.ToInt32(TVitem.Tag), "pesni",2);
                                stih_pes();
                            }
                            /* else
                             {
                                 Delete_File("nomnary", "nomnary", Convert.ToInt32(tvitem.Tag), "nomnary", 0);

                             }*/
                        };
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        public void Perevod_proizv()
        {
            try
            {
                webbrowser.Visibility = Visibility.Collapsed;
                TV.Visibility = Visibility.Visible;
                TV.Items.Clear();
                int count = con.ExecuteScalar("select count (*) from ochulgalary where ochulgalary_uid = " + uid);
                string[] kniga = con.Reader_Array("select ochulgalary_id from ochulgalary where ochulgalary_uid = " + uid, count);
                string[] title = con.Reader_Array("select ochulgalary_title from ochulgalary where ochulgalary_uid = " + uid, count);
                string[] autor = con.Reader_Array("select ochulgalary_autor from ochulgalary where ochulgalary_uid = " + uid, count);
                string[] perevodchik = con.Reader_Array("select ochulgalary_perevodchik from ochulgalary where ochulgalary_uid = " + uid, count);
                string[] tipograph = con.Reader_Array("select ochulgalary_tipograph from ochulgalary where ochulgalary_uid = " + uid, count);
                string[] year = con.Reader_Array("select ochulgalary_god from ochulgalary where ochulgalary_uid = " + uid, count);
                string[] Rus = con.Reader_Array("select ochulgalary_log from ochulgalary where ochulgalary_uid = " + uid, count);
                string[] kniga_text = con.Reader_Array("select ochulgalary_text from ochulgalary where ochulgalary_uid = " + uid, count);
                for (int i = 0; i < count; i++)
                {
                    TreeViewItem TVitem = new TreeViewItem();
                    TVitem.FontFamily = new FontFamily("Arial");
                    TVitem.FontSize = 16;
                    if (Rus[i].Trim() == "tyv") Rus[i] = "На тувинский";
                    else Rus[i] = "С тувинского";
                    TVitem.Header = (i + 1) + ". " + autor[i] + " " + title[i] + ":/ " + perevodchik[i] + " - " + tipograph[i] + ", " + year[i] + ", " + Rus[i];
                    TV.Items.Add(TVitem);
                    if (kniga_text[i] != "") TVitem.Foreground = Brushes.Brown;
                    else TVitem.Foreground = Brushes.Black;
                    TVitem.Tag = kniga[i];
                    if (Del_Add == 2)
                    {
                        TVitem.PreviewMouseUp += (s, j) =>
                        {
                            Delete_File("ochulgalary", "ochulgalary", Convert.ToInt32(TVitem.Tag), "perevod", 1);
                            Perevod_proizv();
                        };
                    }
                    else if (Del_Add == 1)
                    {
                        TVitem.PreviewMouseUp += (s, j) =>
                        {
                            if (TVitem.Foreground == Brushes.Black)
                            {
                                Delete_File("ochulgalary", "ochulgalary", Convert.ToInt32(TVitem.Tag), "perevod", 2);
                                Perevod_proizv();
                            }
                            /* else
                             {
                                 Delete_File("nomnary", "nomnary", Convert.ToInt32(tvitem.Tag), "nomnary", 0);

                             }*/
                        };
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
        public void vis_add(bool a)
        {
            if(a)
            {
                yndyrgen_nomnary.Visibility = Visibility.Visible;
                chog_dug_azhyld.Visibility = Visibility.Visible;
                Ochulgalary.Visibility = Visibility.Visible;
                yry_apargan.Visibility = Visibility.Visible;
            }
            else
            {
                yndyrgen_nomnary.Visibility = Visibility.Collapsed;
                chog_dug_azhyld.Visibility = Visibility.Collapsed;
                Ochulgalary.Visibility = Visibility.Collapsed;
                yry_apargan.Visibility = Visibility.Collapsed;
            }
        }
      
        public pass()
        {
            InitializeComponent();
            Password.Visibility = Visibility.Visible;
            
            foreach (TreeViewItem item in MaterTV.Items)
            {
                item.Expanded += (s, j) =>
                 {
                     foreach(TreeViewItem otheritem in MaterTV.Items)
                     {
                         if (otheritem.Tag != item.Tag) otheritem.IsExpanded = false;
                     }
                 };
                item.PreviewMouseUp += (s, j) =>
                {
                    if (item.IsExpanded == false)
                    {
                        webbrowser.Visibility = Visibility.Collapsed;
                        TV.Visibility = Visibility.Collapsed;
                        spisok_v_db.Visibility = Visibility.Collapsed;
                       
                        if (ChangeBtn == true)
                        {
                            Web_Tree.Children.Remove(nazad);
                            Web_Tree.Children.Remove(btn);
                            ChangeBtn = false;
                        }

                    }
                    else
                    {
                        spisok_v_db.Visibility = Visibility.Visible;
                    }
                    int k = Convert.ToInt32(item.Tag);
                    //MessageBox.Show(k + "");
                    /////ADD
                    if(Del_Add == 1)
                    {
                        vis_add(true);
                        switch (k)
                        {
                            case 1:////BIOGRAPHIA
                                {
                                    if(File.Exists(folder_path+"biographia.pdf"))
                                    {
                                        TV.Visibility = Visibility.Collapsed;
                                        webbrowser.Visibility = Visibility.Visible;
                                        webbrowser.Navigate(folder_path + "biographia.pdf");///Вывод биографии писателя.
                                        string Exist = exist("Биография этого автора есть. Хотите заменить другим файлом?");
                                        if (Exist == "Yes")
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
                                        TV.Visibility = Visibility.Collapsed;
                                        webbrowser.Visibility = Visibility.Visible;
                                        webbrowser.Navigate(folder_path + "biographia.pdf");///Вывод биографии писателя.
                                    }
                                    //MessageBox.Show(message("Вы хотите добавить биографию писателя", "Биография"));

                                }
                                break;
                                
                            case 2://///Писательская работа
                                {
                                    if (File.Exists(folder_path + "pisatel_rab.pdf"))
                                    {
                                        TV.Visibility = Visibility.Collapsed;
                                        webbrowser.Visibility = Visibility.Visible;
                                        webbrowser.Navigate(folder_path + "pisatel_rab.pdf");///Вывод биографии писателя.
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
                                        TV.Visibility = Visibility.Collapsed;
                                        webbrowser.Visibility = Visibility.Visible;
                                        webbrowser.Navigate(folder_path + "pisatel_rab.pdf");///Вывод биографии писателя.
                                    }
                                } break;
                         }
                    }
                    /////DELETE
                    else if (Del_Add == 2)
                    {
                        vis_add(false);
                        switch (k)
                        {
                            case 1:
                                {
                                    TV.Visibility = Visibility.Collapsed;
                                    webbrowser.Visibility = Visibility.Visible;
                                    string _input = "";
                                    if (File.Exists(folder_path + "biographia.pdf"))
                                    {
                                        webbrowser.Navigate(folder_path + "biographia.pdf");
                                        MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить биографию?", "Удаление биографии", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                        switch (result)
                                        {
                                            case MessageBoxResult.Yes:
                                                {
                                                 /*   webbrowser.Refresh();
                                                    webbrowser.Navigate(folder_path + "pisatel_rab.pdf");*/
                                                    File.Delete(folder_path + "biographia.pdf");
                                                }
                                                break;

                                            case MessageBoxResult.No:
                                                break;
                                        }
                                    }
                                    else { _input = message("По этому писателю биография отсутствует. Хотите добавить?", "Нет биографии"); File.Copy(_input, folder_path + "biographia.pdf"); }

                                } break;
                            case 2:
                                {
                                    TV.Visibility = Visibility.Collapsed;
                                    webbrowser.Visibility = Visibility.Visible;
                                    string _input = "";
                                    if (File.Exists(folder_path + "pisatel_rab.pdf"))
                                    {
                                        webbrowser.Navigate(folder_path + "pisatel_rab.pdf");
                                        MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить писательскую работу?", "Удаление писательской работы", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                        switch (result)
                                        {
                                            case MessageBoxResult.Yes:
                                                {
                                                    File.Delete(folder_path + "pisatel_rab.pdf");
                                                }
                                                break;

                                            case MessageBoxResult.No:
                                                break;
                                        }
                                    }
                                    else { _input = message("Писательская работа отсутствует. Хотите добавить?", "Нет писательской работы"); File.Copy(_input, folder_path + "pisatel_rab.pdf"); }
                                } break;
                          }
                    }
                };
                item.Expanded += (s, j) =>
                {
                    int k = Convert.ToInt32(item.Tag);
                    switch (k)
                        {
                            case 3://///Выпущенные книги
                                {
                                    Vypush_knigi();
                                }
                                break;
                            case 4:////Работы о писателе
                                {
                                    raboty_o_pisatele();   
                                }
                                break;
                            case 5:////Перевод произведений
                                {
                                    Perevod_proizv();
                                }
                                break;
                            case 7:////Стихотворения ставшие песнями
                                {
                                    stih_pes();
                                }
                                break;
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
            try
            {
                string passwrd = con.Reader("select password from Password where id = 1");
                string log = con.Reader("select login from Password where id = 1");
                string _hash = "";
                using (MD5 md5Hash = MD5.Create())
                {
                    _hash = Code(md5Hash, password.Password.Trim());
                }
                if (login.Text == log && _hash == passwrd)
                {
                    this.WindowState = WindowState.Maximized;
                    Password.Visibility = Visibility.Collapsed;
                    admin();
                }
                else MessageBox.Show("Неверный пароль или логин!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            
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
            try
            {
                if(File.Exists(path+ "\\all\\img\\default.png"))
                    add_img_pis.ImageSource = con.GetImage("\\all\\img\\default.png");
              

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }


            
        }
                                                                                        //////РЕДАКТИРОВАНИЕ ПИСАТЕЛЯ
        private void Change_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            PAGES += "3";
            Back.Visibility = Visibility.Visible;
            adminPanel.Visibility = Visibility.Collapsed;
            Menu_Admin = 2;
            deleteWR.Visibility = Visibility.Visible;
            Menu_Header.Content = "Редактирование писателя";
            txtLastName.Text = "";
            search("");
           
        }
        private void txtLastName_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            string surname = txtLastName.Text.Trim();
            search(surname);
        }
        public void search(string fam)
        {
            try
            {
                Delete_grid.Children.Clear();
                fam = txtLastName.Text.Trim();
                int count = con.ExecuteScalar("select count (*) from chogaalchy where chog_fam like '" + fam + "%'");
                string[] all_id = con.Reader_Array("select chog_id from chogaalchy where chog_fam like '" + fam + "%' order by chog_fam", count);
                string[] all_img = con.Reader_Array("select chog_photo from chogaalchy where chog_fam like '" + fam + "%' order by chog_fam" , count);
                string[] all_fam = con.Reader_Array("select chog_fam from chogaalchy where chog_fam like '" + fam + "%' order by chog_fam", count);
                string[] all_imya = con.Reader_Array("select chog_imya from chogaalchy  where chog_fam like '" + fam + "%' order by chog_fam", count);
                string[] all_otch = con.Reader_Array("select chog_otch from chogaalchy  where chog_fam like '" + fam + "%' order by chog_fam", count);
                string[] all_folder = con.Reader_Array("select folder from chogaalchy  where chog_fam like '" + fam + "%' order by chog_fam", count);
                for (int i = 0; i < count; ++i)
                {
                    Delete_grid.RowDefinitions.Add(new RowDefinition());
                    TextBlock txb = new TextBlock();
                    txb.Text = all_fam[i] + " " + all_imya[i] + " " + all_otch[i];
                    txb.FontSize = 16;
                    //txb.HorizontalAlignment = HorizontalAlignment.Center;

                    Border b = new Border();
                    b.BorderBrush = Brushes.White;
                    b.BorderThickness = new Thickness(1);
                    b.CornerRadius = new CornerRadius(15);
                    b.Width = 75;
                    b.Height = 75;

                    ImageBrush img = new ImageBrush();
                    if (File.Exists(path + "pisateli\\" + all_folder[i] + "\\" + all_img[i]))
                        img.ImageSource = con.GetImage2(path + "pisateli\\" + all_folder[i] + "\\" + all_img[i]);
                    else img.ImageSource = con.GetImage2(path + "\\all\\img\\default.png");


                    b.Background = img;
                    txb.SetValue(Grid.ColumnProperty, 0);
                    txb.SetValue(Grid.RowProperty, i);
                    b.SetValue(Grid.ColumnProperty, 1);
                    b.SetValue(Grid.RowProperty, i);
                    Delete_grid.Children.Add(txb);
                    Delete_grid.Children.Add(b);
                    txb.Tag = all_id[i];
                    b.Tag = i;
                    txb.PreviewMouseUp += (s, j) =>
                    {
                        if (Menu_Admin == 3)
                        {
                            int k = Convert.ToInt32(b.Tag);
                            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить писателя \"" + all_fam[k] + " " + all_imya[k] + " " + all_otch[k] + "\"?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {
                                        try
                                        {
                                            string folder = con.Reader("select folder from chogaalchy where chog_id = " + txb.Tag);
                                            if (Directory.Exists(path + "pisateli\\" + folder)) FolderDelete(path + "pisateli\\" + folder);
                                            con.ExecuteNonQuery("delete from chogaalchy where chog_id = " + txb.Tag); ////// Удаление  писателя
                                            con.ExecuteNonQuery("delete from chog_dug_azhyldar where chdugazh_uid = " + txb.Tag); //// Удаление работ о писателе
                                            con.ExecuteNonQuery("delete from nomnary where nomnary_uid = " + txb.Tag); //// Удаление книг писателя
                                            con.ExecuteNonQuery("delete from ochulgalary where ochulgalary_uid = " + txb.Tag); //// Удаление переводов писателя
                                            con.ExecuteNonQuery("delete from yry_apargan_shulukteri where yry_apsh_uid = " + txb.Tag); //// Удаление стихотворений ставших песнями писателя
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex + "");
                                        }
                                        finally
                                        {
                                            MessageBox.Show("Удалено.");
                                            Delete_PreviewMouseUp(s, j);
                                        }
                                    }
                                    break;
                                case MessageBoxResult.No: break;
                            }
                        }
                        else if (Menu_Admin == 2)
                        {
                            uid = Convert.ToInt32(txb.Tag);
                            folder_path = path + "pisateli\\" + con.Reader("select folder from chogaalchy where chog_id = " + uid) + "\\";
                            Edit_Writer();
                            PAGES += "4";
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
                                                                                        ////////УДАЛЕНИЕ ПИСАТЕЛЯ
        private void Delete_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Menu_Header.Content = "Удаление писателя";
            Menu_Admin = 3;
            PAGES += "5";
            Back.Visibility = Visibility.Visible;
            deleteWR.Visibility = Visibility.Visible;
            adminPanel.Visibility = Visibility.Collapsed;
            txtLastName.Text = "";
            search("");
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
    
                                                                                                     ///////ДОБАВЛЕНИЕ ПИСАТЕЛЯ
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fam = Surname.Text.Trim();
                string imya = Name.Text.Trim();
                string otch = Patronymic.Text.Trim();
                string fio = (fam + imya + otch).Trim();
                DateTime BornSelectedDate = Born.SelectedDate.Value;
                string bornDate = BornSelectedDate.Day + " " + BornSelectedDate.ToString("MMMM") + " " + BornSelectedDate.Year;
                DateTime DeathSelectedDate = Death.SelectedDate.Value;
                string deathDate = DeathSelectedDate.Day + " " + DeathSelectedDate.ToString("MMMM") + " " + DeathSelectedDate.Year;
                int region_id = Koj.SelectedIndex + 1;
                int count = con.ExecuteScalar("select count (*) from chogaalchy");
                if (fam != "" && imya != "" && otch != ""&& Koj.SelectedIndex!=-1)
                {
                    string[] id = con.Reader_Array("select chog_id from chogaalchy order by chog_fam", count);
                    string[] all_fam = con.Reader_Array("select chog_fam from chogaalchy order by chog_fam", count);
                    string[] all_imya = con.Reader_Array("select chog_imya from chogaalchy order by chog_fam", count);
                    string[] all_otch = con.Reader_Array("select chog_otch from chogaalchy order by chog_fam", count);
                    string[] all_fio = new string[count];
                    bool check = false;
                    for (int i = 0; i < count; ++i)
                    {
                        all_fio[i] = (all_fam[i]+all_imya[i]+all_otch[i]).Trim();
                        if (all_fio[i] == fio)
                        {
                            check = true;
                            string messageBoxText = "Писатель с такими данными уже состит в базе хотите редактировать писателя?";
                            string caption = "Существующий писатель";
                            MessageBoxButton button = MessageBoxButton.YesNoCancel;
                            MessageBoxImage icon = MessageBoxImage.Warning;
                            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {
                                        uid = Convert.ToInt32(id[i]);
                                        PAGES += 4;
                                        Edit_Writer();
                                    }
                                    break;
                                case MessageBoxResult.No:
                                    {
                                    }
                                    break;
                            }
                            break;
                            
                        }
                    }
                    if(!check)
                    {
                        try
                        {
                            string folder = perevod(fam + imya.Substring(0, 1) + otch.Substring(0, 1));
                            int i = 1;
                            while(Directory.Exists(path+"pisateli\\"+folder))
                            {
                                folder += i;
                                i++;
                            }
                            Directory.CreateDirectory(path + "pisateli\\" + folder);
                            Directory.CreateDirectory(path + "pisateli\\" + folder + "\\chog_dug_azhyldary");
                            Directory.CreateDirectory(path + "pisateli\\" + folder + "\\nomnary");
                            Directory.CreateDirectory(path + "pisateli\\" + folder + "\\perevod");
                            Directory.CreateDirectory(path + "pisateli\\" + folder + "\\pesni");
                            if (pis_image != "")
                            {

                                if (File.Exists(pis_image))
                                {
                                    File.Copy(pis_image, path + "pisateli\\" + folder + "\\photo" + Writer_Photo);
                                }
                            }
                            string query = "insert into chogaalchy (chog_fam,chog_imya,chog_otch,chog_datarozh,chog_datasmerti,chog_region_id,chog_photo,folder,chog_nasel_punkt,lat_coords,lon_coords,chog_rod_deyat,chog_gody_tvorch,chog_mestosmerti,chog_zhanr,chog_biografiya) values ('" + fam + "','" + imya + "','" + otch + "','" + bornDate + "','" + deathDate + "'," + region_id + ",'photo" + Writer_Photo + "','" + folder + "','','','','','','','','')";
                            con.ExecuteNonQuery(query);
                            string messageBoxText = "Писатель добавлен. Добавить материалы?";
                            string caption = "Добавлен";
                            MessageBoxButton button = MessageBoxButton.YesNoCancel;
                            MessageBoxImage icon = MessageBoxImage.Warning;
                            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    {
                                        uid = con.Reader_Int("select chog_id from chogaalchy where chog_fam like '" + fam + "' and chog_imya like '" + imya + "' and chog_otch like '" + otch + "'");
                                        PAGES += 4;
                                        Edit_Writer();
                                    }
                                    break;
                                case MessageBoxResult.No:
                                    {
                                    }
                                    break;
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex + "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            }
        }
        
                                                                                    /////ВЫВОД ИНФОРМАЦИИ ПО ПИСАТЕЛЮ
        public void Edit_Writer()
        {
            addWR.Visibility = Visibility.Collapsed;
            searchWR.Visibility = Visibility.Collapsed;
            deleteWR.Visibility = Visibility.Collapsed;
            editWR.Visibility = Visibility.Visible;
            FAM.Text = con.Reader("select chog_fam from chogaalchy where chog_id = " + uid);
            Imya.Text = con.Reader("select chog_imya from chogaalchy where chog_id = " + uid);
            Otch.Text = con.Reader("select chog_otch from chogaalchy where chog_id = " + uid);
            Data_rojd.Text = con.Reader("select chog_datarozh from chogaalchy where chog_id = " + uid);
            Data_Smerti.Text = con.Reader("select chog_datasmerti from chogaalchy where chog_id = " + uid);
            string image = con.Reader("select folder from chogaalchy where chog_id = " + uid)+"\\" + con.Reader("select chog_photo from chogaalchy where chog_id = " + uid);
            if (File.Exists(path+"pisateli\\" + image))
                Wr_Image.ImageSource = con.GetImage2(path + "pisateli\\" + image); 
            else Wr_Image.ImageSource = con.GetImage2(path + "\\all\\img\\default.png");
            int koj_num = con.Reader_Int("select chog_region_id from chogaalchy where chog_id = " + uid);
            foreach(ComboBoxItem cmbitem in Kojuun.Items)
            {
                if (Convert.ToInt32(cmbitem.Tag) == koj_num) Kojuun.SelectedIndex = koj_num - 1;
            }

        }
        //////Добавление материала
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bord = dobav_mater;
            dobav_mater.Background = new SolidColorBrush(Color.FromRgb(0, 96, 135));
            udal_mater.Background = new SolidColorBrush(Color.FromRgb(0, 150, 211));
            spisok_v_db.Visibility = Visibility.Collapsed;
            Del_Add = 1;
            MaterText.Text = "Добавление материала";
            DeleteMater.Visibility = Visibility.Visible;
            TV.Visibility = Visibility.Collapsed;
            foreach(TreeViewItem item in MaterTV.Items)
            {
                item.IsExpanded = false;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bord = udal_mater;
            udal_mater.Background = new SolidColorBrush(Color.FromRgb(0, 96, 135));
            dobav_mater.Background = new SolidColorBrush(Color.FromRgb(0, 150, 211));
            spisok_v_db.Visibility = Visibility.Collapsed;
            Del_Add = 2;
            MaterText.Text = "Удаление материала";
            DeleteMater.Visibility = Visibility.Visible;
            TV.Visibility = Visibility.Collapsed;
            foreach (TreeViewItem item in MaterTV.Items)
            {
                item.IsExpanded = false;
            }
        }
                                                                                                  /////////ИЗ КИРИЛИЦЫ В ЛАТИНИЦУ
        public string perevod(string a)
        {
            string AlphRus = "АБВГДЕЁЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзиклмнопрстуфхцчшщъыьэюя ";
            string AlphEng = "ABVGDEEJZIKLMNOPRSTUFHCCHSQYQEUAabvgdeejziklmnoprstufhcchsqyqeua_";
            string output = "";
            bool k = false;
            foreach (char s in a)
            {
                k = false;
                for (int i=0;i<AlphRus.Length;++i)
                {
                    if (s == AlphRus[i])
                    {
                        output += AlphEng[i];
                        k = true;
                    }
                }
                if (k == false)
                {
                    output += s;
                }
            }
            return output;
        }
                                                                                         /////////ДОБАВЛЕНИЕ КНИГИ DONE
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                string Name = Book_Name.Text.Trim();
                string Janr = Book_Janr.Text.Trim();
                string Year = Book_Year.Text.Trim();
                string Tipograph = Book_Tipogr.Text.Trim();
                string Pages = Book_Pages.Text.Trim();
                if (Name != "" && Janr != "" && Year != "" && Tipograph != "" && Pages != "")
                {
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
                        _FileName = perevod(ofd.SafeFileName);
                        _Output = folder_path + "nomnary\\" + _FileName;
                    }
                    if (k == 0)
                    {
                        con.ExecuteNonQuery("insert into nomnary (nomnary_title,nomnary_zhanr,nomnary_god,nomnary_tipograph,nomnary_stranisy,nomnary_text,nomnary_uid,nomnary_pozisiya) values ('" + Name + "','" + Janr + "'," + Year + ",'" + Tipograph + "'," + Pages + ",'" + _FileName + "'," + uid + ",0)");
                        k = 1;
                    }
                    if (k == 1)
                    {
                        File.Copy(_Input, _Output);
                    }
                    TV.Items.Clear();
                    Vypush_knigi();
                }
                else MessageBox.Show("Пожалуйста, заполните все поля");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex+"");
            }
        }
                                                                                                    //////КНОПКА НАЗАД
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (PAGES.Length>1)
            {
                int _now = Convert.ToInt32(PAGES.Substring(PAGES.Length - 1, 1));
                PAGES = PAGES.Substring(0, PAGES.Length - 1);
                int _before = Convert.ToInt32(PAGES.Substring(PAGES.Length - 1, 1));
                switch (_now)
                {
                    case 2: { addWR.Visibility = Visibility.Collapsed; } break;
                    case 3: { deleteWR.Visibility = Visibility.Collapsed;  } break;
                    case 4: { editWR.Visibility = Visibility.Collapsed;  } break;
                    case 5: { deleteWR.Visibility = Visibility.Collapsed; } break;
                    case 6: { PassChangePanel.Visibility = Visibility.Collapsed; }break;
                }
                switch (_before)
                {
                    case 1:
                        {
                            admin();
                        } break;
                    case 2:
                        {
                            addWR.Visibility = Visibility.Visible;
                        }
                        break;
                    case 3:
                        {
                            deleteWR.Visibility = Visibility.Visible;
                        }break;
                    case 5: admin(); break;

                }
            }
        }
        /////////ДОБАВЛЕНИЕ РАБОТЫ О ПИСАТАЛЕ DONE
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            { 
                string Title = Chog_Dug_Name.Text.Trim();
                string Autor = Chog_Dug_Autor.Text.Trim();
                string Magazine = Chog_Dug_Istochnik.Text.Trim();
                string Year = Chog_Dug_Year.Text.Trim();
                string Day = Chog_Dug_Day.Text.Trim();
                string Number = Chog_Dug_Number.Text.Trim();
                string Pages = Chog_Dug_Pages.Text.Trim();
                string Language = "";
                if (Chog_Dug_Language1.IsChecked == true) Language = "rus";
                else Language = "tyv";
                if (Title != "" && Autor != "" && Magazine != "" && Year != "" && Day != "" && Number != "" && Pages != "")
                {
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
                        _FileName = perevod(ofd.SafeFileName);
                        _Output = folder_path + "chog_dug_azhyldary\\" + _FileName;
                    }
                    if (k == 0)
                    {
                        con.ExecuteNonQuery("insert into chog_dug_azhyldar (chdugazh_title,chdugazh_avtor,chdugazh_text,chdugazh_istochnik,chdugazh_god,chdugazh_den,chdugazh_nomer,chdugazh_stranisy,chdugazh_log,chdugazh_uid) values ('" + Title + "','" + Autor + "','" + _FileName + "','" + Magazine + "'," + Year + ",'" + Day + "','№ " + Number + "','С. " + Pages + ".','" + Language + "'," + uid + ")");
                        k = 1;
                    }
                    if (k == 1)
                    {
                        File.Copy(_Input, _Output);
                    }
                    raboty_o_pisatele();
                }
                else MessageBox.Show("Пожалуйста, заполните все поля");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex+"");
            }
}

                                                                                               /////////ДОБАВЛЕНИЕ ПЕРЕВОДЫ DONE
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

            try
            {
                string Title = Ochulgalary_Name.Text.Trim();
                string Janr = Ochulgalary_Janr.Text.Trim();
                string Autor = Ochulgalary_Autor.Text.Trim();
                string Translator = Ochulgalary_Translator.Text.Trim();
                string Redaktor = Ochulgalary_Redaktor.Text.Trim();
                string Tipograph = Ochulgalary_Tipograph.Text.Trim();
                ////DATE
                DateTime SelectedDate = Ochulgalary_Date.SelectedDate.Value;
                int Year = SelectedDate.Year;
                string Day = SelectedDate.Day.ToString();
                string Month = SelectedDate.ToString("MMMM");
                string daymonth = Day + " " + Month;
                ////
                string Number = Ochulgalary_Number.Text.Trim();
                string Pages = Ochulgalary_Pages.Text.Trim();

                string papka = "";

                string Language = "";
                if (Ochulgalary_Language.IsChecked == true) { Language = "oth"; papka = "perevod_other\\"; }
                else { Language = "tyv"; papka = "perevod_tuv\\"; }
                if (Title != "" && Janr != "" && Translator != "" && Redaktor != "" && Tipograph != "" && Number != "" && Pages != "")
                {
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
                        _FileName = perevod(ofd.SafeFileName);
                        _Output = folder_path + papka + _FileName;
                    }
                    if (k == 0)
                    {
                        con.ExecuteNonQuery("insert into ochulgalary (ochulgalary_title,ochulgalary_zhanr,ochulgalary_autor,ochulgalary_perevodchik,ochulgalary_redaktor,ochulgalary_text,ochulgalary_tipograph,ochulgalary_god,ochulgalary_den,ochulgalary_nomer,ochulgalary_stranisy,ochulgalary_log,ochulgalary_uid) values ('" + Title + "','" + Janr + "','" + Autor + "','" + Translator + "','" + Redaktor + "','" + _FileName + "','" + Tipograph + "'," + Year + ",'" + daymonth + "'," + Number + "','" + Pages + " ар.','" + Language + "'," + uid + ")");
                        k = 1;
                    }
                    if (k == 1)
                    {
                        if (File.Exists(_Output)) File.Delete(_Output);
                        File.Copy(_Input, _Output);
                    }
                    Perevod_proizv();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }

                                                                                                         /////////ДОБАВЛЕНИЕ ПЕСНИ СТАВШИЕ СТИХАМИ DONE
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                string Title = Songs_Name.Text.Trim();
                string Music = Songs_Music.Text.Trim();
                string Year = Songs_Year.Text.Trim();

                if (Title != "" && Music != "" && Year != "")
                {

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
                        _FileName = perevod(ofd.SafeFileName);
                        _Output = folder_path + "pesni\\" + _FileName;
                    }
                    if (k == 0)
                    {
                        con.ExecuteNonQuery("insert into yry_apargan_shulukteri (yry_apsh_title,yry_apsh_muz,yry_apsh_text,yry_apsh_god,yry_apsh_uid) values ('" + Title + "','" + Music + "','" + _FileName + "'," + Year + "," + uid + ")");
                        k = 1;
                    }
                    if (k == 1)
                    {
                        if (File.Exists(_Output)) File.Delete(_Output);
                        File.Copy(_Input, _Output);
                    }
                    stih_pes();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }
        }
                                                                                                                        ////////РЕДАКТИРОВАНИЕ ФОТО
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                string _Input = "";
                string _Output = "";
                string _FileName = "";
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = @"C:\";
                ofd.Filter = "Image Files|*.png;*.jpg";
                if (ofd.ShowDialog() == true)
                {
                    _Input = ofd.FileName;
                    _FileName = "photo" + Path.GetExtension(_Input);
                    _Output = folder_path + "\\" + _FileName;
                }
                int k = 0;
                if (k == 0)
                {
                    if (File.Exists(folder_path + "\\" + con.Reader("select chog_photo from chogaalchy where chog_id = " + uid)))
                    {

                        File.Delete(folder_path + "\\" + con.Reader("select chog_photo from chogaalchy where chog_id = " + uid));
                    }
                    con.ExecuteNonQuery("update chogaalchy set chog_photo = '" + _FileName + "' where chog_id = " + uid);
                    k = 1;
                }
                if (k == 1)
                {
                    File.Copy(_Input, _Output);
                }
                string image = con.Reader("select folder from chogaalchy where chog_id = " + uid) + "\\" + con.Reader("select chog_photo from chogaalchy where chog_id = " + uid);
                if (File.Exists(folder_path + "\\" + con.Reader("select chog_photo from chogaalchy where chog_id = " + uid)))
                    Wr_Image.ImageSource = con.GetImage2(folder_path + "\\" + con.Reader("select chog_photo from chogaalchy where chog_id = " + uid));
                else Wr_Image.ImageSource = con.GetImage2(path + "\\all\\img\\default.png");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }

        }
                                                                                                                    ///////РЕДАКТИРОВАНИЕ ИНФОРМАЦИИ О ПИСАТАЛЕ
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {
                int kojuun_id = Kojuun.SelectedIndex + 1;
                string fam = FAM.Text.Trim();
                string imya = Imya.Text.Trim();
                string otch = Otch.Text.Trim();
                string dataroj = Data_rojd.Text.Trim();
                string datasmert = Data_Smerti.Text.Trim();
                string query = "update chogaalchy set chog_fam='" + fam + "',chog_imya='" + imya + "',chog_otch='" + otch + "',chog_datarozh='" + dataroj + "',chog_datasmerti='" + datasmert + "'," + "chog_region_id = " + kojuun_id + " where chog_id = " + uid;
                con.ExecuteNonQuery(query);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + "");
            }
            finally
            {
                MessageBox.Show("Данные писателя успешно изменены.");
            }
        }
        
        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
           
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\";
            ofd.Filter = "Image Files|*.png;*.jpg";
            if (ofd.ShowDialog() == true)
            {
                pis_image = ofd.FileName;
                Writer_Photo = Path.GetExtension(ofd.FileName);
            }
            if (File.Exists(pis_image)) add_img_pis.ImageSource = con.GetImage2(pis_image);
            else add_img_pis.ImageSource = con.GetImage("\\all\\img\\default.png");
        }


        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom("#FF0096D3");
            brush.Freeze();
            b.Background = brush;
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Delete_MouseLeave(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            b.Background = Brushes.White;
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        private void Pass_ChangePreviewMouseUp(object sender, MouseEventArgs e)
        {
            adminPanel.Visibility = Visibility.Collapsed;
            PAGES += "6";
            Back.Visibility = Visibility.Visible;
            PassChangePanel.Visibility = Visibility.Visible;
            try
            {
                NewLogin.Text = con.Reader("select login from Password where id = 1");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "");
            }
          

        }
        private void but_MouseEnter(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            b.Background = new SolidColorBrush(Color.FromRgb(39, 79, 145));
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void but_MouseLeave(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            b.Background = new SolidColorBrush(Color.FromRgb(0, 150, 211));
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        private void but_MouseEnter1(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            b.Background = new SolidColorBrush(Color.FromRgb(39, 79, 145));
            Mouse.OverrideCursor = Cursors.Hand;
        }
        Border bord;

        private void but_MouseLeave1(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            if (bord == dobav_mater)
            {
                dobav_mater.Background = new SolidColorBrush(Color.FromRgb(0, 96, 135));
                udal_mater.Background = new SolidColorBrush(Color.FromRgb(0, 150, 211));
            }
            else if (bord == udal_mater)
            {
                udal_mater.Background = new SolidColorBrush(Color.FromRgb(0, 96, 135));
                dobav_mater.Background = new SolidColorBrush(Color.FromRgb(0, 150, 211));
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        private void Border_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (NewLogin.Text != "" || NewPassword.Password != "")
                {
                    string _hash = "";
                    using (MD5 md5Hash = MD5.Create())
                    {
                        _hash = Code(md5Hash, NewPassword.Password.Trim());
                    }
                    con.ExecuteNonQuery("UPDATE Password SET password = '" + _hash + "' , login = '" + NewLogin.Text.Trim() + "' WHERE id = 1");
                    MessageBox.Show("Логин и пароль успешно изменены");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex+"");
            }
        }
        private string Code(MD5 md5Hash, string input)
        {

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
