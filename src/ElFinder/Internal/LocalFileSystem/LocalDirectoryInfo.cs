using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElFinder
{
    internal class LocalDirectoryInfo : LocalUnitInfo, IDirectoryInfo
    {
        public override string MimeType
        {
            get { return "directory"; }
        }

        public IDirectoryInfo Parent
        {
            get { return new LocalDirectoryInfo(FileSystemRoot, DirectoryInfo.Parent); }
        }

        public IEnumerable<IDirectoryInfo> GetDirectories()
        {
            return DirectoryInfo.EnumerateDirectories().Select(i => new LocalDirectoryInfo(FileSystemRoot, i));
        }

        public IEnumerable<IFileInfo> GetFiles()
        {
            return DirectoryInfo.EnumerateFiles().Select(i => new LocalFileInfo(FileSystemRoot, i));
        }

        public IEnumerable<IUnitInfo> GetUnits()
        {
            return DirectoryInfo.EnumerateFileSystemInfos().Select(i =>
                {
                    IUnitInfo result;
                    if (i is FileInfo)
                        result = new LocalFileInfo(FileSystemRoot, (FileInfo)i);
                    else
                        result = new LocalDirectoryInfo(FileSystemRoot, (DirectoryInfo)i);
                    return result;
                });
        }

        public override UnitDTO ToDTO()
        {
            if (RelativePath == Name)
            {
                bool hasSubdirs = false;
                foreach (DirectoryInfo item in DirectoryInfo.EnumerateDirectories())
                {
                    if ((item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        hasSubdirs = true;
                        break;
                    }
                }
                RootDTO response = new RootDTO()
                {
                    Mime = MimeType,
                    Dirs = hasSubdirs ? (byte)1 : (byte)0,
                    Hash = Root.VolumeId + Helper.EncodePath(Name),
                    Read = 1,
                    Write = (byte)(Root.AccessManager.IsReadOnly ? 0 : 1),
                    Locked = (byte)(Root.AccessManager.IsLocked ? 1 : 0),
                    Name = Root.Alias,
                    Size = 0,
                    UnixTimeStamp = (long)(DirectoryInfo.LastWriteTimeUtc - UnixOrigin).TotalSeconds,
                    VolumeId = Root.VolumeId
                };
                return response;
            }
            else
            {
                DirectoryDTO response = new DirectoryDTO()
                {
                    Mime = MimeType,
                    ContainsChildDirs = DirectoryInfo.EnumerateDirectories().Any() ? (byte)1 : (byte)0,
                    Hash = Root.VolumeId + Helper.EncodePath(RelativePath),
                    Read = 1,
                    Write = (byte)(Root.AccessManager.IsReadOnly ? 0 : 1),
                    Locked = (byte)(Root.AccessManager.IsLocked ? 1 : 0),
                    Size = 0,
                    Name = Name,
                    UnixTimeStamp = (long)(DirectoryInfo.LastWriteTimeUtc - UnixOrigin).TotalSeconds,
                    ParentHash = Root.VolumeId + Helper.EncodePath(Parent.RelativePath)
                };
                return response;
            }
        }


        public LocalDirectoryInfo(LocalFileSystemRoot root, string fullPath)
            : this(root, new DirectoryInfo(fullPath)) { }

        public LocalDirectoryInfo(LocalFileSystemRoot root, DirectoryInfo info)
            : base(root, info) { }

        private DirectoryInfo DirectoryInfo
        {
            get { return (DirectoryInfo)Info; }
        }
    }
}
