using SV21T1020464.BusinessLayers;
using SV21T1020464.DataLayers;
using SV21T1020464.DomainModels;



namespace SV21T1020464.BusinessLayers
{
    public static class CartDataService
    {
        private static readonly ICartDAL cartDB;

        static CartDataService()
        {
            cartDB = new CartDAL(Configuration.ConnectionString);
        }
        public static int AddCart(Cart Cart)
        {
            return cartDB.Add(Cart);
        }
        public static int AddCartDetail(Cartdetail data)
        {
            return cartDB.AddCartDetail(data);
        }
        public static int Count(int customerID)
        {
            return cartDB.Count(customerID);
        }
        public static Cart getCartByCustomerID(int cutomerID)
        {
            return cartDB.GetByID(cutomerID);
        }
        public static Cartdetail checkProductExists(int CartID, int productID)
        {
            return cartDB.checkProductExists(CartID, productID);
        }
        public static bool SaveCartdetail(int CartID, int ProductID, int Quantity)
        {
            return cartDB.SaveDetail(CartID, ProductID, Quantity);
        }
        public static bool SaveCart(Cart Cart)
        {
            return cartDB.Update(Cart);
        }
        public static List<ViewCart>listViewCart(int customerID)
        {
            return cartDB.GetDetailList(customerID);
        }
        public static bool DeleteDetail( int cartDetailID)
        {
            return cartDB.DeleteDetail( cartDetailID);
        }

           
        
    }
}
