using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElFinder
{
    public class AccessManager
    {
        /// <summary>
        /// Get or sets if root for read only (users can't change file)
        /// </summary>
        public bool IsReadOnly { get; set; }
        

        public bool IsShowOnly { get; set; }

        /// <summary>
        /// Get or sets if root is locked (user can't remove, rename or delete files or subdirectories)  
        /// </summary>
        public bool IsLocked { get; set; }


        /// <summary>
        /// Get or sets maximum upload file size. This size is per files in bytes. 
        /// Note: you still to configure maxupload limits in web.config for whole application
        /// </summary>
        public int? MaxUploadSize
        {
            get { return m_maxUploadSize; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Max upload size can not be less than zero", "value");
                m_maxUploadSize = value;
            }
        }

        /// <summary>
        /// Get or sets maximum upload file size. This size is per files in kb. 
        /// Note: you still to configure maxupload limits in web.config for whole application
        /// </summary>
        public double? MaxUploadSizeInKb
        {
            get { return MaxUploadSize / 1024.0; }
            set { MaxUploadSize = (int?)(value * 1024); }
        }

        /// <summary>
        /// Get or sets maximum upload file size. This size is per files in Mb. 
        /// Note: you still to configure maxupload limits in web.config for whole application
        /// </summary>
        public double? MaxUploadSizeInMb
        {
            get { return MaxUploadSizeInKb/ 1024.0; }
            set { MaxUploadSizeInKb = (int?)(value * 1024); }
        }

        private int? m_maxUploadSize;
    }
}
