using Bakery.DB;
using Bakery.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.HellperClass
{
    internal class TempFile
    {
        public static DB.UserAccount user;
        public static DB.Basket bask;
        public static DB.Product ProdSelect;
        public static DB.UserAccount UserSelect;
        public static DB.Client client; 
        //  public static DB.Product ProdCart;
        public static DB.Product editProd;
        //public static DB.Basket basketG;
       // public static List<Basket> basket = new List<Basket>();
        public static List<int> ChekNew = new List<int>();
        public static List<Product> productLists = new List<Product>();
        public static int Check;
        public static decimal Summ;
        public static int CheckInStock;
        public static int IdOrder;
        public static int CMBTypeProd;
        //public static List<Product> productsBasket = new List<Product>();


        //public static DB.Client client;

    }
}
