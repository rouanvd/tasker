using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using SLib.Data;
using SLib.Prelude;

namespace SLib.FileSystem.Temp
{
    /// <summary>
    ///   Represents a temporary directory inside the WorkingDir.
    /// </summary>
    public class TempDir
    {
        readonly string _basePath;
        readonly string _dirName;
        readonly List<TempFile> _files;
        
        
        public TempDir(string basePath, string dirName)
        {
            Contract.Requires( ! string.IsNullOrWhiteSpace( basePath ) );
            Contract.Requires( ! string.IsNullOrWhiteSpace( dirName ) );

            if (PathModule.IsPath( dirName ))
                throw new ArgumentException( "dirName is not allowed to be a path.", nameof( dirName ) );

            _basePath = basePath;
            _dirName  = dirName;
        
            _files = new List<TempFile>();            
        }

        
        public string DirPath => Path.Combine( _basePath, _dirName );
        public bool IsInitialized => ! string.IsNullOrWhiteSpace( _dirName );
        public List<TempFile> TempFiles => _files.Where(f => ! f.IsPermanent).ToList();
        public List<TempFile> PermaFiles => _files.Where(f => f.IsPermanent).ToList();
        
        
        /// <summary>
        ///   Creates the TempDir on the file system if it does not already exist.
        /// </summary>
        public void Create()
        {
            if (! Directory.Exists( DirPath ))
                Directory.CreateDirectory( DirPath );
        }
    
    
        /// <summary>
        ///   Creates a new TempDir object, relative to this TempDir, using the supplied dirNameTemplate.
        ///   NOTE: This does not actually create the directory on the file system.
        /// </summary>
        /// <param name="dirNameTemplate">
        ///   A template string that is used to determine the name of the new TempDir.
        ///   It can have 1 of 3 possible values:
        ///     (1.) Empty           - a fully random name will be generated that is unique within the TempDir.
        ///     (2.) "temp_dir_{0}"  - a semi random name will be generated, replacing the {0} placeholder with some random, unique value.
        ///     (3.) "my_temp_dir"   - an exact, non-random name that will always be the same.
        /// </param>
        public TempDir NewTempDir(string dirNameTemplate = "")
        {
            string name = TempUtils.GenerateFsEntryName( DirPath, dirNameTemplate );
            var tempDir = new TempDir( DirPath, name );
            return tempDir;
        }

    
        /// <summary>
        ///   Creates a new TempFile object, relative to this TempDir, using the supplied fileNameTemplate, and registers
        ///   the new TempFile with this TempDir, so that we can later delete the registered TempFiles using the
        ///   ClearTempFiles() method.
        ///
        ///   NOTE: This does NOT actually create the file on the file system.
        /// </summary>
        /// <param name="fileNameTemplate">
        ///   A template string that is used to determine the name of the new TempFile.
        ///   It can have 1 of 3 possible values:
        ///     (1.) Empty                - a fully random name will be generated that is unique within the TempDir.
        ///     (2.) "temp_file_{0}.tmp"  - a semi random name will be generated, replacing the {0} placeholder with some random, unique value.
        ///     (3.) "my_temp_file"       - an exact, non-random name that will always be the same.
        /// </param>
        public TempFile NewTempFile(string fileNameTemplate = "")
        {
            string fileName = TempUtils.GenerateFsEntryName( DirPath, fileNameTemplate );
            var tempFile = new TempFile( this, fileName );
        
            // keep track of temp files so that we can selectively clean them up later
            _files.Add( tempFile );
        
            return tempFile;
        }
    
    
        /// <summary>
        ///   Creates a new TempFile object, relative to this TempDir, using the supplied fileNameTemplate.  Files created
        ///   with this method will NOT be deleted when calling the ClearTempFiles() method.
        ///
        ///   NOTE: This does NOT actually create the file on the file system.
        /// </summary>
        /// <param name="fileNameTemplate">
        ///   A template string that is used to determine the name of the new TempFile.
        ///   It can have 1 of 3 possible values:
        ///     (1.) Empty                - a fully random name will be generated that is unique within the TempDir.
        ///     (2.) "temp_file_{0}.tmp"  - a semi random name will be generated, replacing the {0} placeholder with some random, unique value.
        ///     (3.) "my_temp_file"       - an exact, non-random name that will always be the same.
        /// </param>
        public TempFile NewPermaFile(string fileNameTemplate = "")
        {
            string fileName = TempUtils.GenerateFsEntryName( DirPath, fileNameTemplate );
            var permaFile = new TempFile( this, fileName );
            permaFile.MakePermanent();
            
            // keep track of temp files so that we can selectively clean them up later
            _files.Add( permaFile );            
            
            return permaFile;
        }
    
    
        /// <summary>
        ///   Deletes all the files in this TempDir (even untracked files).
        /// </summary>
        public void ClearAllFiles()
        {
            if (! IsInitialized)
                throw new Exception( "TempDir not initialized.  The DirName property cannot have an empty value." );

            foreach (string filePath in Directory.GetFiles( DirPath ))
            {
                File.Delete( filePath );
            }
            
            _files.Clear();
        }
        
        
        /// <summary>
        ///   Deletes only TEMP files in this TempDir that is not marked as PERMANENT.
        /// </summary>
        public void ClearTempFiles()
        {
            foreach (TempFile tempFile in TempFiles)
                tempFile.Delete();
        }
        
        
        /// <summary>
        ///   Iterates over the collection, creating a unique TempFile for each element, and applies the actionF function 
        ///   to each element and TempFile combination.
        ///
        ///   This captures the idea that for each element in a collection, we need to create a TempFile, and we will
        ///   write some data to the TempFile using the element as input.
        /// </summary>
        public List<TempFile> ForEachWithTempFile<T>(IEnumerable<T> coll, Func<T,string> fileNameTemplateF, Action<T, TempFile> actionF)
        {
            if (coll == null || ! coll.Any())
                return new List<TempFile>();
        
            int idx = 0;
            var tempFiles = new List<TempFile>();
            foreach (T elem in coll)
            {
                string filename   = fileNameTemplateF( elem )?.Replace("{0}", idx.ToString());
                TempFile tempFile = NewTempFile( filename );
                tempFiles.Add( tempFile );
            
                actionF( elem, tempFile );
            
                ++idx;
            }

            return tempFiles;
        }
    
    
        /// <summary>
        ///   Iterates over the collection, creating a unique PERMANENT TempFile for each element, and applies the actionF 
        ///   function to each element and TempFile combination.
        ///
        ///   This captures the idea that for each element in a collection, we need to create a TempFile, and we will
        ///   write some data to the TempFile using the element as input.
        /// </summary>
        public List<TempFile> ForEachWithPermaFile<T>(IEnumerable<T> coll, Func<T,string> fileNameTemplateF, Action<T, TempFile> actionF)
        {
            if (coll == null || ! coll.Any())
                return new List<TempFile>();
        
            int idx = 0;
            var tempFiles = new List<TempFile>();
            foreach (T elem in coll)
            {
                string filename   = fileNameTemplateF( elem )?.Replace("{0}", idx.ToString());
                TempFile tempFile = NewPermaFile( filename );
                tempFiles.Add( tempFile );
            
                actionF( elem, tempFile );
            
                ++idx;
            }

            return tempFiles;
        }
        
        
        /// <summary>
        /// </summary>
        public void ForEachWithOverwritingTempFile<T>(IEnumerable<T> coll, string fileNamePrefix, Action<T, TempFile, TempFile> actionF)
        {
            if (coll == null || ! coll.Any())
                return;
        
            string permFilename = fileNamePrefix;
            string tempFilename = fileNamePrefix + ".tmp";
                
            foreach (T elem in coll)
            {
                TempFile permFile = NewPermaFile( permFilename );
                TempFile tempFile = NewTempFile( tempFilename );
            
                try
                {
                    actionF( elem, permFile, tempFile );
                }
                finally
                {
                    if (File.Exists( tempFile.FilePath ))
                        tempFile.MoveTo( permFile );
                }
            }
        }


        public TempFile AggregateFilesInto(IEnumerable<TempFile> files, TempFile accumulatorFile, Action<TempFile, IEnumerable<TempFile>> combineF)
        {
            if (files.IsEmpty())
            {
                accumulatorFile.CreateIfNotExists();
                return accumulatorFile;                
            }

            if (files.Count() == 1)
            {
                files.First().CopyTo( accumulatorFile );
                return accumulatorFile;
            }

            combineF( accumulatorFile, files );
            return accumulatorFile;
        }


        /// <summary>
        ///   Aggregates all the supplied FILES into a separate PERMA file using the supplied combineF action to merge all
        ///   the individual files into 1 combined output file.
        /// </summary>
        /// <param name="files">The sequence of files to combine into 1 file.</param>
        /// <param name="resultFileNameTemplate">The template string to use for the name of the combined file.</param>
        /// <param name="combineF">
        ///   Action to combine 2 files into 1.   
        ///   combineF :: (accumulator: TempFile, inputFile: TempFile) -> unit.
        /// </param>
        /// <returns>A TempFile instance for the new combined file.</returns>
        public TempFile AggregateFilesIntoPerma(IEnumerable<TempFile> files, string resultFileNameTemplate, Action<TempFile, TempFile> combineF)
        {
            TempFile accumulatorFile = NewPermaFile( resultFileNameTemplate );

            if (files.IsEmpty())
            {
                accumulatorFile.CreateIfNotExists();
                return accumulatorFile;                
            }

            if (files.Count() == 1)
            {
                files.First().CopyTo( accumulatorFile );
                return accumulatorFile;
            }

            TempFile firstFile = files.ElementAt(0);
            firstFile.CopyTo( accumulatorFile );

            files.Skip(1)
                 .ToList()
                 .ForEach(file => combineF( accumulatorFile, file ));

            return accumulatorFile;
        }


        public TempFile AggregateFilesIntoPerma(IEnumerable<TempFile> files, string resultFileNameTemplate, Action<TempFile, IEnumerable<TempFile>> combineF)
        {
            TempFile accumulatorFile = NewPermaFile( resultFileNameTemplate );

            if (files.IsEmpty())
            {
                accumulatorFile.CreateIfNotExists();
                return accumulatorFile;                
            }

            if (files.Count() == 1)
            {
                files.First().CopyTo( accumulatorFile );
                return accumulatorFile;
            }

            combineF( accumulatorFile, files );
            return accumulatorFile;
        }
        
    
        /// <summary>
        ///   Aggregates all the supplied FILES into a separate TEMP file using the supplied combineF action to merge all
        ///   the individual files into 1 combined output file.
        /// </summary>
        /// <param name="files">The sequence of files to combine into 1 file.</param>
        /// <param name="resultFileNameTemplate">The template string to use for the name of the combined file.</param>
        /// <param name="combineF">
        ///   Action to combine 2 files into 1.   
        ///   combineF :: (accumulator: TempFile, inputFile: TempFile) -> unit.
        /// </param>
        /// <returns>A TempFile instance for the new combined file.</returns>
        public TempFile AggregateFilesIntoTemp(IEnumerable<TempFile> files, string resultFileNameTemplate, Action<TempFile, TempFile> combineF)
        {
            TempFile accumulatorFile = NewTempFile( resultFileNameTemplate );

            if (files.IsEmpty())
            {
                accumulatorFile.CreateIfNotExists();
                return accumulatorFile;                
            }

            if (files.Count() == 1)
            {
                files.First().CopyTo( accumulatorFile );
                return accumulatorFile;
            }

            TempFile firstFile = files.ElementAt(0);
            firstFile.CopyTo( accumulatorFile );

            files.Skip(1)
                 .ToList()
                 .ForEach(file => combineF( accumulatorFile, file ));

            return accumulatorFile;
        }


        /// <summary>
        ///   Aggregates all the supplied FILES into THE FIRST file (marking it as a PERMA file) using the supplied combineF 
        ///   action to merge all the individual files into 1 combined output file.  
        /// </summary>
        /// <remarks>
        ///   The first file is renamed to the resultFileNameTemplate, marked as a PERMA file and updated in place on the 
        ///   file system.  This is more efficient because we do not make a copy of the first file, we rename the first file and
        ///   UPDATE IT IN PLACE.  This results in less file IO, which improves performance for large files.
        /// </remarks>
        /// <param name="files">The sequence of files to combine into 1 file.</param>
        /// <param name="resultFileNameTemplate">The template string to use for the name of the combined file.</param>
        /// <param name="combineF">
        ///   Action to combine 2 files into 1.   
        ///   combineF :: (accumulator: TempFile, inputFile: TempFile) -> unit.
        /// </param>
        /// <returns>A TempFile instance for the new combined file.</returns>
        public TempFile AggregateFilesIntoFirstAsPerma(IEnumerable<TempFile> files, string resultFileNameTemplate, Action<TempFile, TempFile> combineF)
        {
            if (files.IsEmpty())
            {
                TempFile accumulatorFile = NewPermaFile( resultFileNameTemplate );
                accumulatorFile.CreateIfNotExists();
                return accumulatorFile;                
            }

            if (files.Count() == 1)
            {
                TempFile accumulatorFile = files.Single();
                accumulatorFile.Rename( resultFileNameTemplate );
                accumulatorFile.MakePermanent();

                return accumulatorFile;
            }

            {
                TempFile accumulatorFile = files.First();
                accumulatorFile.Rename( resultFileNameTemplate );
                accumulatorFile.MakePermanent();

                files.Skip(1).Each(file => combineF( accumulatorFile, file ));

                return accumulatorFile;
            }
        }


        /// <summary>
        ///   Aggregates all the supplied FILES into THE FIRST file (marking it as a TEMP file) using the supplied combineF 
        ///   action to merge all the individual files into 1 combined output file.  
        /// </summary>
        /// <remarks>
        ///   The first file is renamed to the resultFileNameTemplate, marked as a TEMP file and updated in place on the 
        ///   file system.  This is more efficient because we do not make a copy of the first file, we rename the first file and
        ///   UPDATE IT IN PLACE.  This results in less file IO, which improves performance for large files.
        /// </remarks>
        /// <param name="files">The sequence of files to combine into 1 file.</param>
        /// <param name="resultFileNameTemplate">The template string to use for the name of the combined file.</param>
        /// <param name="combineF">
        ///   Action to combine 2 files into 1.   
        ///   combineF :: (accumulator: TempFile, inputFile: TempFile) -> unit.
        /// </param>
        /// <returns>A TempFile instance for the new combined file.</returns>
        public TempFile AggregateFilesIntoFirstAsTemp(IEnumerable<TempFile> files, string resultFileNameTemplate, Action<TempFile, TempFile> combineF)
        {
            if (files.IsEmpty())
            {
                TempFile accumulatorFile = NewTempFile( resultFileNameTemplate );
                accumulatorFile.CreateIfNotExists();
                return accumulatorFile;                
            }

            if (files.Count() == 1)
            {
                TempFile accumulatorFile = files.Single();
                accumulatorFile.Rename( resultFileNameTemplate );
                accumulatorFile.MakeTemporary();

                return accumulatorFile;
            }

            {
                TempFile accumulatorFile = files.First();
                accumulatorFile.Rename( resultFileNameTemplate );
                accumulatorFile.MakeTemporary();

                files.Skip(1).Each(file => combineF( accumulatorFile, file ));

                return accumulatorFile;
            }
        }


        public TempFile AggregateFilesIntoFirstAsTemp(IEnumerable<TempFile> files, string resultFileNameTemplate, Action<TempFile, IEnumerable<TempFile>> combineF)
        {
            if (files.IsEmpty())
            {
                TempFile accumulatorFile = NewTempFile( resultFileNameTemplate );
                accumulatorFile.CreateIfNotExists();
                return accumulatorFile;                
            }

            if (files.Count() == 1)
            {
                TempFile accumulatorFile = files.Single();
                accumulatorFile.Rename( resultFileNameTemplate );
                accumulatorFile.MakeTemporary();
                return accumulatorFile;
            }

            {
                TempFile accumulatorFile = NewTempFile( resultFileNameTemplate );
                combineF( accumulatorFile, files );
                return accumulatorFile;
            }
        }
        
        
        /// <summary>
        ///   Creates 1 perma file and 1 temp file, using the fileNameTemplate, and applies an action to the
        ///   both TempFile objects, overwriting the perma file with the temp file.
        /// </summary>
        public void WithOverwritingTempFile(string fileNameTemplate, Action<TempFile, TempFile> actionF)
        {
            string permFilename = fileNameTemplate;
            string tempFilename = fileNameTemplate + ".tmp";
        
            TempFile permFile = NewPermaFile( permFilename );
            TempFile tempFile = NewTempFile( tempFilename );
        
            try
            {
                actionF( permFile, tempFile );
            }
            finally
            {
                if (File.Exists( tempFile.FilePath ))
                    tempFile.MoveTo( permFile );
            }
        }    
    

        public void DeleteFile(string fileName)
        {
            var file = new TempFile( this, fileName );
            file.Delete();
        }        
    }
}
