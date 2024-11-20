using SV21T1020464.DomainModels;

namespace SV21T1020464.Web.Models
{
  
        public class ShipperSearchResult : PaginationSearchResult
        {
            public required List<Shipper> Data { get; set; }
        }
    }

