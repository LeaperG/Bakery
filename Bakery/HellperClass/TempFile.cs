using Bakery.DB;
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
        public static DB.Product ProdSelect;
        public static DB.Product editProd;
        public static int Check;
        public static int CMBTypeProd;
        public static List<Product> productsBasket = new List<Product>();


        //public static DB.Client client;
    }
}
