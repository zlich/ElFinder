using ElFinder;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace ElFinderTests
{
    [TestFixture]
    public class LocalDirectoryInfoTests
    {
        [Test]
        public void TestName()
        {
            string name = "subfolder";
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            LocalDirectoryInfo info = new LocalDirectoryInfo(root, TestHelper.GetTestDataPath(name));
            Assert.AreEqual(name, info.Name);
        }

        [Test]
        public void TestMimeType()
        {
            string name = "subfolder";
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            LocalDirectoryInfo info = new LocalDirectoryInfo(root, TestHelper.GetTestDataPath(name));
            Assert.AreEqual("directory", info.MimeType);
        }

        [Test]
        public void TestParent()
        {
            string path = TestHelper.GetTestDataPath("subfolder/1");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            LocalDirectoryInfo info = new LocalDirectoryInfo(root, path);

            LocalDirectoryInfo parent = new LocalDirectoryInfo(root, TestHelper.GetTestDataPath("subfolder"));
            Assert.AreEqual(info.Parent.RelativePath, parent.RelativePath);
        }

        [Test]
        public void TestIsHidden()
        {
            string path = TestHelper.GetTestDataPath("subfolder");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalDirectoryInfo info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual(false, info.IsHidden);

            path = TestHelper.GetTestDataPath("subfolder/1");
            info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual(true, info.IsHidden);
        }

        [Test]
        public void TestExists()
        {
            string path = TestHelper.GetTestDataPath("subfolder");
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            LocalDirectoryInfo info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual(true, info.Exists);

            path = TestHelper.GetTestDataPath("subfolderNotExists.txt");
            info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual(false, info.Exists);
        }

        [Test]
        public void TestRelativePath()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);

            string path = TestHelper.GetTestDataPath("subfolder/2");
            LocalDirectoryInfo info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual("\\subfolder\\2", info.RelativePath);

            path = TestHelper.GetTestDataPath("subfolder\\2");
            info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual("\\subfolder\\2", info.RelativePath);

            path = TestHelper.GetTestDataPath("subfolder\\2/");
            info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual("\\subfolder\\2", info.RelativePath);

            path = TestHelper.GetTestDataPath("subfolder\\2\\");
            info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual("\\subfolder\\2", info.RelativePath);
        }

        [Test]
        public void TestRoot()
        {            
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            string path = TestHelper.GetTestDataPath("subfolder");
            LocalDirectoryInfo info = new LocalDirectoryInfo(root, path);
            Assert.AreEqual(root, info.Root);
        }

        [Test]
        public void TestGetDirectories()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            string path = TestHelper.GetTestDataPath("subfolder");
            LocalDirectoryInfo info = new LocalDirectoryInfo(root, path);
            List<IDirectoryInfo> dirs = info.GetDirectories().ToList();
            Assert.AreEqual(2, dirs.Count);
            Assert.AreEqual("1", dirs[0].Name);
            Assert.AreEqual("2", dirs[1].Name);
        }

        [Test]
        public void TestGetUnits()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            string path = TestHelper.GetTestDataPath("subfolder");
            LocalDirectoryInfo info = new LocalDirectoryInfo(root, path);
            List<IUnitInfo> units = info.GetUnits().ToList();
            Assert.AreEqual(3, units.Count);
            Assert.AreEqual("1", units[0].Name);
            Assert.AreEqual("2", units[1].Name);
            Assert.AreEqual("subfolder file.txt", units[2].Name);
        }

        [Test]
        public void TestGetFiles()
        {
            LocalFileSystemRoot root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            string path = TestHelper.GetTestDataPath("subfolder");
            LocalDirectoryInfo info = new LocalDirectoryInfo(root, path);
            List<IFileInfo> files = info.GetFiles().ToList();
            Assert.AreEqual(1, files.Count);
            Assert.AreEqual("subfolder file.txt", files[0].Name);
        }
    }
}
