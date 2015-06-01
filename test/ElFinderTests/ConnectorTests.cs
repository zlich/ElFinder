using ElFinder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace ElFinderTests
{
    [TestFixture]
    public class ConnectorTests
    {
        [Test]
        public void TestRoots()
        {
            Connector connector = new Connector();
            Assert.AreEqual(0, connector.Roots.Count);

            connector.AddRoot(new LocalFileSystemRoot(TestHelper.TestDataPath));
            Assert.AreEqual(1, connector.Roots.Count);
            Assert.AreEqual("v1_", connector.Roots[0].VolumeId);

            connector.AddRoot(new LocalFileSystemRoot("C:/"));
            Assert.AreEqual(2, connector.Roots.Count);
            Assert.AreEqual("v2_", connector.Roots[1].VolumeId);
        }

        [Test]
        public void TestOpen()
        {
            Connector connector = CreateTestConnector();
            //check missed parameter
            ErrorResponse error = GetResponse<ErrorResponse>(connector, "cmd=open");

            //check open subfolder
            UnitDTO target = connector.Roots[0].GetDirectory("subfolder").ToDTO();
            OpenResponse response = GetResponse<OpenResponse>(connector, "cmd=open&target=" + target.Hash);
            Assert.AreEqual(target.Hash, response.CurrentWorkingDirectory.Hash);
            Assert.AreEqual(3, response.Files.Count);
        }

        [Test]
        public void TestInit()
        {
            Connector connector = CreateTestConnector();
            InitResponse response = GetResponse<InitResponse>(connector, "cmd=open&init=1");
            Assert.AreEqual(4, response.Files.Count);
        }

        [Test]
        public void TestParents()
        {
            Connector connector = CreateTestConnector();
            //check missed parameter
            ErrorResponse error = GetResponse<ErrorResponse>(connector, "cmd=parents");

            //check parents
            UnitDTO target = connector.Roots[0].GetDirectory("subfolder/2").ToDTO();
            TreeResponse response = GetResponse<TreeResponse>(connector, "cmd=parents&target=" + target.Hash);
            Assert.AreEqual(3, response.Tree.Count);
        }

        [Test]
        public void TestTree()
        {
            Connector connector = CreateTestConnector();
            //check missed parameter
            ErrorResponse error = GetResponse<ErrorResponse>(connector, "cmd=tree");

            UnitDTO target = connector.Roots[0].GetDirectory("").ToDTO();
            TreeResponse response = GetResponse<TreeResponse>(connector, "cmd=tree&target=" + target.Hash);
            Assert.AreEqual(1, response.Tree.Count);
            Assert.AreEqual(1, ((DirectoryDTO)response.Tree[0]).ContainsChildDirs);
        }

        [Test]
        public void TestMakeDir()
        {
            Connector connector = CreateTestConnector();
            //check missed parameter
            ErrorResponse error = GetResponse<ErrorResponse>(connector, "cmd=mkdir");

            UnitDTO target = connector.Roots[0].GetDirectory("subfolder/2").ToDTO();
            error = GetResponse<ErrorResponse>(connector, "cmd=mkdir&target=" + target.Hash);

            AddResponse response = GetResponse<AddResponse>(connector, "cmd=mkdir&target=" + target.Hash + "&name=newDir");
            Assert.AreEqual(1, response.Added.Count);
            Assert.AreEqual("newDir", response.Added[0].Name);


            //clean up
            string path = TestHelper.GetTestDataPath("subfolder/2/newDir");
            Directory.Delete(path);
        }

        [Test]
        public void TestMakeFile()
        {
            Connector connector = CreateTestConnector();
            //check missed parameter
            ErrorResponse error = GetResponse<ErrorResponse>(connector, "cmd=mkfile");

            UnitDTO target = connector.Roots[0].GetDirectory("subfolder/2").ToDTO();
            error = GetResponse<ErrorResponse>(connector, "cmd=mkfile&target=" + target.Hash);

            AddResponse response = GetResponse<AddResponse>(connector, "cmd=mkfile&target=" + target.Hash + "&name=newFile");
            Assert.AreEqual(1, response.Added.Count);
            Assert.AreEqual("newFile", response.Added[0].Name);


            //clean up
            string path = TestHelper.GetTestDataPath("subfolder/2/newFile");
            File.Delete(path);
        }

        [Test]
        public void TestRenameDir()
        {
            Connector connector = CreateTestConnector();
            //check missed parameter
            ErrorResponse error = GetResponse<ErrorResponse>(connector, "cmd=rename");

            UnitDTO target = connector.Roots[0].GetDirectory("subfolder/2").ToDTO();
            error = GetResponse<ErrorResponse>(connector, "cmd=rename&target=" + target.Hash);
            ReplaceResponse response = GetResponse<ReplaceResponse>(connector, "cmd=rename&target=" + target.Hash + "&name=renamed2");
            Assert.AreEqual(1, response.Added.Count);
            Assert.AreEqual(1, response.Removed.Count);
            Assert.AreEqual(target.Hash, response.Removed[0]);
            Assert.AreEqual("renamed2", response.Added[0].Name);

            //revert
            response = GetResponse<ReplaceResponse>(connector, "cmd=rename&target=" + response.Added[0].Hash + "&name=2");
            Assert.AreEqual(1, response.Added.Count);
            Assert.AreEqual(1, response.Removed.Count);
            Assert.AreEqual("2", response.Added[0].Name);
        }

        [Test]
        public void TestRenameFile()
        {
            Connector connector = CreateTestConnector();
            //check missed parameter
            ErrorResponse error = GetResponse<ErrorResponse>(connector, "cmd=rename");

            UnitDTO target = connector.Roots[0].GetFile("subfolder/subfolder file.txt").ToDTO();
            error = GetResponse<ErrorResponse>(connector, "cmd=rename&target=" + target.Hash);
            ReplaceResponse response = GetResponse<ReplaceResponse>(connector, "cmd=rename&target=" + target.Hash + "&name=renamedFile");
            Assert.AreEqual(1, response.Added.Count);
            Assert.AreEqual(1, response.Removed.Count);
            Assert.AreEqual(target.Hash, response.Removed[0]);
            Assert.AreEqual("renamedFile", response.Added[0].Name);

            //revert
            response = GetResponse<ReplaceResponse>(connector, "cmd=rename&target=" + response.Added[0].Hash + "&name=subfolder file.txt");
            Assert.AreEqual(1, response.Added.Count);
            Assert.AreEqual(1, response.Removed.Count);
            Assert.AreEqual("subfolder file.txt", response.Added[0].Name);
        }

        private static T GetResponse<T>(Connector connector, string query) where T : ResponseBase
        {
            using (var writer = new MemoryWriterMock())
            {
                HttpRequest request = new HttpRequest(null, "http://tempuri.org", query);
                HttpContext context = new HttpContext(request, new HttpResponse(writer));
                return (T)connector.GetResponse(context.Request);
            }
        }

        private static Connector CreateTestConnector()
        {
            var connector = new Connector();
            connector.AddRoot(new LocalFileSystemRoot(TestHelper.TestDataPath));
            return connector;
        }
    }
}
