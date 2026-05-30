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
using System.Windows.Shapes;

namespace Курсова_робота
{
   
    public partial class DetaliGift : Window
    {
        public DetaliGift(Gift gift)
        {
            InitializeComponent();

            //Відображення базових даних
            DetaliName.Text = gift.Name;
            DetaliPrice.Text = gift.Price.ToString() + "грн";
            //Перевірка та завантаження з масиву байтів
            if (gift.Image == null && gift.ImageData != null)
            {
                gift.Image = ByteArrayToImage(gift.ImageData);
            }
            DetaliImage.Source = gift.Image;
            //Встановлення статтусу(спільниий або індивідуальний
            if (gift.Shared==true )
            {
                int left = gift.MaxPeople - gift.CurrentPeopleC;
                DetaliStatus.Text = "Спільний подарунок.Залишиось місць: " + left;
            }
            else
            {
                DetaliStatus.Text = gift.Reserved ? $"Заброньовано {gift.ReservedBy}" : "Вільний";
            }
            //Збереження посилання в тег для подальшого користування
            DetaliLink.Tag = gift.Url;
        }
        //Конвертація байтів назад у зображення для відображення
        private BitmapImage ByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;

            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze(); 
            return image;
        }
        //Відкриття посилання у браузері
        private void DetaliLink_Click(object sender, RoutedEventArgs e)
        {
           string url=DetaliLink.Tag?.ToString()??"";
            if (string.IsNullOrWhiteSpace(url)) return;

            try
            {
                if (!url.StartsWith("http")) url = "https://" + url;
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
                MessageBox.Show("Помилка: не вдалося відкрити посилання!");
            }
        }
        //Закриття вікна
        private void CloseDetali_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 
    }
}
