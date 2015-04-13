using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class OpenResponse : OpenResponseBase
    {
        public OpenResponse(IDirectoryInfo currentWorkingDirectory)
            : base(currentWorkingDirectory)
        {
            Options = new Options(currentWorkingDirectory);
            Files.Add(currentWorkingDirectory.ToDTO());
        }
    }
}