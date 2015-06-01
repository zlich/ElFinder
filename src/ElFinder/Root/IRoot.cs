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

        void DeleteFile(string relativePath); 
        #endregion

        #region Directory operations
        IDirectoryInfo GetDirectory(string relativePath);

        IDirectoryInfo CreateDirectory(string relativeDir, string name);

        IDirectoryInfo RenameDirectory(string relativePath, string name);

        void DeleteDirectory(string relativePath); 
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
                throw new NotImplementedException();
            }
            set { Contract.Requires(!string.IsNullOrEmpty(value)); }
        }

        string IRoot.Alias
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string IRoot.Url
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        IDirectoryInfo IRoot.Directory
        {
            get { throw new NotImplementedException(); }
        }

        IDirectoryInfo IRoot.StartPath
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        ThumbnailsManager IRoot.ThumbnailsManager
        {
            get { throw new NotImplementedException(); }
        }

        AccessManager IRoot.AccessManager
        {
            get { throw new NotImplementedException(); }
        }

        IFileInfo IRoot.GetFile(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<IFileInfo>() != null);

            throw new NotImplementedException();
        }

        IFileInfo IRoot.CreateFile(string relativeDir, string name)
        {
            Contract.Requires(relativeDir != null);
            Contract.Requires(name != null);
            Contract.Ensures(Contract.Result<IFileInfo>() != null);

            throw new NotImplementedException();
        }

        IFileInfo IRoot.RenameFile(string relativePath, string name)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<IFileInfo>() != null);

            throw new NotImplementedException();
        }

        void IRoot.DeleteFile(string relativePath)
        {
            Contract.Requires(relativePath != null);

            throw new NotImplementedException();
        }

        IDirectoryInfo IRoot.GetDirectory(string relativePath)
        {
            Contract.Requires(relativePath != null);
            Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);

            throw new NotImplementedException();
        }

        IDirectoryInfo IRoot.CreateDirectory(string relativeDir, string name)
        {
            Contract.Requires(relativeDir != null);
            Contract.Requires(name != null);
            Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);

            throw new NotImplementedException();
        }

        IDirectoryInfo IRoot.RenameDirectory(string relativePath, string name)
        {
            Contract.Requires(relativePath != null);
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);

            throw new NotImplementedException();
        }

        void IRoot.DeleteDirectory(string relativePath)
        {
            Contract.Requires(relativePath != null);

            throw new NotImplementedException();
        }
    }
}
