using ElFinder;
using NUnit.Framework;
using System.IO;

namespace ElFinderTests
{
    [TestFixture]
    public class LocalFileInfoTests
    {
        [Test]
        public void TestName()
        {
            string name = "testText.txt";
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            LocalFileInfo info = new LocalFileInfo(root, TestHelper.GetTestDataPath(name));

            Assert.AreEqual(name, info.Name);
        }

        [Test]
        public void TestLength()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, path);

            Assert.AreEqual(new FileInfo(path).Length, info.Length);
        }

        [Test]
        public void TestMimeType()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, path);
            Assert.AreEqual("text/plain", info.MimeType);

            path = TestHelper.GetTestDataPath("testPicture.jpg");
            info = new LocalFileInfo(root, path);
            Assert.AreEqual("image/jpeg", info.MimeType);

            path = TestHelper.GetTestDataPath("testText");
            info = new LocalFileInfo(root, path);
            Assert.AreEqual("unknown", info.MimeType);
        }

        [Test]
        public void TestExtension()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, path);
            Assert.AreEqual(".txt", info.Extension);

            path = TestHelper.GetTestDataPath("testPicture.jpg");
            info = new LocalFileInfo(root, path);
            Assert.AreEqual(".jpg", info.Extension);

            path = TestHelper.GetTestDataPath("testText");
            info = new LocalFileInfo(root, path);
            Assert.AreEqual("", info.Extension);
        }

        [Test]
        public void TestDirectory()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, path);

            DirectoryInfo dir = new DirectoryInfo(TestHelper.TestDataPath);
            Assert.AreEqual(dir.Name, info.Directory.Name);
        }

        [Test]
        public void TestIsHidden()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, path);
            Assert.AreEqual(false, info.IsHidden);

            path = TestHelper.GetTestDataPath("testText");
            info = new LocalFileInfo(root, path);
            Assert.AreEqual(true, info.IsHidden);
        }

        [Test]
        public void TestExists()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, path);
            Assert.AreEqual(true, info.Exists);

            path = TestHelper.GetTestDataPath("testNotExistFile.txt");
            info = new LocalFileInfo(root, path);
            Assert.AreEqual(false, info.Exists);
        }

        [Test]
        public void TestRelativePath()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalFileInfo info = new LocalFileInfo(root, path);
            Assert.AreEqual("\\testText.txt", info.RelativePath);
        }

        [Test]
        public void TestRoot()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            LocalFileInfo info = new LocalFileInfo(root, path);
            Assert.AreEqual(root, info.Root);
        }

        [Test]
        public void TestCopyTo()
        {
            string path = TestHelper.GetTestDataPath("testText.txt");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            LocalFileInfo info = new LocalFileInfo(root, path);
            using (MemoryStream stream = new MemoryStream())
            {
                info.CopyTo(stream);
                Assert.AreEqual(TestHelper.GetFileHash(path), TestHelper.GetDataHash(stream.ToArray()));
            }
        }
    }
}
