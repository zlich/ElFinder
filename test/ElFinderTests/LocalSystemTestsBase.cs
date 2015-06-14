using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace ElFinderTests
{
    public class LocalSystemTestsBase
    {
        [SetUp]
        public void CreateUnits()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            CleanUp();
            //subfolder
            //testPicture.jpg
            //testText  (hidden)
            //testText.txt
            //subfolder/subfolder file.txt
            //subfolder/1 (hidden)
            //subfolder/1/subfolder file.txt
            //subfolder/2
            //subfolder/2/subfolder file.txt


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
            Console.WriteLine("Files prepared: " + stopWatch.ElapsedMilliseconds + "ms");
            stopWatch.Stop();
        }

        [TearDown]
        public void CleanUp()
        {
            foreach (string item in Directory.GetDirectories(TestHelper.TestDataPath))
            {
                    Directory.Delete(item, true);
            }
            foreach (string item in Directory.GetFiles(TestHelper.TestDataPath))
            {
                if(Path.GetFileName(item) != "testPicture.jpg")
                    File.Delete(item);
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