using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class RootDTO : BaseDTO
    {
        [DataMember(Name = "volumeId")]
        public string VolumeId { get; set; }

        [DataMember(Name = "dirs")]
        public byte Dirs { get; set; }
    }
}