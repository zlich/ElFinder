using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class ListResponse : JsonResponse
    {
        [DataMember(Name="list")]
        public List<string> List { get; private set; }

        public ListResponse()
        {
            List = new List<string>();
        }     
    }
}