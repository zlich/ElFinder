using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElFinder.Response
{
    [DataContract]
    internal class TreeResponse : JsonResponse
    {
        [DataMember(Name="tree")]
        public List<UnitDTO> Tree { get; private set; }

        public TreeResponse()
        {
            Tree = new List<UnitDTO>();
        }     
    }
}