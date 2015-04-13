using System.IO;
using System.Web;

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

        void CopyTo(Stream output);

        bool ProcessHttpCache(HttpRequest request, HttpResponse response);
    }
}
