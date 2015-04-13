namespace ElFinder
{
    /// <summary>
    /// Represents a root in fileManger
    /// </summary>
    public interface IRoot
    {
        string VolumeId { get; internal set; }

        string Alias { get; set; }

        string Url { get; set; }

        bool IsReadOnly { get; set; }

        bool IsShowOnly { get; set; }

        bool IsLocked { get; set; }

        int? MaxUploadSize { get; set; }

        bool UploadOverwrite { get; set; }
    }
}
