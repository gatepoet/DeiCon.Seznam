namespace Seznam.Controllers
{
    public class DataResponse<TData>
    {
        public static DataResponse<TData> Error(string message)
        {
            return new DataResponse<TData>
                       {
                           Ok = false,
                           Message = message
                       };

        }
        public static DataResponse<TData> Success(TData data)
        {
            return new DataResponse<TData>
                       {
                           Ok = true,
                           Data = data
                       };

        }

        public bool Ok { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }
    }
}