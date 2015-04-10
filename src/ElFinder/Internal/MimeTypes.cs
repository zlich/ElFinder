using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ElFinder
{
    /// <summary>
    /// Repsents container for mime types
    /// </summary>
    internal static class MimeTypes
    {
        public static string GetMimeType(string extension)
        {
            return m_mimeTypes.ContainsKey(extension) ? m_mimeTypes[extension] : "unknown";
        }

        static MimeTypes()
        {
            m_mimeTypes = new Dictionary<string, string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (assembly != null)
            {
                using (Stream stream = assembly.GetManifestResourceStream("Elfinder.mimeTypes.txt"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            line = line.Trim();
                            if (!string.IsNullOrWhiteSpace(line) && line[0] != '#')
                            {
                                string[] parts = line.Split(' ');
                                if (parts.Length > 1)
                                {
                                    string mime = parts[0];
                                    for (int i = 1; i < parts.Length; i++)
                                    {
                                        string ext = parts[i].ToLower();
                                        if (!m_mimeTypes.ContainsKey(ext))
                                        {
                                            m_mimeTypes.Add(ext, mime);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static Dictionary<string, string> m_mimeTypes;
    }
}
