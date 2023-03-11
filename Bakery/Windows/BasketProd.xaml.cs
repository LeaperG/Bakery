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
    /// Логика взаимодействия для BasketProd.xaml
    /// </summary>
    public partial class BasketProd : Window
    {
        public BasketProd()
        {
            InitializeComponent();
        }

        private void Page_LoadedBasket(object sender, RoutedEventArgs e)
        {
            List < Basket > baskets = new List<Basket>();
            var user = TempFile.user;
            baskets = EFClass.ContextDB.Basket.Where(i => i.IdClient == user.IdUser).ToList();
            LvProductBasket.ItemsSource = baskets;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ProductList productList = new ProductList();
            this.Close();
            productList.Show();
        }

        private void BtnAddCount_Click(object sender, RoutedEventArgs e)
        {


            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var prodNum = button.DataContext as Basket;
            var prod = ContextDB.Product.ToList();
            var selectProd = prod.FirstOrDefault(i => i.IdProd == prodNum.IdProd );





            if (selectProd.Quantity != 0  && selectProd.Quantity > prodNum.Quantity)
            {
                  //Учётная запись
                  var user = TempFile.user;

                  //Получение списка Корзины с условием
                  var basket = ContextDB.Basket.ToList();
                  var selectedProd = basket.FirstOrDefault(i => i.IdProd == prodNum.IdProd && i.IdClient == user.IdUser);


                //Удаление количества продуктов из таблицы Product
                // selectProd.Quantity += -1;



                  int count = basket.FirstOrDefault(i => i.IdProd == prodNum.IdProd && i.IdClient == user.IdUser).Quantity;
                  selectedProd.Quantity = count + 1;
                  ContextDB.SaveChanges();
                  Page_LoadedBasket(sender, e);
            }
            else
            {
                MessageBox.Show("Извините товар закончился", "Товар", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void BtnMinusCount_Click(object sender, RoutedEventArgs e)
        {
            // TempFile.ProdSelect = LvProductBasket.SelectedItem as Product; // вроде не нужно


            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var prodNum = button.DataContext as Basket;
            var prod = ContextDB.Product.ToList();
            var selectProd = prod.FirstOrDefault(i => i.IdProd == prodNum.IdProd);



                   var user = TempFile.user;
                   var basket = ContextDB.Basket.ToList();
                   var selectedProd = basket.FirstOrDefault(i => i.IdProd == prodNum.IdProd && i.IdClient == user.IdUser);

            if (selectedProd.Quantity != 1)
            {
                   //Для изменения количества продукта в таблице Product
                   //selectProd.Quantity = selectProd.Quantity + 1;



                   if (selectedProd.Quantity == 1)
                   {
                       return;
                   }

                   int count = basket.FirstOrDefault(i => i.IdProd == prodNum.IdProd && i.IdClient == user.IdUser).Quantity;
                   selectedProd.Quantity = count - 1;

                   ContextDB.SaveChanges();

                   Page_LoadedBasket(sender, e);
            }


        }

        private void BtnDell_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Удалить из заказа ?", "Вопрос",
            MessageBoxButton.YesNo, MessageBoxImage.Question);


            if (result == MessageBoxResult.Yes)
            {
                var button = sender as Button;
                if (button == null)
                {
                    return;
                }

                var prodNum = button.DataContext as Basket;
                var prod = ContextDB.Product.ToList();
                var selectProd = prod.FirstOrDefault(i => i.IdProd == prodNum.IdProd);
                //Для изменения количества продукта в таблице Product
                //selectProd.Quantity += prodNum.Quantity;

                EFClass.ContextDB.Basket.Remove(prodNum);
                EFClass.ContextDB.SaveChanges();
                LvProductBasket.ItemsSource = EFClass.ContextDB.Basket.ToList();
                Page_LoadedBasket(sender, e);
            }
        }
    }
}
