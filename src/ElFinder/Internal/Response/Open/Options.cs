using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ElFinder
{
    [DataContract]
    internal class Options
    {
        [DataMember(Name = "copyOverwrite")]
        public byte IsCopyOverwrite { get { return 1; } }        

        [DataMember(Name = "separator")]
        public char Separator { get { return '/'; } }

        [DataMember(Name = "path")]
        public string Path { get; private set; }

        [DataMember(Name = "tmbUrl")]
        public string ThumbnailsUrl { get; private set; }

        [DataMember(Name = "url")]
        public string Url { get; private set; }

        [DataMember(Name = "archivers")]
        public Archive Archivers { get { return s_emptyArchives; } }

        [DataMember(Name = "disabled")]
        public IEnumerable<string> Disabled { get { return s_disabled; } }

        public Options(IDirectoryInfo directory)
        {
            Contract.Requires(directory != null);
            Path = directory.Root.Alias;
            if (directory.RelativePath != string.Empty)
                Path += Separator + directory.RelativePath.Replace('\\', Separator);
            Url = directory.Root.Url ?? string.Empty;
            ThumbnailsUrl = directory.Root.ThumbnailsManager.Url ?? string.Empty;
        }

        private static string[] s_disabled = new string[] { "extract", "create" };
        private static Archive s_emptyArchives = new Archive();
    }
}