using SV21T1020464.DomainModels;

namespace SV21T1020464.DataLayers
{
    public interface ICartDAL
    {
        public int Add(Cart data);
        public int AddCartDetail(Cartdetail data);
        public int Delete(int customerID);
        public bool Update(Cart data);
        public int Count(int customerID);
        public Cart GetByID(int cutomerID);
        public List<ViewCart> GetDetailList(int customerID);
        public Cartdetail GetDetail(int cartID, int productID);
        bool SaveDetail(int cartDeatilID, int productID, int quantity);
        bool DeleteDetail( int cartDetailID);
        bool SaveCart(int customerID, int sum);
        public Cartdetail? checkProductExists(int cartID, int productID);
        public List<ViewCart> GetViewCarts(int userID);
    }
}
