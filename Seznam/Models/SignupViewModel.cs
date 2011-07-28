using Newtonsoft.Json;

namespace Seznam.Models
{
    [JsonObject]
    public class SignupViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}