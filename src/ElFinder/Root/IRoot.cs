using System;
using System.Diagnostics.Contracts;
namespace ElFinder
{
    /// <summary>
    /// Represents a root in fileManger
    /// </summary>
    [ContractClass(typeof(ContractForIRoot))]
    public interface IRoot
    {
        string VolumeId { get; set; }

        string Alias { get; set; }

        string Url { get; set; }

        IDirectoryInfo Directory { get; }

        IDirectoryInfo StartPath { get; set; }

        ThumbnailsManager ThumbnailsManager { get; }

        AccessManager AccessManager { get; }


        #region File operations
        IFileInfo GetFile(string relativePath);

        IFileInfo CreateFile(string relativeDir, string name);

        IFileInfo RenameFile(string relativePath, string name);

        #endregion

        #region Directory operations
        IDirectoryInfo GetDirectory(string relativePath);

        IDirectoryInfo CreateDirectory(string relativeDir, string name);

        IDirectoryInfo RenameDirectory(string relativePath, string name);

        #endregion
    }

    [ContractClassFor(typeof(IRoot))]
    internal abstract class ContractForIRoot : IRoot
    {
        string IRoot.VolumeId
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
            set { Contract.Requires(!string.IsNullOrEmpty(value)); }
        }

        string IRoot.Alias
        {
            get { return null; }
            set { }
        }

        string IRoot.Url
        {
            get { return null; }
            set { }
        }

        IDirectoryInfo IRoot.Directory
        {
            get
            {
                Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);
                return null;
            }
        }

        IDirectoryInfo IRoot.StartPath
        {
            get { return null; }
            set {  }
        }

        ThumbnailsManager IRoot.ThumbnailsManager
        {
            get
            {
                Contract.Ensures(Contract.Result<ThumbnailsManager>() != null);
                return null;
            }
        }

        AccessManager IRoot.AccessManager
        {
            get
            {
                Contract.Ensures(Contract.Result<AccessManager>() != null);
                return null;
            }
        }

        IFileInfo IRoot.GetFile(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<IFileInfo>() != null);

            return null;
        }

        IFileInfo IRoot.CreateFile(string relativeDir, string name)
        {
            Contract.Requires(relativeDir != null);
            Contract.Requires(name != null);
            Contract.Ensures(Contract.Result<IFileInfo>() != null);

            return null;
        }

        IFileInfo IRoot.RenameFile(string relativePath, string name)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<IFileInfo>() != null);

            return null;
        }

        IDirectoryInfo IRoot.GetDirectory(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);

            return null;
        }

        IDirectoryInfo IRoot.CreateDirectory(string relativeDir, string name)
        {
            Contract.Requires(relativeDir != null);
            Contract.Requires(name != null);
            Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);

            return null;
        }

        IDirectoryInfo IRoot.RenameDirectory(string relativePath, string name)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);

            return null;
        }
    }
}
