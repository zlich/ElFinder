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

        public override IDirectoryInfo Parent
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

        public override void Delete()
        {
            Directory.Delete(DirectoryInfo.FullName, true);
        }

        public void CopyTo(IDirectoryInfo output)
        {
            foreach (IFileInfo item in GetFiles())
            {
                IFileInfo newFile = output.Root.GetFile(output.RelativePath + "/" + item.Name);
                item.CopyTo(newFile);
            }
            foreach (IDirectoryInfo item in GetDirectories())
            {
                IDirectoryInfo newDir = output.Root.CreateDirectory(output.RelativePath, item.Name);
                item.CopyTo(newDir);
            }
        }

        public void CutTo(IDirectoryInfo output)
        {
            LocalDirectoryInfo localDir = output as LocalDirectoryInfo;
            if (localDir != null)
            {
                if (Directory.Exists(localDir.DirectoryInfo.FullName))
                    localDir.Delete();
                DirectoryInfo.MoveTo(localDir.DirectoryInfo.FullName);
            }
            else
            {
                CopyTo(output);
                Delete();
            }
        }

        public override UnitDTO ToDTO()
        {
            UnitDTO response;
            bool hasSubdirs = DirectoryInfo.EnumerateDirectories().Any(i => (i.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden);
            if (RelativePath == string.Empty)
            {
                response = new RootDTO()
                {
                    ContainsChildDirs = hasSubdirs ? (byte)1 : (byte)0,
                    Name = Root.Alias ?? Name,
                    VolumeId = Root.VolumeId
                };
            }
            else
            {
                response = new DirectoryDTO()
                {
                    ContainsChildDirs = hasSubdirs ? (byte)1 : (byte)0,
                    Name = Name,
                    ParentHash = Root.VolumeId + PathHelper.EncodePath(Parent.RelativePath)
                };
            }
            response.Mime = MimeType;
            response.Read = 1;
            response.Write = (byte)(Root.AccessManager.IsReadOnly ? 0 : 1);
            response.Locked = (byte)(Root.AccessManager.IsLocked ? 1 : 0);
            response.Hash = Root.VolumeId + PathHelper.EncodePath(RelativePath);
            response.UnixTimeStamp = (long)(DirectoryInfo.LastWriteTimeUtc - UnixOrigin).TotalSeconds;
            response.Size = 0;
            return response;
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
