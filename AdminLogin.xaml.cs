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
    
    public partial class AdminLogin : Page
    {
        public AdminLogin()
        {
            InitializeComponent();
        }
        //Логіка входу для Адміна
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            //Перевірка паролю
            if(PasswordBox.Password=="1234")
            {
                NavigationService.Navigate(new GiftP(true));
            }
            else
            {
                MessageBox.Show("Неправильний пароль!(Спрообуйте 1234)");
            }
        }
        //Кнопка виходу на попередня
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}
