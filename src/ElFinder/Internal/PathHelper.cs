﻿using System.Diagnostics.Contracts;
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

        //public static string GetDuplicatedName(IFileInfo file)
        //{
        //    Contract.Requires(file != null);
        //    Contract.Ensures(Contract.Result<string>() != null);

        //    //var parentPath = file.DirectoryName;
        //    var name = Path.GetFileNameWithoutExtension(file.Name);
        //    var ext = file.Extension;

        //    var newName = string.Format(@"{0}\{1} copy{2}", parentPath, name, ext);            
        //    if (!File.Exists(newName))
        //    {
        //        return newName;               
        //    }
        //    else
        //    {
        //        bool finded = false;
        //        for (int i = 1; i < 10 && !finded; i++)
        //        {
        //            newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, i, ext);
        //            if (!File.Exists(newName))
        //                finded = true;
        //        }
        //        if (!finded)
        //            newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, Guid.NewGuid(), ext);
        //    }

        //    return newName;
        //}


    }
}