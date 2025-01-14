using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020464.DomainModels
{
    public class ViewCart
    {

        public int CartDetailID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public string ProductName { get; set; } = "";
        public string Photo { get; set; } = "";
   



    

    }
}
