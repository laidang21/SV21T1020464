using SV21T1020464.DomainModels;

namespace SV21T1020464.Web.Models
{
    public class CategorySearchResult : PaginationSearchResult
    {
        public required List<Category> Data { get; set; }
    }
}