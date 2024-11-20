namespace SV21T1020464.DataLayers
{
    /// <summary>
    /// định nghĩa chức năng truy vấn dữ liệu
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISimpleQueryDAL<T> where T: class

    {
        /// <summary>
        /// Truy vấn  và lấy toàn bộ dữ liệu
        /// </summary>
        /// <returns></returns>
        List<T> List();
    }
}
