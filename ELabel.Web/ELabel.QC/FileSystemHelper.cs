using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC
{
    public class FileSystemHelper
    {
        public IEnumerable<string> GetFilesOnStartup(string directoryToWatch, string fileExtension)
        {
            try
            {
                return Directory.GetFiles(directoryToWatch, fileExtension).Select(p => p);
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("GetFileOnStartUp threw following exception: {0}", ex));
                return Enumerable.Empty<string>();
            }
        }
        public bool MoveToArchive(string pathArchive, string fileName)
        {
            return MoveFileToDirectory(fileName, pathArchive);
        }


        public bool MoveToFailed(string pathFailed, string fileName)
        {
            return MoveFileToDirectory(fileName, pathFailed);
        }
        public bool DeleteFile(string filename)
        {
            bool deleted;

            try
            {
                File.Delete(filename);
                deleted = true;
            }
            catch (Exception ex)
            {
                deleted = false;
            }

            return deleted;
        }
        private bool MoveFileToDirectory(string sourceFile, string directoryPath)
        {
            try
            {
                var fileName = new FileInfo(sourceFile).Name;

                if (!File.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                var destinationFile = new StringBuilder().Append(directoryPath).Append("\\\\").Append(fileName).ToString();

                if (File.Exists(destinationFile)) File.Delete(destinationFile);

                MoveFile(sourceFile, destinationFile);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private void MoveFile(string sourceFile, string destinationFile)
        {
            File.Move(sourceFile, destinationFile);
        }

        public bool IsFileReady(string fileName)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void ArchiveFile(string file, bool success)
        {
            // Move to Archive
            if (success)
            {
                MoveToArchive(ConfigurationManager.AppSettings["CsvArchive"], file);
            }
            // Move to Failed
            else
            {
                MoveToFailed(ConfigurationManager.AppSettings["CsvFailed"], file);
            }
        }

    }
}
