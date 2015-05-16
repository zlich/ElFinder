using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Net;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace ElFinder
{
    public class Driver
    {
        /// <summary>
        /// Gets roots array.
        /// </summary>
        public ReadOnlyCollection<IRoot> Roots
        {
            get { return m_roots.AsReadOnly(); }
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
            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();

            OpenResponse response = new OpenResponse(directory);
            response.Files.AddRange(directory.GetUnits().Where(i => !i.IsHidden).Select(i => i.ToDTO()));

            return response;
        }

        internal JsonResponse Init(string target)
        {
            IDirectoryInfo directory;
            if (string.IsNullOrEmpty(target))
            {
                IRoot root = m_roots.FirstOrDefault(r => r.StartPath != null);
                if (root == null)
                    root = m_roots.First();
                directory = root.StartPath ?? root.Directory;
            }
            else
            {
                directory = ParsePath(target) as IDirectoryInfo;
            }
            if (directory == null)
                return Errors.NotFound();

            InitResponse response = new InitResponse(directory);

            response.Files.AddRange(directory.GetUnits().Where(i => !i.IsHidden).Select(i => i.ToDTO()));
            response.Files.AddRange(m_roots.Select(i => i.Directory.ToDTO()));

            IRoot currentRoot = directory.Root;

            if (directory.RelativePath != directory.Name)
                response.Files.AddRange(currentRoot.Directory.GetDirectories().Where(i => !i.IsHidden).Select(i => i.ToDTO()));

            if (currentRoot.AccessManager.MaxUploadSize.HasValue)
                response.UploadMaxSize = currentRoot.AccessManager.MaxUploadSizeInKb.Value + "K";

            return response;
        }
        
        internal JsonResponse Parents(string target)
        {
            IDirectoryInfo directory = ParsePath(target) as IDirectoryInfo;
            if (directory == null)
                return Errors.NotFound();
            TreeResponse answer = new TreeResponse();
            if (directory.RelativePath == string.Empty)
            {
                answer.Tree.Add(directory.ToDTO());
            }
            else
            {
                foreach (IDirectoryInfo item in directory.Parent.GetDirectories())
                {
                    answer.Tree.Add(item.ToDTO());
                }

                IDirectoryInfo parent = directory.Parent;
                while (parent.RelativePath != string.Empty)
                {
                    answer.Tree.Add(parent.ToDTO());
                    parent = parent.Parent;
                }
                answer.Tree.Add(parent.ToDTO());
            }
            return answer;
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
            IUnitInfo unit = ParsePath(target);
            if (unit is IDirectoryInfo)
                return new HttpStatusCodeResponse(HttpStatusCode.Forbidden, "You can not download whole folder");
            else
            {
                IFileInfo file = (IFileInfo)unit;
                if (!file.Exists)
                    return new HttpStatusCodeResponse(HttpStatusCode.NotFound, "File not found");
                if (file.Root.AccessManager.IsShowOnly)
                    return new HttpStatusCodeResponse(HttpStatusCode.Forbidden, "Access denied. Volume is for show only");

                return new DownloadResponse(file, download);
            }
        }

        private IUnitInfo ParsePath(string target)
        {
            Contract.Requires(target != null);
            int separatorIndex = target.IndexOf('_');
            if (separatorIndex == -1)
                throw new ArgumentException("Target path must containt a separator between volumeIndex");
            separatorIndex++;
            string volumePrefix = target.Substring(0, separatorIndex);
            string pathHash = target.Substring(separatorIndex);

            IRoot root = m_roots.First(r => r.VolumeId == volumePrefix);
            string relativePath = Helper.DecodePath(pathHash);
            IDirectoryInfo dir = root.GetDirectory(relativePath);
            if (dir.Exists)
                return dir;
            else
                return root.GetFile(relativePath);
        }

        private readonly List<IRoot> m_roots;
        private readonly string _volumePrefix = "v";
    }
}
