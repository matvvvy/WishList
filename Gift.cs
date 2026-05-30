using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Курсова_робота
{
    //Модель даних для подарунків
    public class Gift
    {
        public int Id {  get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public string? Url { get; set; }
       public byte[]? ImageData {  get; set; }//Фото у форматі байтів для бази даних

        [NotMapped]//Не зберігати це поле в базі
        public BitmapImage? Image { get; set; }
        public bool Reserved { get; set; }
        public string? ReservedBy { get; set; }
        public string Status { get; set; } = "Вiльно";
        public bool Shared { get; set; } = false;//Чи є подарунок спільним
        public int MaxPeople { get; set; } = 1;
        public int CurrentPeopleC { get; set; } = 0;

    }
    //Контекст бази даних
    public class WishlistContext : DbContext
    {
        public DbSet<Gift> Gifts { get; set; }//Таблиця подарунків

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Налаштування SQLite
            optionsBuilder.UseSqlite("Data Source=wishlist.db");
        }
    }
    //статичне сховище та методи роботи з данимим
    public static class DataStorage
    {
        public static string CurrentUserName { get; set; } = "Дарувальник";
        //Створення бази,якщо її не існує
        public static void InitializeDatabase()
        {
            using(var db=new WishlistContext())
            {
                db.Database.EnsureCreated();
            }
        }
        //Отримання списку всіх подарунків
       public static List<Gift> GetGifts()
       {
            using(var db=new WishlistContext())
            {
                return db.Gifts.ToList();
            }
       }
        //Додавання нового запису
        public static void AddGift(Gift gift)
        {
            using(var db = new WishlistContext())
            {
                db.Gifts.Add(gift);
                db.SaveChanges();
            }
        }
        //Оновлення існуючого запису
        public static void UpdateGift(Gift gift)
        {
            using (var db = new WishlistContext())
            {
                db.Gifts.Update(gift);
                db.SaveChanges();
            }
        }
        //Видалення подарунку
        public static void DeleteGift(Gift gift)
        {
            using (var db = new WishlistContext())
            {
                db.Gifts.Remove(gift);
                db.SaveChanges();
            }
        }
    }
}
