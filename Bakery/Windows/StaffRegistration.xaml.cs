using Bakery.DB;
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

namespace Bakery.Windows
{
    /// <summary>
    /// Логика взаимодействия для StaffRegistration.xaml
    /// </summary>
    public partial class StaffRegistration : Window
    {
        public StaffRegistration()
        {
            InitializeComponent();
            CMBGender.ItemsSource = ContextDB.Gender.ToList();
            CMBGender.DisplayMemberPath = "GenderName";

            CMBRole.ItemsSource = ContextDB.Position.ToList();
            CMBRole.DisplayMemberPath = "PositionName";
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            if (EmailBox.Text == string.Empty || PhoneBox.Text == string.Empty || LoginBox.Text == string.Empty || PassBox.Password == string.Empty || VerPassBox.Password == string.Empty  || FirstNameBox.Text == string.Empty 
                || LastNameBox.Text == string.Empty || BirthdayBox.Text == string.Empty || MedPolisBox.Text == string.Empty || SalaryBox.Text == string.Empty || CMBGender.SelectedIndex < 0 || CMBRole.SelectedIndex < 0)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (!Regex.IsMatch(EmailBox.Text, @"^\w+@\w+\.\w{2,3}$"))
            {
                MessageBox.Show("Почта не верна!");
                return;
            }


            if (!Regex.IsMatch(PhoneBox.Text, @"^[0-9]{11}$"))
            {
                MessageBox.Show("Телефон не верен!");
                return;
            }


            if (PassBox.Password != VerPassBox.Password)
            {
                MessageBox.Show("Пароли не совподают!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }


            if (PassBox.Password.Length < 8 && VerPassBox.Password.Length < 8)
            {
                MessageBox.Show("Пароль должен быть минимум из 8 символов!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }


            var accounts = ContextDB.UserAccount.ToList();
            var CheckUser = accounts.FirstOrDefault(i => (i.LoginName.ToLower() == LoginBox.Text.ToLower()));

            if (CheckUser != null)
            {
                MessageBox.Show("Такой логин есть", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            CheckUser = accounts.FirstOrDefault(i => (i.Email.ToLower() == EmailBox.Text.ToLower()));

            if (CheckUser != null)
            {
                MessageBox.Show("Такая почта уже есть", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }


            CheckUser = accounts.FirstOrDefault(i => (i.Phone.Contains(PhoneBox.Text)));

            if (CheckUser != null)
            {
                MessageBox.Show("Такой номер телефона уже есть", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (CMBRole.SelectedIndex <= 0)
            {
                MessageBox.Show("Выберите роль", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (CMBGender.SelectedIndex <= 0)
            {
                MessageBox.Show("Выберите пол", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }




            // Рандомный Пароль для Востановления
            Random rnd = new Random();
            rnd.Next(1000, 9999);
            string str = "";
            const string symbols = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";


            for (int i = 0; i < 4; i++)
            {
                str += symbols[rnd.Next(0, 62)];
            }


            UserAccount user = new UserAccount()
            {
                LoginName = LoginBox.Text.Trim(),
                Email = EmailBox.Text.Trim(),
                Phone = PhoneBox.Text.Trim(),
                Password = PassBox.Password.Trim(),
                PasswordDate = DateTime.Now,
                PasswordRecovery = str,
                IdRole = 2
            };


            Staff staff = new Staff()
            {
                IdUser = user.IdUser,
                FirstName = FirstNameBox.Text.Trim(),
                LastName = LastNameBox.Text.Trim(),
                Patronymic = PatronymicBox.Text.Trim(),
                GenderCode = CMBGender.SelectedIndex + 1,
                Brithday = Convert.ToDateTime(BirthdayBox.Text.Trim()),
                IdPosition = CMBRole.SelectedIndex+1,
                MedPolis = MedPolisBox.Text.Trim(),
                Salary =  Convert.ToDecimal(SalaryBox.Text.Trim())
        };

            ContextDB.Staff.Add(staff);
            ContextDB.UserAccount.Add(user);
            ContextDB.SaveChanges();
            MessageBox.Show("Успешно сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);


            Auth auth = new Auth();
            this.Close();
            auth.Show();
        }
    }
}
