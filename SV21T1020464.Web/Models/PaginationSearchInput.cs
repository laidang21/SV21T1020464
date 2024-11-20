namespace SV21T1020464.Web.Models
{
 /// <summary>
/// Lưu giữ các thông tin  đầu vào sử dụng co các chức năng tìm kiếm  và hiển thị dữ liệu dưới dạng phân trang
/// </summary>
    public class PaginationSearchInput
    {
        /// <summary>
        /// Trang cần hiển thị
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Số  dòng hiển thị trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// chuỗi giá trị cần tìm
        /// </summary>
        public string SearchValue { get; set; } = "";
    }
}
