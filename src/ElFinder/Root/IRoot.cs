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

        IDirectoryInfo StartPath { get; set; }

        ThumbnailsManager ThumbnailsManager { get; }

        AccessManager AccessManager { get; }

        IDirectoryInfo GetDirectory(string relativePath);

        IFileInfo GetFile(string relativePath);

        IDirectoryInfo CreateDirectory(string relativeDir, string name);

        IFileInfo CreateFile(string relativeDir, string name);

        IFileInfo RenameFile(string relativePath, string name);

        IDirectoryInfo RenameDir(string relativePath, string name);
    }
}
