using SV21T1020464.DomainModels;

namespace SV21T1020464.Web.Models
{
    public class CustomerSearchResult: PaginationSearchResult
    {
        public required List<Customer> Data { get; set; }
    }
}