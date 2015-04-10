using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElFinder;
using System.IO;

namespace ElFinderTests
{
    [TestClass]
    public class FileSystemInfoTests
    {
        [TestMethod]
        public void TestName()
        {
            string name = "testText.txt";
            FileSystemFileInfo info = new FileSystemFileInfo(TestHelper.GetTestDataPath(name));

            Assert.AreEqual(name, info.Name);
        }

        [TestMethod]
        public void TestLength()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            FileSystemFileInfo info = new FileSystemFileInfo(path);

            Assert.AreEqual(new FileInfo(path).Length, info.Length);
        }

        [TestMethod]
        public void TestLastWriteTimeUtc()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            FileSystemFileInfo info = new FileSystemFileInfo(path);

            Assert.AreEqual(new FileInfo(path).LastWriteTimeUtc, info.LastWriteTimeUtc);
        }

        [TestMethod]
        public void TestMimeType()
        {
            FileSystemFileInfo info = new FileSystemFileInfo("testText.txt");
            Assert.AreEqual("text/plain", info.MimeType);

            info = new FileSystemFileInfo("testPicture.jpg");
            Assert.AreEqual("image/jpeg", info.MimeType);
        }
    }
}
