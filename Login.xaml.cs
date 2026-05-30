using System;
using System.Collections.Generic;
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

namespace Курсова_робота
{
    
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();

        }
        //Перехід до входу для Адміна
        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminLogin());
        }
        //Перехід до входу для Користувача
        private void UserButton_Click(Object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserLogin());
        }
    }
}
