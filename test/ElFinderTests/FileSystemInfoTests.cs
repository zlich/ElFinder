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
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, TestHelper.GetTestDataPath(name));

            Assert.AreEqual(name, info.Name);
        }

        [TestMethod]
        public void TestLength()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, path);

            Assert.AreEqual(new FileInfo(path).Length, info.Length);
        }

        [TestMethod]
        public void TestMimeType()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            LocalFileInfo info = new LocalFileInfo(root, "testText.txt");
            Assert.AreEqual("text/plain", info.MimeType);

            info = new LocalFileInfo(root, "testPicture.jpg");
            Assert.AreEqual("image/jpeg", info.MimeType);
        }
    }
}
