﻿using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace ElFinder
{
    /// <summary>
    /// Represents local filesystem root
    /// </summary>
    public class LocalFileSystemRoot : IRoot
    {
        /// <summary>
        /// Gets a autogenerated prefix of root
        /// </summary>
        public string VolumeId
        {
            get { return m_volumeId; }
            set { m_volumeId = value; }
        }

        /// <summary>
        /// Get or sets alias for root. If not set will use directory name of path
        /// </summary>
        public string Alias
        {
            get { return m_alias; }
            set { m_alias = value; }
        }

        /// <summary>
        /// Get or sets url that points to path directory (also called 'root URL'). 
        /// </summary>
        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        /// <summary>
        /// Get or sets a directory which is root 
        /// </summary>
        public IDirectoryInfo Directory
        {
            get { return new LocalDirectoryInfo(this, m_directory); }
        }

        /// <summary>
        /// Get or sets a subfolder of root diretory, which will be start 
        /// </summary>
        public IDirectoryInfo StartPath
        {
            get { return m_startDirectory; }
            set
            {
                if (value == null)
                    m_startDirectory = null;
                else
                {
                    if (value.Root != this)
                        throw new ArgumentException("Start directory must be from the same root");
                    LocalDirectoryInfo dir = (LocalDirectoryInfo)value;
                    if (!dir.Exists)
                        throw new ArgumentException("Start directory must exist");
                    m_startDirectory = dir;
                }
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

        #region File operations
        public IFileInfo GetFile(string relativePath)
        {
            return new LocalFileInfo(this, Path.Combine(m_directoryPath, PathHelper.NormalizeRelativePath(relativePath)));
        }

        public IFileInfo CreateFile(string relativeDir, string name)
        {
            string path = Path.Combine(m_directoryPath, PathHelper.NormalizeRelativePath(relativeDir), name);
            if (!System.IO.File.Exists(path))
                using (FileStream stream = File.Create(path)) { }
            return new LocalFileInfo(this, path);
        }

        public IFileInfo RenameFile(string relativePath, string newName)
        {
            return new LocalFileInfo(this, Rename(relativePath, newName, false));
        }

        public void DeleteFile(string relativePath)
        {
            Delete(relativePath, false);
        } 
        #endregion

        #region Directory operations
        public IDirectoryInfo GetDirectory(string relativePath)
        {
            return new LocalDirectoryInfo(this, Path.Combine(m_directoryPath, PathHelper.NormalizeRelativePath(relativePath)));
        }

        public IDirectoryInfo CreateDirectory(string relativeDir, string name)
        {
            string path = Path.Combine(m_directoryPath, PathHelper.NormalizeRelativePath(relativeDir), name);
            if (!System.IO.Directory.Exists(path))
                return new LocalDirectoryInfo(this, System.IO.Directory.CreateDirectory(path));
            else
                return new LocalDirectoryInfo(this, path);
        }

        public IDirectoryInfo RenameDirectory(string relativePath, string newName)
        {
            return new LocalDirectoryInfo(this, Rename(relativePath, newName, true));
        }
        public void DeleteDirectory(string relativePath)
        {
            Delete(relativePath, true);
        } 
        #endregion


        public LocalFileSystemRoot(DirectoryInfo directory, string url)
        {
            Contract.Requires(directory != null);
            Contract.Requires(directory.FullName.Length > 0);
            if (!directory.Exists)
                throw new ArgumentException("Root directory must exist", "directory");

            //m_parentPath = directory.Parent != null ? directory.Parent.FullName : string.Empty;
            m_directory = directory;
            m_directoryPath = directory.FullName;
            int length = m_directoryPath.Length;
            if (m_directoryPath[length - 1] == '\\')
                m_directoryPath = m_directoryPath.Substring(0, length);

            m_thumbnailManager = new ThumbnailsManager();
            m_accessManager = new AccessManager();
            m_url = url;
        }

        public LocalFileSystemRoot(string directory, string url)
            : this(new DirectoryInfo(directory), url)
        {
            Contract.Requires(directory != null);
        }

        public LocalFileSystemRoot(DirectoryInfo directory) :
            this(directory, null)
        {
            Contract.Requires(directory != null);
        }

        public LocalFileSystemRoot(string directory) :
            this(directory, null)
        {
            Contract.Requires(directory != null);
        }

        private string Rename(string relativePath, string newName, bool isDir)
        {
            string src = Path.Combine(m_directoryPath, PathHelper.NormalizeRelativePath(relativePath));
            string dest = Path.Combine(System.IO.Directory.GetParent(src).FullName, newName);
            if (isDir)
                System.IO.Directory.Move(src, dest);
            else
                File.Move(src, dest);
            return dest;
        }

        private void Delete(string relativePath, bool isDir)
        {
            string path = Path.Combine(m_directoryPath, PathHelper.NormalizeRelativePath(relativePath));
            if (isDir)
                System.IO.Directory.Delete(path, true);
            else
                File.Delete(path);
        }
     
        private readonly DirectoryInfo m_directory;
        private readonly string m_directoryPath;

        private readonly ThumbnailsManager m_thumbnailManager;
        private readonly AccessManager m_accessManager;

        private string m_url;
        private string m_alias;
        private string m_volumeId;
        private LocalDirectoryInfo m_startDirectory;
    }
}
