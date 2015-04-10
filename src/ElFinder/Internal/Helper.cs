using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ElFinder
{
    internal static class Helper
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

        public static string GetFileMd5(IFileInfo info)
        {
            Contract.Requires(info != null);
            Contract.Ensures(Contract.Result<string>() != null);

            return GetFileMd5(info.Name, info.LastWriteTimeUtc);
        }

        public static string GetFileMd5(string fileName, DateTime modified)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
            Contract.Ensures(Contract.Result<string>() != null);

            fileName += modified.ToFileTimeUtc();
            char[] fileNameChars = fileName.ToCharArray();
            byte[] buffer = new byte[m_stringEncoder.GetByteCount(fileNameChars, 0, fileName.Length, true)];
            m_stringEncoder.GetBytes(fileNameChars, 0, fileName.Length, buffer, 0, true);
            return BitConverter.ToString(m_md5CryptoProvider.ComputeHash(buffer)).Replace("-", string.Empty);
        }

        public static string GetDuplicatedName(IFileInfo file)
        {
            Contract.Requires(file != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var parentPath = file.DirectoryName;
            var name = Path.GetFileNameWithoutExtension(file.Name);
            var ext = file.Extension;

            var newName = string.Format(@"{0}\{1} copy{2}", parentPath, name, ext);            
            if (!File.Exists(newName))
            {
                return newName;               
            }
            else
            {
                bool finded = false;
                for (int i = 1; i < 10 && !finded; i++)
                {
                    newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, i, ext);
                    if (!File.Exists(newName))
                        finded = true;
                }
                if (!finded)
                    newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, Guid.NewGuid(), ext);
            }

            return newName;
        }

        private static Encoder m_stringEncoder = Encoding.UTF8.GetEncoder();
        private static MD5CryptoServiceProvider m_md5CryptoProvider = new MD5CryptoServiceProvider();
    }
}