using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    public class ImageDTO : FileDTO
    {   
        [DataMember(Name = "tmb")]
        public object Thumbnail { get; set; }

        [DataMember(Name = "dim")]
        public string Dimension { get; set; }
    }
}