using ElFinder;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ElFinderTests
{
    [TestFixture]
    public class DriverTests
    {
        [Test]
        public void TestRoots()
        {
            Driver driver = new Driver();
            Assert.AreEqual(0, driver.Roots.Count);

            driver.AddRoot(new LocalFileSystemRoot(TestHelper.TestDataPath));
            Assert.AreEqual(1, driver.Roots.Count);
            Assert.AreEqual("v1_", driver.Roots[0].VolumeId);

            driver.AddRoot(new LocalFileSystemRoot("C:/"));
            Assert.AreEqual(2, driver.Roots.Count);
            Assert.AreEqual("v2_", driver.Roots[1].VolumeId);
        }

        [Test]
        public void TestOpen()
        {

        }

        [Test]
        public void TestInit()
        {

        }

        [Test]
        public void TestParents()
        {
            Driver driver = new Driver();
            var root = new LocalFileSystemRoot(TestHelper.TestDataPath);
            driver.AddRoot(root);

            UnitDTO subdir = root.Directory.ToDTO();
            TreeResponse response = (TreeResponse)driver.Parents(subdir.Hash);
            Assert.AreEqual(1, response.Tree.Count);

            subdir = root.GetDirectory("subfolder/1").ToDTO();
            response = (TreeResponse)driver.Parents(subdir.Hash);
            Assert.AreEqual(4, response.Tree.Count);
        }
    }
}
