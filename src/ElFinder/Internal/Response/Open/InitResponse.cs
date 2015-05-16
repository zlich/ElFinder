using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class InitResponse : OpenResponseBase
    {
        [DataMember(Name="api")]
        public string Api { get { return "2.0"; } }

        [DataMember(Name = "uplMaxSize")]
        public string UploadMaxSize { get; set; }

        [DataMember(Name = "netDrivers")]
        public IEnumerable<string> NetDrivers { get { return s_empty; } }

        public InitResponse(IDirectoryInfo currentWorkingDirectory)
            : base(currentWorkingDirectory)
        {
            Options = new Options(currentWorkingDirectory);
        }

        private static string[] s_empty = new string[0];
    }
}