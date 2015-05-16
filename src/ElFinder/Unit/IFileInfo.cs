using System.IO;
using System.Web;

namespace ElFinder
{
    /// <summary>
    /// Represents a file info.
    /// </summary>
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
}
