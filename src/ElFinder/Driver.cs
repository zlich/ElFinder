using System;
using System.Collections.Generic;

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
        ResponseBase Open(string target, bool tree)
        {
            FullPath fullPath = ParsePath(target);
            OpenResponse response = new OpenResponse( DTOBase.Create(fullPath.Directory, fullPath.Root), fullPath);
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

        private readonly List<IRoot> m_roots;
        private readonly string _volumePrefix = "v";
    }
}
