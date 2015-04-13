using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace ElFinder
{
    internal class FileSystemFileInfo : UnitInfo, IFileInfo
    {
        public string Name
        {
            get { return m_info.Name; }
        }

        public string MimeType
        {
            get { return GetMimeType(); }
        }

        public string Extension
        {
            get { return m_info.Extension; }
        }

        public long Length
        {
            get { return m_info.Length; }
        }

        public IDirectoryInfo Directory
        {
            get { return new FileSystemDirectoryInfo(Root, m_info.DirectoryName); }
        }

        public override UnitDTO ToDTO()
        {
            string parentPath = info.Directory.FullName.Substring(root.Directory.FullName.Length);
            string relativePath = info.FullName.Substring(root.Directory.FullName.Length);
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
            response.Write = Root.IsReadOnly ? (byte)0 : (byte)1;
            response.Locked = Root.IsLocked ? (byte)1 : (byte)0;
            response.Name = m_info.Name;
            response.Size = m_info.Length;
            response.UnixTimeStamp = (long)(m_info.LastWriteTimeUtc - UnixOrigin).TotalSeconds;
            response.Mime = MimeType;
            response.Hash = Root.VolumeId + Helper.EncodePath(relativePath);
            response.ParentHash = Root.VolumeId + Helper.EncodePath(parentPath.Length > 0 ? parentPath : info.Directory.Name);
            return response;
        }

        public FileSystemFileInfo(IRoot root, string fileName)
            : base(root)
        {
            Contract.Requires(fileName != null);

            m_info = new FileInfo(fileName);
        }

        private string GetMimeType()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            string extension = m_info.Extension;
            if (extension.Length > 1)
                return MimeTypes.GetMimeType(extension.ToLower().Substring(1));
            else
                return "unknown";
        }

        private readonly FileInfo m_info;
    }
}
