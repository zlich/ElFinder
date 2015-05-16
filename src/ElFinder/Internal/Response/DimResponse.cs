using System.Drawing;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class DimResponse : JsonResponse
    {
        [DataMember(Name="dim")]
        public string Dimension { get; private set; }

        public DimResponse(Size size)
        {
            Dimension = string.Format("{0}x{1}", size.Width, size.Height);
        }
    }
}