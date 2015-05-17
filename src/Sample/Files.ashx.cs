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
        public override bool IsReusable
        {
            get { return true; }
        }

        public Files()
        {
            Driver.AddRoot(new LocalFileSystemRoot("C:/") { Alias = "System" });
        }
    }
}