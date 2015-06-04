using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ElFinder
{    
    /// <summary>
    /// Respresnts a directory info.
    /// </summary>
    [ContractClass(typeof(ContractForIDirectoryInfo))]
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

    [ContractClassFor(typeof(IDirectoryInfo))]
    internal abstract class ContractForIDirectoryInfo : IDirectoryInfo
    {
        IDirectoryInfo IDirectoryInfo.Parent
        {
            get
            {
                Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);
                return null;
            }
        }

        IEnumerable<IDirectoryInfo> IDirectoryInfo.GetDirectories()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IDirectoryInfo>>() != null);
            return null;
        }

        IEnumerable<IFileInfo> IDirectoryInfo.GetFiles()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IFileInfo>>() != null);
            return null;
        }

        IEnumerable<IUnitInfo> IDirectoryInfo.GetUnits()
        {
            Contract.Ensures(Contract.Result<IEnumerable<IUnitInfo>>() != null);
            return null;
        }

        public abstract string Name { get; }

        public abstract IRoot Root { get; }

        public abstract string MimeType { get; }

        public abstract bool Exists { get; }

        public abstract string RelativePath { get; }

        public abstract bool IsHidden { get; }

        public abstract UnitDTO ToDTO();
    }
}
