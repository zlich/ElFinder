using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ElFinder;

namespace ElFinderTests
{
    internal static class TestHelper
    {
        public static string GetTestDataPath(string localPath)
        {
            return Path.Combine(TestDataPath, localPath);
        }

        public static string GetFileHash(string filename)
        {
            return GetDataHash( File.ReadAllBytes(filename));
        }

        public static string GetDataHash(byte[] data)
        {
            var hash = new SHA1Managed();
            var hashedBytes = hash.ComputeHash(data);
            return ConvertBytesToHex(hashedBytes);
        }

        private static string ConvertBytesToHex(byte[] bytes)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x"));
            }
            return builder.ToString();
        }

        public static readonly string TestDataPath = "..\\..\\..\\testData\\";
    }
}
