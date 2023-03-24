using Bakery.HellperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Bakery.HellperClass.EFClass;
using Bakery.HellperClass;

namespace Bakery.Windows
{
    /// <summary>
    /// Логика взаимодействия для Personal.xaml
    /// </summary>
    public partial class Personal : Window
    {
        public Personal()
        {
            InitializeComponent();
            CMBGender.ItemsSource = ContextDB.Gender.ToList();
            CMBGender.DisplayMemberPath = "GenderName";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            int gender = 0;
            var user = TempFile.user;

            var client = TempFile.client;


            if (!Regex.IsMatch(BirthdayBox.Text, @"^\d{2}\.\d{2}\.\d{4}$") && BirthdayBox.Text != "")
            {
                MessageBox.Show("Дата рождения введена не правильно. Введите День/Месяц/Год", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }


            client.GenderCode = CMBGender.SelectedIndex+1;

            //if (GenderBox.Text.ToLower() == "м" || GenderBox.Text.ToLower() == "ж")
            //{

            //    if (GenderBox.Text.ToLower() == "м")
            //    {
            //        gender = 1;
            //    }
            //    else
            //    {
            //        gender = 2;
            //    }

            //}
            //else if (GenderBox.Text.ToLower() != "")
            //{
            //    MessageBox.Show("Пол введён не правильно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            //    return;

            //}


            if (FirstNameBox.Text != "")
            {
                 client.FirstName = FirstNameBox.Text.Trim();

            }

            if (LastNameBox.Text != "")
            {
                client.LastName = LastNameBox.Text.Trim();
            }

            if (PatronymicBox.Text != "")
            {
                client.Patronymic = PatronymicBox.Text.Trim();
            }

            if (gender != 0)
            {
                client.GenderCode = gender;
            }

            if (BirthdayBox.Text != "")
            {
                try
                {
                    client.Brithday = Convert.ToDateTime(BirthdayBox.Text);

                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка!");
                    return;
                }
            }



            ContextDB.SaveChanges();
            MessageBox.Show("Успешно сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ProductList productList = new ProductList();
            this.Close();
            productList.Show(); 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var user = TempFile.user;
            var clientAll = ContextDB.Client.ToList();
            var client = clientAll.FirstOrDefault(i => (i.IdUser == user.IdUser));

            TempFile.client = client;

            var gender = ContextDB.Gender.ToList();
            var gen = gender.FirstOrDefault(i => (i.GenderCode == client.GenderCode));

            FirstNameBox.Text = client.FirstName;
            LastNameBox.Text = client.LastName;
            PatronymicBox.Text = client.Patronymic;

            if (client.Brithday != null)
            {
                BirthdayBox.Text = client.Brithday.Value.ToShortDateString();
            }

            if (client.GenderCode != null)
            {
                CMBGender.SelectedIndex = client.GenderCode.Value-1;
            }
            //if (gen != null)
            //{
            //    GenderBox.Text = gen.GenderName;
            //}

        }
    }
}
