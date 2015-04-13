using System;
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
            response.Hash = Root.VolumeId + Helper.EncodePath(RelativePath);
            response.ParentHash = Root.VolumeId + Helper.EncodePath(Directory.RelativePath);
            return response;
        }

        public void CopyTo(Stream output)
        {
            Contract.Requires(output != null);
            using (FileStream file = FileInfo.OpenRead())
            {
                file.CopyTo(output);
            }
        }

        public bool ProcessHttpCache(HttpRequest request, HttpResponse response)
        {
            return HttpCacheHelper.IsFileFromCache(Name, FileInfo.LastWriteTimeUtc, request, response);
        }

        public LocalFileInfo(LocalFileSystemRoot root, string fileName)
            : base(root, new FileInfo(fileName)) { }

        public LocalFileInfo(LocalFileSystemRoot root, FileInfo info)
            : base(root, info) { }

        private FileInfo FileInfo
        {
            get { return (FileInfo)Info; }
        }
    }
}
