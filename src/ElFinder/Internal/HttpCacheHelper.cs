using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ElFinder
{
    internal static class HttpCacheHelper
    {
        public static bool IsFileFromCache(string filename, DateTime lastWriteTime, HttpRequest request, HttpResponse response)
        {
            Contract.Requires(!string.IsNullOrEmpty(filename));
            Contract.Requires(request != null);
            Contract.Requires(response != null);

            DateTime modifyDate;
            if (!DateTime.TryParse(request.Headers["If-Modified-Since"], out modifyDate))
            {
                modifyDate = DateTime.UtcNow;
            }
            string eTag = GetFileETag(filename, lastWriteTime);
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetLastModified(lastWriteTime);
            response.Cache.SetETag(eTag);

            if (!IsFileModified(lastWriteTime, eTag, request))
            {
                response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                response.StatusDescription = "Not Modified";
                response.AddHeader("Content-Length", "0");
                return true;
            }
            else
            {
                response.Cache.SetAllowResponseInBrowserHistory(true);
                return false;
            }
        }
        private static bool IsFileModified(DateTime modifyDate, string eTag, HttpRequest request)
        {
            Contract.Requires(eTag != null);
            Contract.Requires(request != null);

            DateTime modifiedSince;
            bool fileDateModified = true;

            //Check If-Modified-Since request header, if it exists 
            if (!string.IsNullOrEmpty(request.Headers["If-Modified-Since"]) && DateTime.TryParse(request.Headers["If-Modified-Since"], out modifiedSince))
            {
                fileDateModified = false;
                if (modifyDate > modifiedSince)
                {
                    TimeSpan modifyDiff = modifyDate - modifiedSince;
                    //ignore time difference of up to one seconds to compensate for date encoding
                    fileDateModified = modifyDiff > TimeSpan.FromSeconds(1);
                }
            }

            //check the If-None-Match header, if it exists, this header is used by FireFox to validate entities based on the etag response header 
            bool eTagChanged = false;
            if (!string.IsNullOrEmpty(request.Headers["If-None-Match"]))
            {
                eTagChanged = request.Headers["If-None-Match"] != eTag;
            }
            return (eTagChanged || fileDateModified);
        }

        private static string GetFileETag(string fileName, DateTime lastModified)
        {
            Contract.Requires(fileName != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return "\"" + GetFileMd5(fileName, lastModified) + "\"";
        }

        private static string GetFileMd5(string fileName, DateTime modified)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
            Contract.Ensures(Contract.Result<string>() != null);

            fileName += modified.ToFileTimeUtc();
            char[] fileNameChars = fileName.ToCharArray();
            byte[] buffer = new byte[m_stringEncoder.GetByteCount(fileNameChars, 0, fileName.Length, true)];
            m_stringEncoder.GetBytes(fileNameChars, 0, fileName.Length, buffer, 0, true);
            return BitConverter.ToString(m_md5CryptoProvider.ComputeHash(buffer)).Replace("-", string.Empty);
        }

        private static Encoder m_stringEncoder = Encoding.UTF8.GetEncoder();
        private static MD5CryptoServiceProvider m_md5CryptoProvider = new MD5CryptoServiceProvider();
    }
}
