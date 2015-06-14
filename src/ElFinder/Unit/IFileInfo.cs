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
        /// Copies file to stream.
        /// </summary>
        /// <param name="output">The output stream.</param>
        void CopyTo(Stream output);

        /// <summary>
        /// Copies file to another file info.
        /// </summary>
        /// <param name="output">The output fileInfo.</param>
        void CopyTo(IFileInfo output);

        /// <summary>
        /// Cuts file to another file info.
        /// </summary>
        /// <param name="output">The output fileInfo.</param>
        void CutTo(IFileInfo output);

        /// <summary>
        /// Opens stream for read file.
        /// </summary>
        /// <returns>The stream for read file.</returns>
        Stream OpenRead();

        /// <summary>
        /// Opens stream for write file.
        /// </summary>
        /// <returns>The stream for write file.</returns>
        Stream OpenWrite();

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
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        void IFileInfo.CopyTo(Stream output)
        {
            Contract.Requires(output != null);
        }

        void IFileInfo.CopyTo(IFileInfo output)
        {
            Contract.Requires(output != null);
        }

        void IFileInfo.CutTo(IFileInfo output)
        {
            Contract.Requires(output != null);
        }

        Stream IFileInfo.OpenRead()
        {
            Contract.Ensures(Contract.Result<Stream>() != null);
            return null;
        }

        Stream IFileInfo.OpenWrite()
        {
            Contract.Ensures(Contract.Result<Stream>() != null);
            return null;
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

        public abstract IDirectoryInfo Parent { get; }

        public abstract void Delete();

        public abstract UnitDTO ToDTO();
    }
}
