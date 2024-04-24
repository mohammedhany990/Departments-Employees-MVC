using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            // 1.Get Located Folder path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

            // 2.Get File Name, Guid adds string in the beginning to make it unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3.Creating File Path
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4.Save File As Streams
            using(var sf = new FileStream(FilePath, FileMode.Create))
                file.CopyTo(sf);
            return FileName;
        }

        public static void DeleteFile(string FileName, string FolderName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);
            if(File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}