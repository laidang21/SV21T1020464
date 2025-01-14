namespace SV21T1020464.DomainModels
{
    public class Cartdetail
    {
        public int CartdetailId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
    }
}
