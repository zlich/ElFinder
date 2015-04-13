using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class OpenResponse : OpenResponseBase
    {
        public OpenResponse(IDirectoryInfo currentWorkingDirectory, FullPath fullPath)
            : base(currentWorkingDirectory)
        {
            Options = new Options(fullPath);
            Files.Add(currentWorkingDirectory.ToDTO());
        }
    }
}