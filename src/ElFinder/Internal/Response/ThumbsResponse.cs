using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class ThumbsResponse : JsonResponse
    {
        [DataMember(Name = "images")]
        public Dictionary<string, string> Images { get; private set; }

        public ThumbsResponse()
        {
            Images = new Dictionary<string, string>();
        }
    }
}