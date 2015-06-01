using System.Diagnostics.Contracts;
using System.IO;
using System.Web;

namespace ElFinder
{
    [ContractClass(typeof(ResponseBaseContract))]
    internal abstract class ResponseBase
    {
        public abstract void WriteResponse(HttpContext context);


        [ContractClassFor(typeof(ResponseBase))]
        private abstract class ResponseBaseContract : ResponseBase
        {
            public override void WriteResponse(HttpContext context)
            {
                Contract.Requires(context != null);
            }
        }
    }
}
