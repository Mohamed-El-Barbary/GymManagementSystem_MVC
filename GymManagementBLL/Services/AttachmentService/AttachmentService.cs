using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long maxFileSize = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment _webHost;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string? Upload(string folderName, IFormFile file)
        {

            try
            {
                if (folderName is null || file is null || file.Length == 0) return null;

                if (file.Length > maxFileSize) return null;

                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(extension)) return null;

                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);

                var fileName = Guid.NewGuid().ToString() + extension;

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);

                file.CopyTo(fileStream);

                return fileName;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File To Folder : {folderName} : {ex}");
                return null;
            }
        }
        public bool Delete(string fileName, string folderName)
        {

            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                    return false;

                var FullPath = Path.Combine(_webHost.WebRootPath, "images" , folderName , fileName);

                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Delete File With Name {fileName} : {ex}");
                return false;
            }

        }
    }
}
