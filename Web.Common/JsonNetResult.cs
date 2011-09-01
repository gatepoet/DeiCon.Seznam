using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Seznam.Web.Common
{
    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = string.IsNullOrEmpty(ContentType)
                                       ? "application/json"
                                       : ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null)
                return;

            var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
            var serializer = JsonSerializer.Create(SerializerSettings);

            serializer.Serialize(writer, Data);
            writer.Flush();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Data, Formatting, SerializerSettings);
        }
    }
}