using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace ElFinder
{
    internal abstract class JsonResponse : ResponseBase
    {
        public override void WriteResponse(HttpContext context)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            using (JsonWriter jsonWriter = new JsonTextWriter(response.Output))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, this);
            }
        }
    }
}
