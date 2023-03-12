using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
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
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            this.Close();
            reg.Show();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            string login = LogBox.Text;
            string password = PBox.Password;

            if (login == string.Empty || password == string.Empty)
            {
                MessageBox.Show("Введите данные!");
                return;
            }

            var accounts = ContextDB.UserAccount.ToList();
            var user = accounts.FirstOrDefault(i => (i.LoginName == login));

            TempFile.user = user;

            if (user == null)
            {
                MessageBox.Show("Такого пользователя с логином не существует!");
                return;
            }

            if (user.IdRole == 5)
            {
                MessageBox.Show("Упс вы забанены, Нам НЕ жаль:)!", "Вход", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            if (PBox.Password == user.Password)
            {
                //MessageBox.Show("Вход успешно выполнен!", "Вход", MessageBoxButton.OK, MessageBoxImage.Information);
                ProductList win = new ProductList();
                this.Close();
                win.Show();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            //Recovery rec = new Recovery();
            //this.Close();
            //rec.Show();
        }
    }
}
