using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElFinder
{
    internal class FileSystemDirectoryInfo : IDirectoryInfo
    {
        public string MimeType
        {
            get { return "directory"; }
        }

        public string Name
        {
            get { return m_info.Name; }
        }

        public System.DateTime LastWriteTimeUtc
        {
            get { return m_info.LastWriteTimeUtc; }
        }

        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            return m_info.GetDirectories().Select(i => new FileSystemDirectoryInfo(i));
        }

        public FileSystemDirectoryInfo(string path)
        {
            m_info = new DirectoryInfo(path);
        }

        private FileSystemDirectoryInfo(DirectoryInfo info)
        {
            m_info = info;
        }

        private DirectoryInfo m_info;
    }
}
