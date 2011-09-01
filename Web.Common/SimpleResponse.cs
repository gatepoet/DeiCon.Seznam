namespace Seznam.Web.Common
{
    public class SimpleResponse
    {
        public static JsonNetResult Error(string message)
        {
            return new SimpleResponse
                       {
                           Ok = false,
                       }.ToJsonResult();

        }
        public static JsonNetResult Success()
        {
            return new SimpleResponse
                       {
                           Ok = true,
                       }.ToJsonResult();

        }

        public bool Ok { get; set; }
        public string Message { get; set; }
    }
}