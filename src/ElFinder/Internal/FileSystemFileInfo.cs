using System;
using System.IO;

namespace ElFinder
{
    internal class FileSystemFileInfo : IFileInfo
    {
        public string Name
        {
            get { return m_info.Name; }
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

        public DateTime LastWriteTimeUtc
        {
            get { return m_info.LastWriteTimeUtc; }
        }

        public FileSystemFileInfo(string fileName)
        {
            m_info = new FileInfo(fileName);
        }

        private FileInfo m_info;
    }
}
