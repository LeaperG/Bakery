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
using Bakery.DB;
using System.IO;

namespace Bakery.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        public EditUser()
        {
            InitializeComponent();
            CMBUserRole.ItemsSource = ContextDB.Role.Where(i => i.IdRole != 3).ToList();
            CMBUserRole.SelectedIndex = 0;
            CMBUserRole.DisplayMemberPath = "RoleName";
            GetListUser();
        }


        private void GetListUser()
        {
            var user = TempFile.UserSelect;
            TbLoginName.Text = user.LoginName.ToString();
            TbEmail.Text = user.Email.ToString();
            TbPhone.Text = user.Phone.ToString();
            TbPassword.Text = user.Password.ToString();
            CMBUserRole.SelectedItem = ContextDB.Role.Where(i => i.IdRole == user.IdRole).FirstOrDefault();

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
     
            var user = TempFile.UserSelect;
            user.LoginName = TbLoginName.Text;
            user.Email = TbEmail.Text;
            user.Phone = TbPhone.Text;
            user.Password = TbPassword.Text;
            user.IdRole = (CMBUserRole.SelectedItem as Role).IdRole;
            user.PasswordDate= DateTime.Now;
            user.PasswordRecovery = "5";

            if (user.IdRole != 1)
            {

                   MessageBoxResult result = MessageBox.Show("Мой Хозяин вы уверены ? Тип роли правильный ?", "Потверждение",
                   MessageBoxButton.YesNo, MessageBoxImage.Question);
                   if (result == MessageBoxResult.No)
                   {
                       return;
                   }

                   MessageBoxResult confirmation = MessageBox.Show("Не примите за дерзость мой Повелитель, вы точно уверены ? Тип роли точно правильный ?", "Потверждение",
                   MessageBoxButton.YesNo, MessageBoxImage.Question);

                   if (confirmation == MessageBoxResult.No)
                   {
                       return;
                   }
            }

            ContextDB.SaveChanges();
            this.Close();
        }
    }
}
