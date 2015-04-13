using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElFinder
{
    /// <summary>
    /// Represents a file info
    /// </summary>
    public interface IFileInfo : IUnitInfo
    {
        long Length { get; }

        string Extension { get; }

        string DirectoryName { get; }

        IDirectoryInfo Directory { get; }
    }
}
