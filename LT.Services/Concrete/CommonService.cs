using LT.Data;
using LT.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace LT.Services.Concrete
{
    public class CommonService : ICommonService
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public readonly dbContext _context;
        public CommonService(IHostingEnvironment hostingEnvironment, IConfiguration configuration, dbContext context)
        {
            this._hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            this._context = context;
        }

        public string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        private string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }
        private string GetPathWithFolderName(string imagePath)
        {
            string contentPath = _hostingEnvironment.WebRootPath;
            string pathWithFolderName = Path.Combine(contentPath, imagePath);

            //Check if directory exist
            if (!Directory.Exists(pathWithFolderName))
            {
                Directory.CreateDirectory(pathWithFolderName); //Create directory if it doesn't exist
            }
            return pathWithFolderName;
        }
        public string SaveIamge(string imagePath, IFormFile formFile)
        {
            Guid guid = Guid.NewGuid();
            string imageName = string.Empty;
            if (formFile != null)
            {
                var lastPart = GetFileExtensionFromMimeType(formFile.ContentType);
                if (lastPart.ToLower() == "jpg" || lastPart.ToLower() == "png" || lastPart.ToLower() == "svg" || lastPart.ToLower() == "jpeg")
                {
                    var uploadFileName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf('.'));

                    if (string.IsNullOrEmpty(lastPart))
                        lastPart = formFile.FileName.Substring(formFile.FileName.LastIndexOf('.') + 1);
                    imageName = guid.ToString() + "." + lastPart;
                    string filePath = Path.Combine(GetPathWithFolderName(imagePath), imageName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }
                    FileInfo fileInfo = new FileInfo(filePath);
                    double fileSize = fileInfo.Length * 1.00 / (1024 * 1024);
                    if (fileSize > 2)
                    {
                        File.Delete(filePath);
                        imageName = "filesize";
                    }
                }
                else
                {
                    imageName = "error";
                }
            }
            return imageName;
        }
    }
}
