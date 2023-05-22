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
    /// Логика взаимодействия для OrderList.xaml
    /// </summary>
    public partial class OrderList : Window
    {

        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По дате (по возрастанию)",
            "По дате (по убыванию)",
            "По цене (по возрастанию)",
            "По цене (по убыванию)",
            "По новизне (по возрастанию)",
            "По новизне (по убыванию)"
        };



        public OrderList()
        {
            InitializeComponent();

            DataPickerFrom.SelectedDate = DateTime.Now.Date;
            CMBSorting.ItemsSource = listSort;
            CMBSorting.SelectedIndex = 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Order> ord = new List<Order>();
            var user = TempFile.user;
            if (!TempFile.ChekNew.Contains(7))
            {
                ord = ContextDB.Order.Where(i => i.IdClient == user.IdUser).ToList();

            }
            else
            {
                ord = ContextDB.Order.ToList();
            }
            var selectProd = ord.FirstOrDefault(i => i.IdOrder >= 1);

            //Номера заказов: 1,2,3... и т.д
            for (int i = 0; i < ord.Count; i++)
            {
                selectProd = ord[i];
                selectProd.OrderNumber = selectProd.IdOrder - selectProd.IdOrder + i + 1;
            }


            //Поиск
            if (TbSearch.Text != "")
            {
                ord = ord.Where(i => Convert.ToString(i.IdOrder).ToLower().Contains(TbSearch.Text.ToLower())).ToList();
            }


            var selectedIndexCmb = CMBSorting.SelectedIndex;

            switch (selectedIndexCmb)
            {
                case 0:
                    ord = ord.OrderByDescending(i => i.IdOrder).ToList();
                    break;


                case 1:
                    ord = ord.OrderBy(i => i.OrderDate).ToList();
                    break;


                case 2:
                    ord = ord.OrderByDescending(i => i.OrderDate).ToList();
                    break;


                case 3:
                    ord = ord.OrderBy(i => i.Cost).ToList();
                    break;


                case 4:
                    ord = ord.OrderByDescending(i => i.Cost).ToList();
                    break;


                case 5:
                    //products = products.OrderBy(i =>Convert.ToInt32(i.Cost)).ToList();
                    ord = ord.OrderBy(i => i.OrderNumber).ToList();
                    break;


                case 6:
                    ord = ord.OrderByDescending(i => i.OrderNumber).ToList();
                    break;

                default:
                    break;
            }

            //string date = "28.03.2023";
            //string LastDate = "0.04.2023";
            //var x = Convert.ToDateTime(LastDate);
            //var c = Convert.ToDateTime(date);
            //List<Order> ord1 = new List<Order>();
            //ord1

            //DataPickerFrom.SelectedDate = DateTime.Now.Date;
            //var DateFrom1 = Convert.ToDateTime(DataPickerFrom.Text);
            //if (DateFrom1.Day == DateTime.Now.Day)
            //{
            //    ord = ord.Where(i => i.OrderDate >= DateFrom1).ToList();
            //}
           if (TempFile.ChekNew.Contains(8) && TempFile.ChekNew.Contains(9) && DataPickerFrom.Text != "" && DataPickerBefore.Text != "")
            {
                var DateFrom = Convert.ToDateTime(DataPickerFrom.Text);
                var DateBefore = Convert.ToDateTime(DataPickerBefore.Text);

                ord = ord.Where(i => i.OrderDate >= DateFrom && i.OrderDate <= DateBefore).ToList();
            }
            else if (TempFile.ChekNew.Contains(8) && DataPickerFrom.Text != "")
            {
                var DateFrom = Convert.ToDateTime(DataPickerFrom.Text);
                ord = ord.Where(i => i.OrderDate >= DateFrom).ToList();
            }
            else if(TempFile.ChekNew.Contains(9) && DataPickerBefore.Text != "")
            {
                var DateBefore = Convert.ToDateTime(DataPickerBefore.Text);
                ord = ord.Where(i =>  i.OrderDate <= DateBefore).ToList();
            }


            //if (ord.Count == 0 || ord.Count == 1)
            //{
            //    return;
            //}
          //  TempFile.ChekNew.Clear();

            LvOrders.ItemsSource = ord;

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            TempFile.ChekNew.Clear();
            ProductList productList = new ProductList();
            this.Close();
            productList.Show();
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var prodNum = button.DataContext as Order;
            var prod = ContextDB.Order.ToList();
            var selectProd = prod.FirstOrDefault(i => i.IdOrder == prodNum.IdOrder);

            TempFile.IdOrder = prodNum.IdOrder;
            OrderClient orderClient = new OrderClient();
            this.Close();
            orderClient.ShowDialog();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Page_Loaded(sender, e);
        }

        private void CMBSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Page_Loaded(sender, e);
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            List<Order> ord = new List<Order>();
            var user = TempFile.user;
            if (!TempFile.ChekNew.Contains(7))
            {
                ord = ContextDB.Order.Where(i => i.IdClient == user.IdUser).ToList();
            }
            else
            {
                ord = ContextDB.Order.ToList();
            }

            TbSearch.Clear();
            CMBSorting.SelectedIndex = 0;
            //TempFile.ChekNew.Clear();
            ord = ord.OrderByDescending(i => i.IdOrder).ToList();
            DataPickerFrom.SelectedDate = null;
            DataPickerBefore.SelectedDate = null;
            LvOrders.ItemsSource = ord;
        }

        private void DataPickerFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TempFile.ChekNew.Add(8);
            Page_Loaded(sender, e);
        }

        private void DataPickerBefore_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TempFile.ChekNew.Add(9);
            Page_Loaded(sender, e);
        }
    }
}
