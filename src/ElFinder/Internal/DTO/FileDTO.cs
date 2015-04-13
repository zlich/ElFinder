using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class FileDTO : UnitDTO
    {          
        /// <summary>
        ///  Hash of parent directory. Required except roots dirs.
        /// </summary>
        [DataMember(Name = "phash")]
        public string ParentHash { get; set; }
    }
}