using System.IO;

namespace ElFinderTests
{
    internal class MemoryWriterMock : StreamWriter
    {
        public MemoryWriterMock()
            : base(new MemoryStream()) { }
    }
}