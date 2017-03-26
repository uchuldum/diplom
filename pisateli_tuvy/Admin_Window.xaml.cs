using System.Windows;

namespace pisateli_tuvy
{
    /// <summary>
    /// Interaction logic for Admin_Window.xaml
    /// </summary>
    public partial class Admin_Window : Window
    {
        public Admin_Window()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Enter enter = new Enter();
            enter.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
