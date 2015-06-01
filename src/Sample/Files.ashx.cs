using ElFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample
{
    /// <summary>
    /// Summary description for FilerHandler
    /// </summary>
    public class Files : Connector
    {
        public Files()
        {            
            AddRoot(new LocalFileSystemRoot(@"D:\OpenSource\ElFinder\testData") { Alias = "TestData" });
            AddRoot(new LocalFileSystemRoot("C:/Program Files") { Alias = "System" });
        }
    }
}