namespace Seznam.Controllers
{
    public class SimpleResponse : JsonNetResult
    {
        public static SimpleResponse Error(string message)
        {
            return new SimpleResponse
                       {
                           Ok = false,
                           Message = message
                       };

        }
        public static SimpleResponse Success()
        {
            return new SimpleResponse
                       {
                           Ok = true,
                       };

        }

        public bool Ok { get; set; }
        public string Message { get; set; }
    }
}