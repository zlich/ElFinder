using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElFinder.Response
{
    [DataContract]
    internal class ReplaceResponse : JsonResponse
    {
        [DataMember(Name = "added")]
        public List<UnitDTO> Added { get; private set; }

        [DataMember(Name = "removed")]
        public List<string> Removed { get; private set; }

        public ReplaceResponse()
        {
            Added = new List<UnitDTO>();
            Removed = new List<string>();
        }     
    }
}