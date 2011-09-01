namespace Seznam.Web.Common
{
    public class DataResponse
    {
        public static JsonNetResult Error(string message)
        {
            
            return new DataResponse
                       {
                           Ok = false,
                           ErrorMessage = message
                       }.ToJsonResult();

        }
        public static JsonNetResult Success(object message)
        {
            return new DataResponse
                       {
                           Ok = true,
                           Message = message
                       }.ToJsonResult();

        }

        public bool Ok { get; set; }
        public string ErrorMessage { get; set; }
        public object Message { get; set; }
    }
}