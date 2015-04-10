using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElFinder
{
    public interface IFileSystemInfo
    {
        string MimeType { get; }

        string Name { get; }


        DateTime LastWriteTimeUtc { get; }
    }
}
