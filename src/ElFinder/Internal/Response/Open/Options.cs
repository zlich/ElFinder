using System.Runtime.Serialization;
using System.Collections.Generic;

namespace ElFinder
{
    [DataContract]
    internal class Options
    {
        private static string[] _disabled = new string[] { "extract", "create" };
        private static Archive _emptyArchives = new Archive();

        [DataMember(Name = "copyOverwrite")]
        public byte IsCopyOverwrite { get { return 1; } }        

        [DataMember(Name = "separator")]
        public char Separator { get { return '/'; } }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "tmbUrl")]
        public string ThumbnailsUrl { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "archivers")]
        public Archive Archivers { get { return _emptyArchives; } }

        [DataMember(Name = "disabled")]
        public IEnumerable<string> Disabled { get { return _disabled; } }

        public Options(IDirectoryInfo directory)
        {
            Path = directory.Root.Alias;
            if (directory.RelativePath != string.Empty)
                Path += Separator + directory.RelativePath.Replace('\\', Separator);
            Url = directory.Root.Url ?? string.Empty;
            ThumbnailsUrl = directory.Root.ThumbnailsManager.Url ?? string.Empty;
        }
    }
}