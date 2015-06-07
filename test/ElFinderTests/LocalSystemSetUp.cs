using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace ElFinderTests
{
    [SetUpFixture]
    public class LocalSystemSetUp
    {
        [SetUp]
        public void CreateUnits()
        {
            CleanUp();

            DirectoryInfo testDir = new DirectoryInfo(TestHelper.TestDataPath);
            CreateSubfolderFile(testDir, "testText");
            CreateSubfolderFile(testDir, "testText.txt");
            (new FileInfo(TestHelper.GetTestDataPath("testText"))).Attributes |= FileAttributes.Hidden;


            DirectoryInfo subfolder = Directory.CreateDirectory(Path.Combine(testDir.FullName, "subfolder"));
            CreateSubfolderFile(subfolder);

            DirectoryInfo dir1 = Directory.CreateDirectory(Path.Combine(subfolder.FullName, "1"));
            CreateSubfolderFile(dir1);
            dir1.Attributes |= FileAttributes.Hidden;

            DirectoryInfo dir2 = Directory.CreateDirectory(Path.Combine(subfolder.FullName, "2"));
            CreateSubfolderFile(dir2);
        }

        [TearDown]
        public void CleanUp()
        {
            string[] subdirs = new string[] { "subfolder" };
            string[] subfiles = new string[] { "testText", "testText.txt" };
            foreach (string item in subdirs)
            {
                string path = TestHelper.GetTestDataPath(item);
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            foreach (string item in subfiles)
            {
                string path = TestHelper.GetTestDataPath(item);
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        private void CreateSubfolderFile(DirectoryInfo directory)
        {
            CreateSubfolderFile(directory, "subfolder file.txt");
        }

        private void CreateSubfolderFile(DirectoryInfo subfolder, string name)
        {
            using (FileStream file = File.Create(Path.Combine(subfolder.FullName, name)))
            {
                WriteSampleText(file);
            }
        }

        private void WriteSampleText(FileStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            for (int i = 1; i < 6; i++)
            {
                if (i != 1)
                    writer.Write(Environment.NewLine);
                writer.Write(i);
            }
            writer.Flush();
        }
    }
}