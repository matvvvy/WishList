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
    public partial class UserLogin : Page
    {
        public UserLogin()
        {
            InitializeComponent();
        }
        //Логіка входу Користувача
        private void UserExitButton_Click(object sender, RoutedEventArgs e)
        {
            string userName=UserNameBox.Text.Trim();
            //Перевірка на порожнє поле імені
            if(string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Будь ласка, введіть ваше ім'я!");
                return;
            }
            //Збереження імені в базу даних
            DataStorage.CurrentUserName=userName;
            //Перехід до списку подарунків як Користувач
            NavigationService.Navigate(new GiftP(false));
        }
        //Кнопка повернення  на попередню сторінку
        private void BackUserButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}
