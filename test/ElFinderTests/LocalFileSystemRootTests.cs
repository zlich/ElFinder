using ElFinder;
using NUnit.Framework;
using System;
using System.IO;

namespace ElFinderTests
{
    [TestFixture]
    public class LocalFileSystemRootTests
    {
        [Test]
        public void TestVolumeId()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            Assert.IsNull(root.VolumeId);

            Connector connector = new Connector();
            connector.AddRoot(root);

            Assert.IsNotNull(root.VolumeId);
            Assert.AreEqual("v1_", root.VolumeId);
        }

        [Test]
        public void TestAlias()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            Assert.IsNull(root.Alias);
            string alias = "Custom root alias";
            root.Alias = alias;
            Assert.AreEqual(alias, root.Alias);
        }

        [Test]
        public void TestUrl()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            Assert.IsNull(root.Url);
            string url = "http://consoto.com";
            root.Url = url;
            Assert.AreEqual(url, root.Url);
        }

        [Test]
        public void TestDirectory()
        {
            DirectoryInfo info = null;
            Assert.Throws<ArgumentNullException>(() => new LocalFileSystemRoot(info));

            string path = null;
            Assert.Throws<ArgumentNullException>(() => new LocalFileSystemRoot(path));

            path = "C:/NotExistFolder_111";
            Assert.Throws<ArgumentException>(() => new LocalFileSystemRoot(path));

            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            Assert.AreEqual("testData", root.Directory.Name);
        }

        [Test]
        public void TestStartPath()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            Assert.IsNull(root.StartPath);

            LocalFileSystemRoot anotherRoot = new LocalFileSystemRoot("C:/");

            Assert.Throws<ArgumentException>(() => root.StartPath = anotherRoot.GetDirectory("Program Files"));
            Assert.Throws<ArgumentException>(() => root.StartPath = root.GetDirectory("notExistFolder"));

            root.StartPath = root.GetDirectory("\\subfolder");
            Assert.AreEqual("subfolder", root.StartPath.Name);

            root.StartPath = root.GetDirectory("subfolder");
            Assert.AreEqual("subfolder", root.StartPath.Name);

            root.StartPath = root.GetDirectory("/subfolder");
            Assert.AreEqual("subfolder", root.StartPath.Name);

            root.StartPath = root.GetDirectory("subfolder/");
            Assert.AreEqual("subfolder", root.StartPath.Name);

            root.StartPath = root.GetDirectory("/subfolder\\");
            Assert.AreEqual("subfolder", root.StartPath.Name);
        }

        [Test]
        public void TestGetFile()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            Assert.Throws<ArgumentNullException>(() => root.GetFile(null));

            IFileInfo info = root.GetFile("subfolder/subfolder file.txt");
            Assert.AreEqual(true, info.Exists);
            Assert.AreEqual("subfolder file.txt", info.Name);

            info = root.GetFile("\\subfolder/subfolder file.txt");
            Assert.AreEqual(true, info.Exists);
            Assert.AreEqual("subfolder file.txt", info.Name);

            info = root.GetFile("subfolder\\subfolder file.txt");
            Assert.AreEqual(true, info.Exists);
            Assert.AreEqual("subfolder file.txt", info.Name);

            info = root.GetFile("testText.txt");
            Assert.AreEqual(true, info.Exists);
            Assert.AreEqual("testText.txt", info.Name);

            info = root.GetFile("\\testText.txt");
            Assert.AreEqual(true, info.Exists);
            Assert.AreEqual("testText.txt", info.Name);
        }
    }
}
