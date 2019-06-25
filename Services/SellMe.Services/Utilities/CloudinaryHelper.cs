﻿using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace SellMe.Services.Utilities
{
    public static class CloudinaryHelper
    {
        public const string CloudinaryCloudName = "dqqeaqrwy";
        public const string cloudinarySecret = "dzxgAwwJsWSUrjbO8JbwU3r4V_Y";
        public const string cloudinaryApiKey = "375332952839828";

        public  static Cloudinary SetCloudinary()
        {
            Account account = new Account(CloudinaryCloudName, cloudinaryApiKey, cloudinarySecret);

            Cloudinary cloudinary = new Cloudinary(account);

            return cloudinary;
        }

        public static async Task<string> UploadImage(Cloudinary cloudinary, IFormFile fileForm, string name)
        {
            if (fileForm == null)
            {
                return null;
            }

            byte[] storyImage;

            using (var memoryStream = new MemoryStream())
            {
                await fileForm.CopyToAsync(memoryStream);
                storyImage = memoryStream.ToArray();
            }

            var stream = new MemoryStream(storyImage);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(name, stream),
                Transformation = new Transformation().Width(200).Height(250).Crop("fit").SetHtmlWidth(250).SetHtmlHeight(100)
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            stream.Dispose();
            return uploadResult.SecureUri.AbsoluteUri;
        }
    }
}