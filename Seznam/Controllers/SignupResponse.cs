namespace Seznam.Controllers
{
    public class SignupResponse
    {
        public static SignupResponse Error(string message)
        {
            return new SignupResponse
                       {
                           Ok = false,
                           Message = message
                       };

        }
        public static SignupResponse Success(string userId)
        {
            return new SignupResponse
                       {
                           Ok = true,
                           UserId = userId
                       };

        }

        public bool Ok { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}