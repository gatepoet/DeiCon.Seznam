namespace Seznam.Utilities
{
    public class SessionContext : ISessionContext
    {
        public string Username { get; set; }
        public static ISessionContext Current = new SessionContext();
    }
}