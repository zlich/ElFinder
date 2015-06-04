using System.Diagnostics.Contracts;
using System.IO;
using System.Web;

namespace ElFinder
{
    /// <summary>
    /// Represents a file info.
    /// </summary>
    [ContractClass(typeof(ContractForIFileInfo))]
    public interface IFileInfo : IUnitInfo
    {
        /// <summary>
        /// Gets file length in bytes.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Gets file extension.
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Gets parent directory.
        /// </summary>
        IDirectoryInfo Directory { get; }

        /// <summary>
        /// Copies file to stream.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <exception cref="System.ArgumentNullException">Trown when output is <c>null</c>.</exception>
        void CopyTo(Stream output);

        /// <summary>
        /// Is this file cached in HttpRequest.
        /// </summary>
        /// <param name="request">The HttpRequest.</param>
        /// <param name="response">The HttpResponse.</param>
        /// <returns>True if file cached, otherwise false.</returns>
        bool IsFileFromCache(HttpRequest request, HttpResponse response);
    }

    [ContractClassFor(typeof(IFileInfo))]
    internal abstract class ContractForIFileInfo :  IFileInfo
    {

        long IFileInfo.Length
        {
            get
            {
                Contract.Ensures(Contract.Result<long>() >= 0);
                return 0; 
            }
        }

        string IFileInfo.Extension
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return null;
            }
        }

        IDirectoryInfo IFileInfo.Directory
        {
            get
            {
                Contract.Ensures(Contract.Result<IDirectoryInfo>() != null);
                return null;
            }
        }

        void IFileInfo.CopyTo(Stream output)
        {
            Contract.Requires(output != null);
        }

        bool IFileInfo.IsFileFromCache(HttpRequest request, HttpResponse response)
        {
            return true;
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
