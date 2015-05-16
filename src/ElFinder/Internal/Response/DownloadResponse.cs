using System.Diagnostics.Contracts;
using System.Web;

namespace ElFinder
{
    internal class DownloadResponse : ResponseBase
    {
        public DownloadResponse(IFileInfo fileInfo, bool isDownload)
        {
            Contract.Requires(fileInfo != null);            
            m_fileInfo = fileInfo;
            m_isDownload = isDownload;
        }
        public override void WriteResponse(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            if (!m_fileInfo.IsFileFromCache(request, response))
            {
                string fileName;
                string fileNameEncoded = HttpUtility.UrlEncode(m_fileInfo.Name);

                if (request.UserAgent.Contains("MSIE")) // IE < 9 do not support RFC 6266 (RFC 2231/RFC 5987)
                    fileName = "filename=\"" + fileNameEncoded + "\"";
                else
                    fileName = "filename*=UTF-8\'\'" + fileNameEncoded; // RFC 6266 (RFC 2231/RFC 5987)

                string mime;
                string disposition;
                if (m_isDownload)
                {
                    mime = "application/octet-stream";
                    disposition = "attachment; " + fileName;
                }
                else
                {
                    mime = m_fileInfo.MimeType;
                    disposition = (mime.Contains("image") || mime.Contains("text") || mime == "application/x-shockwave-flash" ? "inline; " : "attachment; ") + fileName;
                }

                response.ContentType = mime;
                response.AppendHeader("Content-Disposition", disposition);
                response.AppendHeader("Content-Location", m_fileInfo.Name);
                response.AppendHeader("Content-Transfer-Encoding", "binary");
                response.AppendHeader("Content-Length", m_fileInfo.Length.ToString());
                m_fileInfo.CopyTo(response.OutputStream);
                response.Flush();
            }
            else
            {
                response.ContentType = m_isDownload ? "application/octet-stream" : m_fileInfo.MimeType;
                response.End();
            }
        }

        private readonly IFileInfo m_fileInfo;
        private readonly bool m_isDownload;
    }
}
