using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class OpenResponseBase : JsonResponse
    {
        [DataMember(Name = "files")]
        public List<UnitDTO> Files { get; private set; }

        [DataMember(Name = "cwd")]
        public UnitDTO CurrentWorkingDirectory { get; private set; }

        [DataMember(Name = "options")]
        public Options Options { get; protected set; }

        [DataMember(Name = "debug")]
        public Debug Debug { get; private set; }

        public OpenResponseBase(IDirectoryInfo currentWorkingDirectory)
        {
            Contract.Requires(currentWorkingDirectory != null);
            Files = new List<UnitDTO>();
            Debug = new Debug();
            CurrentWorkingDirectory = currentWorkingDirectory.ToDTO();
        }
    }
}