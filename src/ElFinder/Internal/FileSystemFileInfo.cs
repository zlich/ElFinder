using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace ElFinder
{
    internal class FileSystemFileInfo : IFileInfo
    {
        public string Name
        {
            get { return m_info.Name; }
        }

        public string Extension
        {
            get { return m_info.Extension; }
        }

        public string MimeType
        {
            get
            {
                string extension = m_info.Extension;
                if (extension.Length > 1)
                    return MimeTypes.GetMimeType(extension.ToLower().Substring(1));
                else
                    return "unknown";
            }
        }

        public long Length
        {
            get { return m_info.Length; }
        }

        public IDirectoryInfo Directory
        {
            get { return new FileSystemDirectoryInfo(m_info.DirectoryName); }
        }

        public string DirectoryName
        {
            get { return m_info.DirectoryName; }
        }

        public DateTime LastWriteTimeUtc
        {
            get { return m_info.LastWriteTimeUtc; }
        }

        public FileSystemFileInfo(string fileName)
        {
            Contract.Requires(fileName != null);

            m_info = new FileInfo(fileName);
        }

        private readonly FileInfo m_info;
    }
}
