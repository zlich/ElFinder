using System.Diagnostics.Contracts;
using System.Web;

namespace ElFinder
{
    internal static class PathHelper
    {
        public static string EncodePath(string path)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return HttpServerUtility.UrlTokenEncode(System.Text.UTF8Encoding.UTF8.GetBytes(path));
        }
        public static string DecodePath(string path)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return System.Text.UTF8Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(path));
        }
        public static string NormalizeRelativePath(string path)
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<string>() != null);

            int start = 0;
            int length = path.Length;
            if (path.Length == 0)
                return string.Empty;
            if (path[0] == '/' || path[0] == '\\')
            {
                start = 1;
                length--;
            }
            if (length > 0 && (path[length - 1] == '/' && path[length - 1] == '\\'))
                length--;
            return path.Substring(start, length);
        }

        public static string Combine(string relativeDir, string name)
        {
            Contract.Requires(relativeDir != null);
            Contract.Requires(name != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return relativeDir + "/" + name;
        }
    }
}