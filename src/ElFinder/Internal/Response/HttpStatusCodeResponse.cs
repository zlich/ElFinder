using System.Diagnostics.Contracts;
using System.Net;
using System.Web;

namespace ElFinder
{
    internal class HttpStatusCodeResponse : ResponseBase
    {
        public HttpStatusCodeResponse(HttpStatusCode code, string description)
        {
            Contract.Requires(description != null);
            m_code = code;
            m_message = description;
        }

        public override void WriteResponse(HttpContext context)
        {
            context.Response.StatusCode = (int)m_code;
            context.Response.StatusDescription = m_message;
        }

        private readonly HttpStatusCode m_code;
        private readonly string m_message;
    }
}
