using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class GetResponse : JsonResponse
    {
        [DataMember(Name="content")]
        public string Content { get; private set; }

        public GetResponse(string content)
        {
            Contract.Requires(content != null);
            Content = content;
        }
    }
}