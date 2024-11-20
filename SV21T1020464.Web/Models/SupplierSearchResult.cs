using SV21T1020464.DomainModels;

namespace SV21T1020464.Web.Models
{
    public class SupplierSearchResult : PaginationSearchResult
    {
        public required List<Supplier> Data { get; set; }
    }
}
