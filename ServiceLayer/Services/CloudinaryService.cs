using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAccessLayer.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ServiceLayer.Abstractions.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        private readonly CloudinaryConfig _config;

        public CloudinaryService(IOptions<CloudinaryConfig> config)
        {
            _config = config.Value;
            var account = new Account(_config.CloudName, _config.ApiKey, _config.ApiSecret);
            cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }
            using (var stream = file.OpenReadStream())
            {
                var uploadParam = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "KycFile",   // Folder trên Cloudinary
                    PublicId = Path.GetFileNameWithoutExtension(file.FileName), // Tên file ko kèm đuôi
                    Overwrite = true,     // Cho phép ghi đè nếu upload lại
                    UseFilename = true,   // Dùng tên file gốc
                    UniqueFilename = false,
                    
                };
                var uploadResult = await cloudinary.UploadAsync(uploadParam);
                return uploadResult.SecureUrl.ToString();
            }
        }
    }
}
