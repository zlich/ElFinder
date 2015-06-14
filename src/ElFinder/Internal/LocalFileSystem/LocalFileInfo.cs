using System.Diagnostics.Contracts;
using System.IO;
using System.Web;

namespace ElFinder
{
    internal class LocalFileInfo : LocalUnitInfo, IFileInfo
    {
        public override string MimeType
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                string extension = Extension;
                if (extension.Length > 1)
                    return MimeTypes.GetMimeType(extension.ToLower().Substring(1));
                else
                    return "unknown";
            }
        }

        public string Extension
        {
            get { return FileInfo.Extension; }
        }

        public long Length
        {
            get { return FileInfo.Length; }
        }

        public IDirectoryInfo Directory
        {
            get { return new LocalDirectoryInfo(FileSystemRoot, FileInfo.Directory); }
        }

        public override UnitDTO ToDTO()
        {
            FileDTO response;

            //TODO: images generation
            //if (Root.CanCreateThumbnail(info))
            //{
            //    ImageDTO imageResponse = new ImageDTO();
            //    imageResponse.Thumbnail = root.GetExistingThumbHash(info) ?? (object)1;
            //    var dim = root.GetImageDimension(info);
            //    imageResponse.Dimension = string.Format("{0}x{1}", dim.Width, dim.Height);
            //    response = imageResponse;
            //}
            //else
            //{
                response = new FileDTO();
            //}
            response.Read = 1;
            response.Write = (byte)(Root.AccessManager.IsReadOnly ? 0 : 1);
            response.Locked = (byte)(Root.AccessManager.IsLocked ? 1 : 0);
            response.Name = Name;
            response.Size = FileInfo.Length;
            response.UnixTimeStamp = (long)(FileInfo.LastWriteTimeUtc - UnixOrigin).TotalSeconds;
            response.Mime = MimeType;
            response.Hash = Root.VolumeId + PathHelper.EncodePath(RelativePath);
            response.ParentHash = Root.VolumeId + PathHelper.EncodePath(Directory.RelativePath);
            return response;
        }

        public override void Delete()
        {
            File.Delete(FileInfo.FullName);
        }
        
        public void CopyTo(Stream output)
        {
            using (Stream readFrom = OpenRead())
            {
                readFrom.CopyTo(output);
            }
        }

        public void CopyTo(IFileInfo output)
        {
            LocalFileInfo localSystemFile = output as LocalFileInfo;
            //if (localSystemFile != null) // fast copy for localsystem units
            //{
            //    File.Copy(FileInfo.FullName, localSystemFile.FileInfo.FullName, true);
            //}
            //else
            {
                using (Stream writeTo = output.OpenWrite())
                {
                    CopyTo(writeTo);
                }
            }
        }

        public void CutTo(IFileInfo output)
        {
            LocalFileInfo localSystemFile = output as LocalFileInfo;
            //if (localSystemFile != null) // fast move for localsystem units
            //{
            //    File.Move(FileInfo.FullName, localSystemFile.FileInfo.FullName);
            //}
            //else
            {
                using (Stream writeTo = output.OpenWrite())
                {
                    CopyTo(writeTo);
                }
            }
            Delete();
        }

        public Stream OpenRead()
        {
            return FileInfo.OpenRead();
        }

        public Stream OpenWrite()
        {
            return FileInfo.Open(FileMode.Create, FileAccess.Write);
        }

        public bool IsFileFromCache(HttpRequest request, HttpResponse response)
        {
            return HttpCacheHelper.IsFileFromCache(Name, FileInfo.LastWriteTimeUtc, request, response);
        }

        public LocalFileInfo(LocalFileSystemRoot root, string fileName)
            : this(root, new FileInfo(fileName)) { }

        public LocalFileInfo(LocalFileSystemRoot root, FileInfo info)
            : base(root, info) { }

        private FileInfo FileInfo
        {
            get { return (FileInfo)Info; }
        }
    }
}
