namespace Seznam.Controllers
{
    public class DataResponse
    {
        public static JsonNetResult Error(string message)
        {
            
            return new DataResponse
                       {
                           Ok = false,
                           Message = message
                       }.ToJsonResult();

        }
        public static JsonNetResult Success(object data)
        {
            return new DataResponse
                       {
                           Ok = true,
                           Data = data
                       }.ToJsonResult();

        }

        public bool Ok { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}