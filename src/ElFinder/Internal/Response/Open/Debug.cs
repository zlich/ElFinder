using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class Debug : JsonResponse
    {
        [DataMember(Name = "connector")]
        public string Connector { get { return ".NET"; } }
    }
}
