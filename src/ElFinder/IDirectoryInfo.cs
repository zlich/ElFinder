using System.Collections.Generic;

namespace ElFinder
{    
    /// <summary>
    /// Respresnts a directory info
    /// </summary>
    public interface IDirectoryInfo : IUnitInfo
    {
        /// <summary>
        /// Gets subdirectories
        /// </summary>
        /// <returns>The collection of subdirectories</returns>
        IEnumerable<IDirectoryInfo> GetDirectories();
    }
}
