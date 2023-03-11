using Bakery.DB;
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
using System.IO;

namespace Bakery.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductList.xaml
    /// </summary>
    public partial class ProductList : Window
    {

        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По имени (по возрастанию)",
            "По имени (по убыванию)",
            "По типу (по возрастанию)",
            "По типу (по убыванию)",
            "По цене (по возрастанию)",
            "По цене (по убыванию)",
            "По количеству (по возрастанию)",
            "По количеству (по убыванию)"
        };

        public ProductList()    
        {
            InitializeComponent();
            CMBFilter.ItemsSource = ContextDB.ProductType.ToList();
            CMBFilter.SelectedIndex = -1;
            CMBFilter.DisplayMemberPath = "TypeName";

            if(CMBFilter.SelectedIndex == -1)
            {
                CMBFilter.Text = "Выберите вариант";
            }

            CMBSorting.ItemsSource = listSort;
            CMBSorting.SelectedIndex = 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<Product> products = new List<Product>();
            products = ContextDB.Product.ToList();

            

            var user = TempFile.user;
            var basket = EFClass.ContextDB.Basket.Where(i => i.IdClient == user.IdUser).ToList();

            foreach (var prod in products)
            {
                if (basket.FirstOrDefault(i => i.IdProd == prod.IdProd) != null)
                {
                    prod.InBasket = Visibility.Visible;
                }
                else
                {
                    prod.InBasket = Visibility.Hidden;
                }
            }


            // поиск, сортировка, фильтрация


            //Поиск
            products = products.Where(i => i.ProdName.ToLower().Contains(TbSearch.Text.ToLower())).ToList();

            //Фильтр
            var product = TempFile.ProdSelect; // вроде не нужна    


            //CMBFilter.ItemsSource = ContextDB.ProductType.ToList();
            //CMBFilter.SelectedIndex = 0;
            //CMBFilter.DisplayMemberPath = "TypeName";

            if (TempFile.Check ==3 || TempFile.ChekNew.Contains(3))
            {
                products = products.Where(i => i.IdProdType == (CMBFilter.SelectedItem as ProductType).IdProdType).ToList();
            }


            var selectedIndexCmb = CMBSorting.SelectedIndex;

            switch (selectedIndexCmb)
            {
                case 0:
                    products = products.OrderBy(i => i.IdProd).ToList();
                    break;


                case 1:
                    products = products.OrderBy(i => i.ProdName.ToLower()).ToList();
                    break;


                case 2:
                    products = products.OrderByDescending(i => i.ProdName.ToLower()).ToList();
                    break;
                
                
                case 3:
                    products = products.OrderBy(i => i.ProductType.TypeName.ToLower()).ToList();
                    break;


                case 4:
                    products = products.OrderByDescending(i => i.ProductType.TypeName.ToLower()).ToList();
                    break;


                case 5:
                    //products = products.OrderBy(i =>Convert.ToInt32(i.Cost)).ToList();
                    products = products.OrderBy(i => i.Cost).ToList();
                    break;


                case 6:
                    products = products.OrderByDescending(i => i.Cost).ToList();
                    break;


                case 7:
                    //products = products.OrderBy(i =>Convert.ToInt32(i.Cost)).ToList();
                    products = products.OrderBy(i => i.Quantity).ToList();
                    break;


                case 8:
                    products = products.OrderByDescending(i => i.Quantity).ToList();
                    break;

                default:
                    break;
            }

            //В наличии
            if (TempFile.Check == 2 && InStock.Content != "Не в наличии"  || TempFile.ChekNew.Contains(2) && InStock.Content != "Не в наличии")
            {
                products = products.Where(i => i.Quantity > 0).ToList();
                InStock.Content = "Не в наличии";
            }
            else
            {
                InStock.Content = "В наличии";
                products = products.Where(i => i.Quantity >= 0).ToList();
            }

            // TempFile.Check = 0;
            //TempFile.ChekNew = new List<int>();


            //Итоговый спсиок
            LvProduct.ItemsSource = products;

        }

        private void AddProd_Click(object sender, RoutedEventArgs e)
        {
            TempFile.Check = 1;
            TempFile.ChekNew.Add(1);
            AddEditProduct addEditProduct = new AddEditProduct();
            addEditProduct.ShowDialog();
            Page_Loaded(sender, e);

        }

        private void EditProd_Click(object sender, RoutedEventArgs e)
        {

            TempFile.ProdSelect = LvProduct.SelectedItem as Product; // вроде не нужно

            if (LvProduct.SelectedItem == null)
            {
                return;
            }

            Page_Loaded(sender, e);
            AddEditProduct addEditProduct = new AddEditProduct();
            addEditProduct.ShowDialog();
            Page_Loaded(sender, e);

        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Page_Loaded(sender, e);
        }

        private void CMBFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //List<Product> products = new List<Product>();
            //products = ContextDB.Product.ToList();
            //products = products.Where(i => i.IdProdType == (CMBFilter.SelectedItem as ProductType).IdProdType).ToList();

            // TempFile.CMBTypeProd = (CMBFilter.SelectedItem as ProductType).IdProdType;
            //LvProduct.ItemsSource = products;
            if (CMBFilter.SelectedIndex != -1)
            {
               // TempFile.Check = 3;
                TempFile.ChekNew.Add(3);

            }
            Page_Loaded(sender, e);

        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TempFile.ChekNew.Clear();
            List<Product> products = new List<Product>();
            products = ContextDB.Product.ToList();
            LvProduct.ItemsSource = products;
            TbSearch.Clear();
            CMBFilter.SelectedIndex = -1;
        }

        private void CMBSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Page_Loaded(sender, e);
        }

        private void InStock_Click(object sender, RoutedEventArgs e)
        {
          //  TempFile.Check = 2;
            TempFile.ChekNew.Add(2);
            Page_Loaded(sender, e);
        }

        private void BtnAddToCartProduct_Click(object sender, RoutedEventArgs e)
        {

            //TempFile.ProdSelect = LvProduct.SelectedItem as Product; // вроде не нужно


            var button = sender as Button;
            if (button == null)
            {
                return;
            }

            var prodNum = button.DataContext as Product;


            var user = TempFile.user;

            var basket = ContextDB.Basket.ToList();
            var selectedProd = basket.FirstOrDefault(i => i.IdProd == prodNum.IdProd && i.IdClient == user.IdUser);


            if (selectedProd == null && prodNum.Quantity != 0)
            {
                Basket bas = new Basket()
                {

                    IdClient = user.IdUser,
                    IdProd = prodNum.IdProd,
                    Quantity = 1
                };

                //Для изменения количества продукта в таблице Product
                //Удаление количества продуктов из таблицы Product
                //prodNum.Quantity += -1;

                // MessageBox.Show("Товар успешно Добавлен!", "Товар", MessageBoxButton.OK, MessageBoxImage.Information);

                //BasketGrid.Items.Refresh();
                ContextDB.Basket.Add(bas);
                ContextDB.SaveChanges();
                Page_Loaded(sender, e);
                return;
            }


            if (prodNum == null || prodNum.Quantity == 0)
            {
                MessageBox.Show("Извините товар закончился", "Товар", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

          //  MessageBox.Show("Товар уже Добавлен! Загляните в корзину", "Товар", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SignInBasket_Click(object sender, RoutedEventArgs e)
        {
            BasketProd basket = new BasketProd();
            this.Close();
            basket.ShowDialog();
            Page_Loaded(sender, e);
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            Auth auth = new Auth();
            this.Close();
            auth.Show();
        }
    }
}
