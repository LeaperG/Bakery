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
            ProductList productList = new ProductList();
            this.Close();
            productList.Show();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
