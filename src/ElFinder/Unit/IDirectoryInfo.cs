using System.Collections.Generic;

namespace ElFinder
{    
    /// <summary>
    /// Respresnts a directory info.
    /// </summary>
    public interface IDirectoryInfo : IUnitInfo
    {
        /// <summary>
        /// Gets parent directory or <c>null</c> if directory is root.
        /// </summary>
        IDirectoryInfo Parent { get; }

        /// <summary>
        /// Gets subdirectories.
        /// </summary>
        /// <returns>The collection of subdirectories.</returns>
        IEnumerable<IDirectoryInfo> GetDirectories();

        /// <summary>
        /// Gets files.
        /// </summary>
        /// <returns>The collection of files.</returns>
        IEnumerable<IFileInfo> GetFiles();

        /// <summary>
        /// Gets files and directories.
        /// </summary>
        /// <returns>The collection of all units.</returns>
        IEnumerable<IUnitInfo> GetUnits();
    }
}
