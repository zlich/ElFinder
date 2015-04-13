using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace ElFinder
{
    [DataContract]
    internal class ErrorResponse : JsonResponse
    {
        [DataMember(Name = "error")]
        public object Error { get; private set; }

        public ErrorResponse(string error)
        {
            Contract.Requires(error != null);
            Error = error;
        }

        public ErrorResponse(params string[] errors)
        {
            Contract.Requires(errors != null);
            Error = errors;
        }
    }
}
