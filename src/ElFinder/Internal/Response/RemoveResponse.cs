using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class RemoveResponse : JsonResponse
    {
        [DataMember(Name = "removed")]
        public List<string> Removed { get; private set; }

        public RemoveResponse()
        {
            Removed = new List<string>();
        }
    }
}