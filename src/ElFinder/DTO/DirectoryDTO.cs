using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class DirectoryDTO : BaseDTO
    {           
        /// <summary>
        ///  Hash of parent directory. Required except roots dirs.
        /// </summary>
        [DataMember(Name = "phash")]
        public string ParentHash { get; set; }
        
        /// <summary>
        /// Is directory contains subfolders
        /// </summary>
        [DataMember(Name = "dirs")]
        public byte ContainsChildDirs { get; set; }
    }
}