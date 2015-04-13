namespace ElFinder
{
    /// <summary>
    /// Represents a root in fileManger
    /// </summary>
    public interface IRoot
    {
        string VolumeId { get; set; }

        string Alias { get; set; }

        string Url { get; set; }

        IDirectoryInfo Directory { get; }

        IDirectoryInfo StartPath { get; }

        ThumbnailsManager ThumbnailsManager { get; }

        AccessManager AccessManager { get; }

        IDirectoryInfo GetDirectory(string relativePath);

        IFileInfo GetFile(string relativePath);
    }
}
