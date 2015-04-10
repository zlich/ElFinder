using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElFinder
{
    public interface IFileInfo : IFileSystemInfo
    {
        long Length { get; }

        string Extension { get; }

        string DirectoryName { get; }

        IDirectoryInfo Directory { get; }
    }
}
