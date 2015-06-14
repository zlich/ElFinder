using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace ElFinder
{
    internal abstract class LocalUnitInfo : IUnitInfo
    {
        public IRoot Root
        {
            get { return m_root; }
        }

        public string Name
        {
            get { return m_info.Name; }
        }

        public bool Exists 
        {
            get { return m_info.Exists; }
        }

        public string RelativePath
        {
            get
            {
                LocalDirectoryInfo dir = (LocalDirectoryInfo)m_root.Directory;
                string dirPath = dir.Info.FullName;
                int length = dirPath.Length;
                if (dirPath[length - 1] == '\\')
                    length--;
                string path = m_info.FullName.Substring(length);
                if (m_info is DirectoryInfo && path.Length > 0 && path[path.Length - 1] == '\\')
                    path = path.Substring(0, path.Length - 1);
                return path;
            }
        }

        public bool IsHidden
        {
            get { return (m_info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden; }
        }

        public abstract string MimeType { get; }

        public abstract UnitDTO ToDTO();

        public abstract void Delete();

        protected LocalUnitInfo(LocalFileSystemRoot root, FileSystemInfo info)
        {
            Contract.Requires(root != null);
            Contract.Requires(info != null);
            m_root = root;
            m_info = info;
        }

        protected FileSystemInfo Info
        {
            get { return m_info; }
        }

        protected LocalFileSystemRoot FileSystemRoot
        {
            get { return m_root; }
        }

        protected static readonly DateTime UnixOrigin = new DateTime(1970, 1, 1, 0, 0, 0);

        private readonly LocalFileSystemRoot m_root;
        private readonly FileSystemInfo m_info;
    }
}
