using SV21T1020464.DomainModels;

namespace SV21T1020464.Web.Models
{
    public class EmployeeSearchResult:PaginationSearchResult
    {
        public required List<Employee> Data { get; set; }
    }
}
