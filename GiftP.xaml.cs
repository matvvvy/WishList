using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
    //Конструктор за замовчуванням
    public partial class GiftP : Page
    {
        public GiftP()
        {
            InitializeComponent();
            GiftsDataGrid.ItemsSource = DataStorage.GetGifts();
            //Права адміна за замовчуванням
            bool defaultAdminStatus = true;
            SetPermissions(defaultAdminStatus);
            UpdateEmptyState();
            //Оновлення даних при завантаженні сторінки
            this.Loaded += (s, e) =>
            {
                GiftsDataGrid.ItemsSource = DataStorage.GetGifts();
                UpdateEmptyState();
            };
        }
        //Конструктор з вибором ролі(Адмін/Користувач)
        public GiftP(bool isAdmin)
        {
            InitializeComponent();

            GiftsDataGrid.ItemsSource = DataStorage.GetGifts();
    
            SetPermissions(isAdmin);

            UpdateEmptyState();
            this.Loaded += (s, e) =>
            {
                GiftsDataGrid.ItemsSource = DataStorage.GetGifts();
                UpdateEmptyState();
            };
        }
        //Перемикання видимості: таблиця або початковий екран
        private void UpdateEmptyState()
        {
            var currentGifts = DataStorage.GetGifts();
            if(currentGifts != null && currentGifts.Count > 0)
            {
                GiftsDataGrid.Visibility= Visibility.Visible;
                StartPanel.Visibility= Visibility.Collapsed;
            }
            else
            {
                GiftsDataGrid.Visibility=Visibility.Collapsed;
                StartPanel.Visibility=Visibility.Visible;
            }
        }
        //Налаштування інтерфейсу під роль користувача
        private void SetPermissions(bool isAdmin)
        {
            if (isAdmin)
            {
                //Елементи керування для адміна
                AddGiftButton.Visibility = Visibility.Visible;
                DeleteGiftButton.Visibility = Visibility.Visible;
                CancelRes.Visibility = Visibility.Collapsed;
                ReservedButton.Visibility = Visibility.Collapsed;

                if (ReservedColumn != null)

                {
                    ReservedColumn.Visibility = Visibility.Collapsed;
                }
                if (GiverColumn != null)
                {
                    GiverColumn.Visibility = Visibility.Collapsed;
                }
                if (AdminStartPanel != null)
                {
                    AdminStartPanel.Visibility = Visibility.Visible;
                }
                if (UserStartPanel != null)
                {
                    UserStartPanel.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                //Елементи керування для звичайного користувача
                AddGiftButton.Visibility = Visibility.Collapsed;
                DeleteGiftButton.Visibility = Visibility.Collapsed;
                CancelRes.Visibility = Visibility.Visible;

                ReservedButton.Visibility = Visibility.Visible;

                if (ReservedColumn != null)
                {
                    ReservedColumn.Visibility = Visibility.Visible;
                }
                if (GiverColumn != null)
                {
                    GiverColumn.Visibility = Visibility.Visible;
                }
                if(AdminStartPanel != null)
                {
                    AdminStartPanel.Visibility = Visibility.Collapsed;
                }
                if(UserStartPanel != null)
                {
                    UserStartPanel.Visibility = Visibility.Visible;
                }
            }
        }
        //Кнопка виходу
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        //Перехід до додавання подарунку
        private void AddGiftButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddGiftP());
        }
        //Видалення подарунку
        private void DeleteGiftButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = GiftsDataGrid.SelectedItem as Gift;

            if (selected != null)
            {
                DataStorage.DeleteGift(selected);

                GiftsDataGrid.ItemsSource = DataStorage.GetGifts();

                UpdateEmptyState();
            }
            else
            {
                MessageBox.Show("Спочатку виберіть подарунок для видалення.");
            }
        }
        //Логіка бронювання(індивідуальне або спільне)
        private void ReservedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGift = GiftsDataGrid.SelectedItem as Gift;

            if (selectedGift != null)
            {
                if (selectedGift.Reserved == true)
                {
                    MessageBox.Show("Цей подарунок вже хтось забронював! Оберіть інший.");
                    return;
                }
                if (selectedGift.Shared == true)
                {
                    //Обробка спільного подарунку(збільшиння лічильника)
                    selectedGift.CurrentPeopleC++;

                    if (string.IsNullOrEmpty(selectedGift.ReservedBy))
                    {
                        selectedGift.ReservedBy = DataStorage.CurrentUserName;
                    }
                    else
                    {
                        selectedGift.ReservedBy = DataStorage.CurrentUserName;
                    }
                    if (selectedGift.CurrentPeopleC >= selectedGift.MaxPeople)
                    {
                        selectedGift.Reserved = true;
                        selectedGift.Status = "Забронювано";
                    }
                    else
                    {
                        int left = selectedGift.MaxPeople - selectedGift.CurrentPeopleC;
                        selectedGift.Status = "Шукаю напарника(ще: " + left + ")";
                    }

                }
                else
                {
                    //Індивідуальне броювання
                    selectedGift.Reserved = true;
                    selectedGift.ReservedBy = DataStorage.CurrentUserName;
                    selectedGift.Status = "Забронювано";
                }

                DataStorage.UpdateGift(selectedGift);
                GiftsDataGrid.ItemsSource = DataStorage.GetGifts();

                MessageBox.Show($"Ви забронювали: {selectedGift.Name}!");
            }
            else
            {
                MessageBox.Show("Спочатку оберіть подарунок у списку.");
            }
        }
        //Скасування бронювання
        private void CancelRes_Click(object sender, RoutedEventArgs e)
        {
            var selectedGift = GiftsDataGrid.SelectedItem as Gift;

            if (selectedGift != null && selectedGift.Reserved == true)
            {
                selectedGift.Reserved = false;
                selectedGift.ReservedBy = "";
                selectedGift.CurrentPeopleC = 0;
                selectedGift.Status = selectedGift.Shared ? "ШУкаю напарника" : "Вільно";

                DataStorage.UpdateGift(selectedGift);
                GiftsDataGrid.ItemsSource = DataStorage.GetGifts();

                MessageBox.Show("Бронь сказовано!");
            }
            else
            {
                MessageBox.Show("Оберіть заброньований подарунок у списку!");
            }
        }
        //Відкриття вікна з деталями подаранку
        private void OpenDetali_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            var gift = button.DataContext as Gift;
            if (gift != null)
            {
                DetaliGift detaliWindow = new DetaliGift(gift);

                detaliWindow.ShowDialog();
            }
        }
    }
}
