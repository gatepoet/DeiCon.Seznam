using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Seznam.Controllers
{
    public static class JsonNetExtensions
    {
        public static JsonNetResult ToJsonResult(this object data)
        {
            return new JsonNetResult
                       {
                           Formatting = Formatting.Indented,
                           Data = data,
                           SerializerSettings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()}
                       };
        }
    }
}