using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElFinderTests
{
    internal static class TestHelper
    {
        public static string GetTestDataPath(string localPath)
        {
            return Path.Combine(TestDataPath, localPath);
        }

        private static readonly string TestDataPath = "..\\..\\..\\testData\\";
    }
}
