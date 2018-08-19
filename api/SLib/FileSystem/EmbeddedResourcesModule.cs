using System;
using System.IO;
using System.Reflection;

namespace SLib.FileSystem
{
    ///<summary>
    ///  Utility class that enables the easy location and reading of embedded resource text files from an Assembly.
    ///</summary>
    public static class EmbeddedResourcesModule
    {
        public static string GetEmbeddedResourceAsText(Assembly assembly, string resourceName)
        {
            Stream resourceFileStream = GetManifestResourceStream(assembly, resourceName);
            string resourceFileText   = "";

            using (var streamReader = new StreamReader(resourceFileStream))
                resourceFileText = streamReader.ReadToEnd();

            return resourceFileText;
        }


        public static Stream GetEmbeddedResourceAsStream(Assembly assembly, string resourceName)
        {
            Stream resourceFileStream = GetManifestResourceStream(assembly, resourceName);
            return resourceFileStream;
        }


        static Stream GetManifestResourceStream(Assembly assembly, string resourceName)
        {
            Stream resourceStream = assembly.GetManifestResourceStream(resourceName);

            if (resourceStream == null)
            {
                var msg = $"EmbeddedResourceReader: Could not locate embedded resource file: {resourceName}, in assembly: {assembly.FullName}";
                throw new Exception(msg);
            }

            return resourceStream;
        }
    }
}
