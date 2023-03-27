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



            //Отображение количества в корзине
            var basket = ContextDB.Basket.ToList();
            var selectedProd = basket.FirstOrDefault(i => i.IdClient == user.IdUser);

            if (selectedProd != null)
            {

                var prod = ContextDB.Product.ToList();
                var selectProd = prod.FirstOrDefault(i => i.IdProd == selectedProd.IdProd);

                var b = selectedProd.Product.Cost - selectedProd.Product.Cost;
                var g = b;
                var sum = g;
                int a = 0;
                string str = string.Empty;

                while (a < baskets.Count)
                {
                    selectedProd = baskets[a];
                    b += selectedProd.Product.Cost;
                    g += selectedProd.Quantity;
                    sum += b * g;
                    a++;
                    b = 0;
                    g = 0;
                }
                TempFile.Summ = sum;
                str = Convert.ToString(sum);
                TXBCostOrder.Text = str.Substring(0, str.Length -2) + " Руб";
            }


            if (TempFile.ChekNew.Contains(5))
            {
                EFClass.ContextDB.Basket.RemoveRange(baskets);
                EFClass.ContextDB.SaveChanges();
                LvProductBasket.ItemsSource = EFClass.ContextDB.Basket.ToList();
                TempFile.ChekNew.Clear();
                Page_LoadedBasket(sender, e);
               // return;
            }
            else
            {
                LvProductBasket.ItemsSource = baskets;
            }
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

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var basket = ContextDB.Basket.ToList();

            var user = TempFile.user;


            List<Basket> baskets= new List<Basket>();
            baskets = ContextDB.Basket.Where(i => i.IdProd >= 1 && i.IdClient == user.IdUser).ToList();
            var selectedProd = basket.FirstOrDefault(i => i.IdClient == user.IdUser);
            int a = 0;

            if (selectedProd == null)
            {
                MessageBox.Show("Корзина пуста", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var prod = ContextDB.Product.ToList();
            var selectProd = prod.FirstOrDefault(i => i.IdProd == selectedProd.IdProd);

            int c = 0;
         //   selectProd = prod[0];
            for (int j = 0; c < baskets.Count; j++)
            {
                if (selectProd.IdProd == selectedProd.IdProd)
                {
                    selectProd.Quantity += -selectedProd.Quantity;
                    c++;

                    if (baskets.Count > c ) //Чтобы индекс не переходил за размера массива
                    {
                        selectedProd = baskets[c];

                    }
                    j = 0;
                }
                if (prod.Count > j)
                {
                    selectProd = prod[j];
                }
            }




            if (baskets.Count <= 0)
            {
                OrderClient orde = new OrderClient();
                this.Close();
                orde.ShowDialog();
                return;
            }
            
            Order order = new Order()
            {
                IdClient = TempFile.user.IdUser,
                IdStaff = 1,
                OrderDate = DateTime.Now,
            };

            ContextDB.Order.Add(order);

            //var b = selectedProd.Product.Cost - selectedProd.Product.Cost;
            //var g = b;
            //var sum = g;
            while (a < baskets.Count)
            {
                //selectProd = prod[selectedProd.IdProd];
                //selectProd.Quantity = selectProd.Quantity-1;

                selectedProd = baskets[a];
                //b += selectedProd.Product.Cost;
                //g += selectedProd.Quantity;
                //sum += b * g;

                if (/*selectedProd.IdProd < basket.Count*/ true) // Не помню зачем ?
                {




                    OrderProd orderProd = new OrderProd()
                    {
                        IdOrder = order.IdOrder,
                        IdProd = selectedProd.IdProd,
                        Quantity = selectedProd.Quantity,
                    };


                    //order.Cost = sum;
                    order.Cost = TempFile.Summ;

                    ContextDB.OrderProd.Add(orderProd);
                    ContextDB.SaveChanges();

                }
                a++;
                //b = 0;
                //g = 0;
            }

            TempFile.ChekNew.Add(5);
            TempFile.ChekNew.Add(6);

            Page_LoadedBasket(sender, e);
            OrderList ord = new OrderList();
            this.Close();
            ord.ShowDialog();
        }
    }
}
