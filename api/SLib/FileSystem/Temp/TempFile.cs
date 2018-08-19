using System;
using System.IO;

namespace SLib.FileSystem.Temp
{
    /// <summary>
    ///   Represents a temporary file inside a TempDir or the WorkingDir.
    /// </summary>
    public class TempFile
    {
        public static TempFile NewFrom(TempFile fromTempFile, string fileName = "")
        {
            var newTempFile = new TempFile( fromTempFile.BasePath, fileName );
            newTempFile._isPermanent = fromTempFile.IsPermanent;
            
            return newTempFile;
        }


        readonly TempDir _baseDir;
        string _fileName;
        bool _isPermanent = false;
    
    
        public TempFile(TempDir baseDir, string fileName = "")
        {
            if (PathModule.IsPath( fileName ))
                throw new ArgumentException( "fileName is not allowed to be a path.", nameof( fileName ) );

            _baseDir = baseDir;
            _fileName = fileName;
        }
    
    
        public TempDir BasePath => _baseDir;
        public string FilePath => Path.Combine( _baseDir.DirPath, _fileName );
        public string FileName => _fileName;
        public bool IsPermanent => _isPermanent;

        
        public void MakePermanent()
        {
            _isPermanent = true;
        }
        
        
        public void MakeTemporary()
        {
            _isPermanent = false;
        }
                

        public void CreateIfNotExists()
        {
            if (! File.Exists( FilePath ))
            {
                var fs = File.Create( FilePath );
                fs.Close();
                fs.Dispose();
            }
        }


        public void Rename(string fileNameTemplate = "")
        {
            string newFileName = TempUtils.GenerateFsEntryName( _baseDir.DirPath, fileNameTemplate );
            TempFile toFile = TempFile.NewFrom( this, newFileName );
            
            if (File.Exists( FilePath ))
            {
                if (File.Exists( toFile.FilePath ))
                  File.Delete( toFile.FilePath );
                
                File.Move( FilePath, toFile.FilePath );
            }
            
            _fileName = newFileName;
        }


        public void Delete()
        {
            if (File.Exists( FilePath ))
                File.Delete( FilePath );
        }
    
    
        public void MoveTo(TempFile toFile)
        {
            toFile.Delete();
            File.Move( FilePath, toFile.FilePath );
        }


        public void CopyTo(TempFile toFile)
        {
            toFile.Delete();
            File.Copy( FilePath, toFile.FilePath );
        }


        public byte[] ReadAllBytes()
        {
            if (! File.Exists( FilePath ))
                return new byte[0];

            byte[] fileContent = File.ReadAllBytes( FilePath );
            return fileContent;
        }


        public bool Exists()
        {
            bool fileExists = File.Exists( FilePath );
            return fileExists;
        }
    }    
}
