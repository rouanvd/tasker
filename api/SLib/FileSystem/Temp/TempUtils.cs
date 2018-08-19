using System;
using System.IO;

namespace SLib.FileSystem.Temp
{
    public static class TempUtils
    {
        public static string GenerateFsEntryName(string dirPath, string template)
        {
            string name = template;
        
            if (string.IsNullOrWhiteSpace( template ))
                // generate a fully random name
                name = GenerateFullyRandomName( dirPath );
            else if (template.Contains("{0}"))
                // generate a partially random name
                name = GeneratePartiallyRandomName( dirPath, template );
            
            // use the template as the exact name (having no random part in the name)
            return name;
        }
    
    
        static string GenerateFullyRandomName(string dirPath)
        {
            string name = GeneratePartiallyRandomName( dirPath, "{0}.tmp" );
            return name;
        }
    
    
        static string GeneratePartiallyRandomName(string dirPath, string template)
        {
            if (string.IsNullOrEmpty( template ) || ! template.Contains( "{0}" ))
                throw new Exception( "Template string cannot empty and must contain a variable marker ({0})." );
        
            string randomValue = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");

            string searchPattern    = template.Replace( "{0}", randomValue );
            int similarEntriesCount = Directory.GetFileSystemEntries( dirPath, searchPattern, SearchOption.TopDirectoryOnly ).Length;
        
            string name = string.Format( template, randomValue + "_" + similarEntriesCount );
            return name;
        }    
    }
}
