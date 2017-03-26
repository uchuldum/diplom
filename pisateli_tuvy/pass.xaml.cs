using System;
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

namespace pisateli_tuvy
{
    /// <summary>
    /// Interaction logic for pass.xaml
    /// </summary>
    public partial class pass : Window
    {
        public pass()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            string passwrd = "1234";
            string log = "admin";
            if (login.Text == log && password.Password == passwrd)
            {
                Admin_Window win = new Admin_Window();
                win.Show();
                this.Close();

            }
            else MessageBox.Show("Неверный пароль или логин!");
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
