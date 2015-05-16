using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class ChangedResponse : JsonResponse
    {
        [DataMember(Name="changed")]
        public List<FileDTO> Changed { get; private set; }

        public ChangedResponse()
        {
            Changed = new List<FileDTO>();
        }
    }
}