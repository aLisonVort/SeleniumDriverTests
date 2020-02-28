using System;
using System.IO;

namespace SeleniumDriverTests
{
    class DownloadedFiles
    {
        FileInfo thisFile;
        public bool CheckFileDownloaded(string fileName)
        {
            bool exist = false;
            //string Path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Downloads";
            string Path = @"C:\Users\alisa.voronych\Downloads";
            string[] filePaths = Directory.GetFiles(Path);
            foreach (string p in filePaths)
            {
                if (p.Contains(fileName))
                {
                    thisFile = new FileInfo(p);

                    if (thisFile.LastWriteTime.ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(1).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(2).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                    thisFile.LastWriteTime.AddMinutes(3).ToShortTimeString() == DateTime.Now.ToShortTimeString())
                    {
                        exist = true;
                        Console.WriteLine($"Existimg of file: ){ exist }");
                        File.Delete(p);
                    }   
                    break;
                }
            }
            return exist;

        }
    }
}
