using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    public class RootDTO : UnitDTO
    {
        [DataMember(Name = "volumeId")]
        public string VolumeId { get; set; }

        [DataMember(Name = "dirs")]
        public byte ContainsChildDirs { get; set; }
    }
}