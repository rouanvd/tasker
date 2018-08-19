using System.IO;
using SLib.Prelude;

namespace SLib.FileSystem
{
    public static class PathModule
    {
        public static bool IsPath(string str)
        {
            if ( string.IsNullOrWhiteSpace( str ) )
                return false;

            bool strIsPath = str.Contains("\\") || str.Contains("/");
            return strIsPath;
        }


        public static string Combine(string pathLeft, string pathRight)
        {
            if (pathLeft.IsEmpty() && pathRight.IsEmpty())
                return "";

            if (pathLeft.IsEmpty())
                return pathRight;

            if (pathRight.IsEmpty())
                return pathLeft;

            pathLeft  = pathLeft.TrimEnd( Path.DirectorySeparatorChar );
            pathRight = pathRight.TrimStart( Path.DirectorySeparatorChar );
            
            string combinedPath = pathLeft + Path.DirectorySeparatorChar + pathRight;
            return combinedPath;
        }


        public static string GetLastDirectoryName(string path)
        {
            var pathParts = path.Split( Path.DirectorySeparatorChar );
            string lastDirName = pathParts[ pathParts.Length - 1 ];
            return lastDirName;
        }


        public static string GetRelativePathFrom(string fullPath, string basePath)
        {
            string relativePath = fullPath.Substring( basePath.Length );
            if (relativePath.StartsWith( Path.DirectorySeparatorChar.ToString() ))
                relativePath = relativePath.Substring( 1 );

            return relativePath;
        }
    }
}
