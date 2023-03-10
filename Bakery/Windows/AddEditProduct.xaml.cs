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
using Microsoft.Win32;

namespace Bakery.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Window
    {

        private string pathPhoto = null;

        private bool isEdit = false;

        private Product editProduct;


        public AddEditProduct()
        {
            InitializeComponent();
            CMBTypeProduct.ItemsSource = ContextDB.ProductType.ToList();
            CMBTypeProduct.SelectedIndex = 0;
            CMBTypeProduct.DisplayMemberPath = "TypeName";

            if (TempFile.Check != 1)
            {
                AddProd.Content = "Изменить товар";
                GetListProduct();
            }
            TempFile.Check = 0;
        }




        private void GetListProduct()
        {
            InitializeComponent();

            var product = TempFile.ProdSelect;

            CMBTypeProduct.ItemsSource = ContextDB.ProductType.ToList();
            CMBTypeProduct.SelectedIndex = 0;
            CMBTypeProduct.DisplayMemberPath = "TypeName";

            TbProdName.Text = product.ProdName.ToString();
            TbProdDescription.Text = product.Description.ToString();
            TbCost.Text = product.Cost.ToString();
            TbQuantity.Text = product.Quantity.ToString();
            CMBTypeProduct.SelectedItem = ContextDB.ProductType.Where(i => i.IdProdType == product.IdProdType).FirstOrDefault();

            if (product.Image != null)
            {
                using (MemoryStream stream = new MemoryStream(product.Image))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    ImgProduct.Source = bitmapImage;

                }
            }


            isEdit = true;

            editProduct = product;
           
        }

        private void AddProd_Click(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                //изменение товара

                editProduct.ProdName = TbProdName.Text;
                editProduct.Description = TbProdDescription.Text;
                editProduct.IdProdType = (CMBTypeProduct.SelectedItem as ProductType).IdProdType;
                editProduct.Cost = Convert.ToDecimal(TbCost.Text);
                editProduct.Quantity = Convert.ToInt32(TbQuantity.Text); 

                if (pathPhoto != null)
                {
                    editProduct.Image = File.ReadAllBytes(pathPhoto);
                }
                ContextDB.SaveChanges();
               // MessageBox.Show("Товар успешно изменён!", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //добавление товара
                Product product = new Product();
                product.ProdName = TbProdName.Text;
                product.Description = TbProdDescription.Text;
                product.IdProdType = (CMBTypeProduct.SelectedItem as ProductType).IdProdType;
                product.Cost = Convert.ToDecimal(TbCost.Text);
                product.Quantity= Convert.ToInt32(TbQuantity.Text);


                if (pathPhoto != null)
                {
                    product.Image = File.ReadAllBytes(pathPhoto);
                }

                ContextDB.Product.Add(product);
                ContextDB.SaveChanges();
                //MessageBox.Show("Товар успешно добавлен!", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
           // TempFile.Check = 0;
            this.Close();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                ImgProduct.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                pathPhoto = openFileDialog.FileName;
            }
        }
    }
}
