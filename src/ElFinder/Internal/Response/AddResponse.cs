using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class AddResponse : JsonResponse
    {
        [DataMember(Name = "added")]
        public List<UnitDTO> Added { get; private set; }

        public AddResponse(IUnitInfo newFile)
        {
            Contract.Requires(newFile != null);
            Added = new List<UnitDTO>() { newFile.ToDTO() };
        }

        public AddResponse()
        {
            Added = new List<UnitDTO>();
        }
    }
}