using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    
    public partial class AddGiftP : Page
    {
        
        public AddGiftP()
        {
            InitializeComponent();
        }
        //Збереження нового подарунку
        private void SaveGiftButton_Click(object sender, RoutedEventArgs e)
        {
            //Перевірка на порожнє ім'я
            if (string.IsNullOrEmpty(NameInput.Text))
            {
                MessageBox.Show("Введіть назву подарунку!");
                return;
            }
            //Створення та заповнення об'єкта
            Gift newGift= new Gift();

            newGift.Name = NameInput.Text;
            newGift.Category= CategoryInput.Text;
            newGift.Url= UrlInput.Text;
            //Робота із зображенням
            if (GiftImagePreview.Source is BitmapImage bitmap)
            {
                newGift.Image = bitmap; 
                newGift.ImageData = ImageToByteArray(bitmap); 
            }
            //З ціною
            decimal.TryParse(PriceInput.Text, out decimal price);
            newGift.Price= price;
            //Логіка для спільного подарунку
            if (SharedCheckBox.IsChecked == true)
            {
                newGift.Shared = true;
                int.TryParse(MaxPeopleTextBox.Text, out int people);
                newGift.MaxPeople = people > 0 ? people : 1;
                newGift.Status = "Шукаю напарника";
            }
            else
            {
                newGift.Shared = false;
                newGift.MaxPeople = 1;
                newGift.Status = "Вільно";

            }
            //Збереження в список та повернення назад
            newGift.Reserved = false;
            newGift.CurrentPeopleC = 0;

            DataStorage.AddGift(newGift);
            NavigationService.GoBack();

        }
        //Виюір картинки через діалоговк вікно
        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog= new OpenFileDialog();
            dialog.Filter = "Image|*.jpg;*png;*jpeg";
            if (dialog.ShowDialog() == true)
            {
                GiftImagePreview.Source=new BitmapImage(new Uri(dialog.FileName));
            }
        }
        //Кнопка виходу
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        //Конвертація картинки у масив байтів для бази даних
        private byte[] ImageToByteArray(BitmapImage image)
        {
            if (image == null) return null;
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }
    }

}
