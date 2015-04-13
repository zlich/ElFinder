using System;
using System.Collections.Generic;
using System.Web;

namespace ElFinder
{
    public class Driver
    {
        /// <summary>
        /// Gets roots array.
        /// </summary>
        public IRoot[] Roots
        {
            get { return m_roots.ToArray(); }
        }

        /// <summary>
        /// Adds an object to the end of the roots.
        /// </summary>
        /// <param name="item">The root</param>
        /// <exception cref="ArgumentNullException">Thrown when value is null</exception>
        public void AddRoot(IRoot item)
        {
            if(item == null)
                throw new ArgumentNullException();
            m_roots.Add(item);
            item.VolumeId = _volumePrefix + m_roots.Count + "_";
        }

        /// <summary>
        /// Initializes new instance of class <see cref="Driver"/>.
        /// </summary>
        public Driver()
        {
            m_roots = new List<IRoot>();
        }

        internal JsonResponse Open(string target, bool tree)
        {
            FullPath fullPath = ParsePath(target);
            OpenResponse response = new OpenResponse(DTOBase.Create(fullPath.Directory, fullPath.Root), fullPath);
            foreach (FileInfo item in fullPath.Directory.GetFiles())
            {
                if ((item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    response.Files.Add(DTOBase.Create(item, fullPath.Root));
            }
            foreach (DirectoryInfo item in fullPath.Directory.GetDirectories())
            {
                if ((item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    response.Files.Add(DTOBase.Create(item, fullPath.Root));
            }
            return response;
        }

        internal JsonResponse Init(string target)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Parents(string target)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Tree(string target)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse List(string target)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse MakeDir(string target, string name)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse MakeFile(string target, string name)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Rename(string target, string name)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Remove(IEnumerable<string> targets)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Duplicate(IEnumerable<string> targets)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Get(string target)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Put(string target, string content)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Paste(string source, string dest, IEnumerable<string> targets, bool isCut)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Upload(string target, HttpFileCollection targets)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Thumbs(IEnumerable<string> targets)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Dim(string target)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Resize(string target, int width, int height)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Crop(string target, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }
        internal JsonResponse Rotate(string target, int degree)
        {
            throw new NotImplementedException();
        }

        internal ResponseBase File(string target, bool download)
        {
            FullPath fullPath = ParsePath(target);
            //TODO: implement
            //if (fullPath.IsDirectoty)
            //    return new HttpStatusCodeResult(403, "You can not download whole folder");
            //if (!fullPath.File.Exists)
            //    return new HttpNotFoundResult("File not found");
            //if (fullPath.Root.IsShowOnly)
            //    return new HttpStatusCodeResult(403, "Access denied. Volume is for show only");
            return new DownloadResponse(fullPath.File, download);
        }

        private readonly List<IRoot> m_roots;
        private readonly string _volumePrefix = "v";
    }
}
