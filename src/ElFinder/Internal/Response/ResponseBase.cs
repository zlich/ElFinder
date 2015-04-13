using System.IO;
using System.Web;

namespace ElFinder
{
    internal abstract class ResponseBase
    {
        public abstract void WriteResponse(HttpResponse response);
    }
}
