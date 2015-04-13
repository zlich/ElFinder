using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElFinder
{
    /// <summary>
    /// Represents local filesystem root
    /// </summary>
    public class LocalFileSystemRoot : IRoot
    {
        public string VolumeId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Alias
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Url
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ThumbnailsManager ThumbnailsManager
        {
            get { return m_thumbnailManager; }
        }

        public AccessManager AccessManager
        {
            get { return m_accessManager; }
        }

        public IDirectoryInfo GetDirectory(string relativePath)
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFile(string relativePath)
        {
            throw new NotImplementedException();
        }

        public LocalFileSystemRoot(string directory) :
            this(new DirectoryInfo(directory)) { }

        public LocalFileSystemRoot(DirectoryInfo directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory", "Root directory can not be null");
            if (!directory.Exists)
                throw new ArgumentException("Root directory must exist", "directory");

            m_parentPath = directory.Parent != null ? directory.Parent.FullName : string.Empty;
            m_thumbnailManager = new ThumbnailsManager();
            m_accessManager = new AccessManager();
        }

        internal string ParentPath
        {
            get { return m_parentPath; }
        }

        private readonly string m_parentPath;
        private readonly ThumbnailsManager m_thumbnailManager;
        private readonly AccessManager m_accessManager;
    }
}
