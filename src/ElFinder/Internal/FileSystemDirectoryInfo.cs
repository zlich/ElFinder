using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace ElFinder
{
    internal class FileSystemDirectoryInfo : UnitInfo, IDirectoryInfo
    {
        public override string Name
        {
            get { return m_info.Name; }
        }
        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            return m_info.GetDirectories().Select(i => new FileSystemDirectoryInfo(Root, i));
        }

        public override UnitDTO ToDTO()
        {
            if (root.Directory.FullName == directory.FullName)
            {
                bool hasSubdirs = false;
                foreach (IDirectoryInfo item in directory.GetDirectories())
                {
                    if ((item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        hasSubdirs = true;
                        break;
                    }
                }
                RootDTO response = new RootDTO()
                {
                    Mime = "directory",
                    Dirs = hasSubdirs ? (byte)1 : (byte)0,
                    Hash = root.VolumeId + Helper.EncodePath(directory.Name),
                    Read = 1,
                    Write = root.IsReadOnly ? (byte)0 : (byte)1,
                    Locked = root.IsLocked ? (byte)1 : (byte)0,
                    Name = root.Alias,
                    Size = 0,
                    UnixTimeStamp = (long)(directory.LastWriteTimeUtc - UnixOrigin).TotalSeconds,
                    VolumeId = root.VolumeId
                };
                return response;
            }
            else
            {
                string parentPath = directory.Parent.FullName.Substring(root.Directory.FullName.Length);
                DirectoryDTO response = new DirectoryDTO()
                {
                    Mime = "directory",
                    ContainsChildDirs = m_info.EnumerateDirectories().Any() ? (byte)1 : (byte)0,
                    Hash = root.VolumeId + Helper.EncodePath(directory.FullName.Substring(root.Directory.FullName.Length)),
                    Read = 1,
                    Write = root.IsReadOnly ? (byte)0 : (byte)1,
                    Locked = root.IsLocked ? (byte)1 : (byte)0,
                    Size = 0,
                    Name = m_info.Name,
                    UnixTimeStamp = (long)(m_info.LastWriteTimeUtc - UnixOrigin).TotalSeconds,
                    ParentHash = root.VolumeId + Helper.EncodePath(parentPath.Length > 0 ? parentPath : directory.Parent.Name)
                };
                return response;
            }
        }

        public FileSystemDirectoryInfo(IRoot root, string path)
            : base(root)
        {
            Contract.Requires(path != null);

            m_info = new DirectoryInfo(path);
        }

        private FileSystemDirectoryInfo(IRoot root, DirectoryInfo info)
            : base(root)
        {
            Contract.Requires(info != null);

            m_info = info;
        }

        private readonly DirectoryInfo m_info;
    }
}
