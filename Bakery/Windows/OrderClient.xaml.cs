using Bakery.DB;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для OrderClient.xaml
    /// </summary>
    public partial class OrderClient : Window
    {
        public OrderClient()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            OrderList orderList = new OrderList();
            this.Close();
            orderList.Show();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<OrderProd> orderProds= new List<OrderProd>();
            orderProds = ContextDB.OrderProd.ToList();
            orderProds = orderProds.Where(i => i.Order.IdOrder == TempFile.IdOrder).ToList();


            LvProductOrder.ItemsSource = orderProds;
        }
    }
}
