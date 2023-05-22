using Bakery.DB;
using Bakery.HellperClass;
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
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {

        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По имени (по возрастанию)",
            "По имени (по убыванию)",
            "По типу (по возрастанию)",
            "По типу (по убыванию)",
        };




        public Administrator()
        {
            InitializeComponent();
            CMBFilter.ItemsSource = ContextDB.Role.Where(i => i.IdRole != 3).ToList();
            CMBFilter.SelectedIndex = -1;
            CMBFilter.DisplayMemberPath = "RoleName";


            if (CMBFilter.SelectedIndex == -1)
            {
                CMBFilter.Text = "По умолчанию";
            }

            CMBSorting.ItemsSource = listSort;
            CMBSorting.SelectedIndex = 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //List<UserAccount> users = new List<UserAccount>();
            //users = ContextDB.UserAccount.ToList();
            List<UserAccount> AllUsers = new List<UserAccount>();

            AllUsers = ContextDB.UserAccount.ToList();
            var users = AllUsers.Where(i => i.IdUser !=1).ToList();

            //Поиск
            users = users.Where(i => i.LoginName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();



            if (TempFile.ChekNew.Contains(4))
            {
                users = users.Where(i => i.IdRole == (CMBFilter.SelectedItem as Role).IdRole).ToList();
            }


            var selectedIndexCmb = CMBSorting.SelectedIndex;

            switch (selectedIndexCmb)
            {
                case 0:
                    users = users.OrderBy(i => i.IdUser).ToList();
                    break;


                case 1:
                    users = users.OrderBy(i => i.LoginName.ToLower()).ToList();
                    break;


                case 2:
                    users = users.OrderByDescending(i => i.LoginName.ToLower()).ToList();
                    break;


                case 3:
                    users = users.OrderBy(i => i.Role.RoleName.ToLower()).ToList();
                    break;


                case 4:
                    users = users.OrderByDescending(i => i.Role.RoleName.ToLower()).ToList();
                    break;

              
                default:
                    break;
            }
      
            //Итоговый спсиок
            LvUser.ItemsSource = users;
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            TempFile.UserSelect = LvUser.SelectedItem as UserAccount; // вроде не нужно

            if (LvUser.SelectedItem == null)
            {
                return;
            }

            Page_Loaded(sender, e);
            EditUser userEdit = new EditUser();
           // this.Close();
            userEdit.ShowDialog();
            Page_Loaded(sender, e);

        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            TempFile.ChekNew.Clear();
            Auth auth = new Auth();
            this.Close();
            auth.ShowDialog();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Page_Loaded(sender, e);
        }

        private void CMBFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            if (CMBFilter.SelectedIndex != -1)
            {
                TempFile.ChekNew.Add(4);
            }

            Page_Loaded(sender, e);
        }

        private void CMBSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Page_Loaded(sender, e);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TempFile.ChekNew.Clear();
            List<UserAccount> AllUsers = new List<UserAccount>();
            AllUsers = ContextDB.UserAccount.ToList();
            var users = AllUsers.Where(i => i.IdUser != 1).ToList();
            LvUser.ItemsSource = users;
            TbSearch.Clear();
            CMBFilter.SelectedIndex = -1;
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            TempFile.ChekNew.Add(7);
            OrderList orderList= new OrderList();
            this.Close();
            orderList.ShowDialog();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            TempFile.ChekNew.Clear();
            ProductList productList = new ProductList();
            this.Close();
            productList.ShowDialog();
        }

        private void RegStaff_Click(object sender, RoutedEventArgs e)
        {
            StaffRegistration staffRegistration = new StaffRegistration();
            staffRegistration.ShowDialog();
        }
    }
}
