namespace Seznam.Data.Services.User.Contracts
{
    public interface IUserService
    {
        string CreateUser(string username, string password);
        string Authenticate(string username, string password);
    }
}