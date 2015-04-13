using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class Archive
    {
        [DataMember(Name = "create")]
        public string[] Create { get; private set; }

        [DataMember(Name = "extract")]
        public string[] Extract { get; private set; }

        public Archive()
        {
            Create = new string[0];
            Extract = new string[0];
        }
    }
}
