using IHelperServices;
using IHelperServices.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;

namespace HelperServices
{
    public class ImageServices : _HelperService, IImageServices
    {
        public ImageServices()
        {
        }

        public ImageSize GetImageSize(byte[] content)
        {
            using (var ms = new MemoryStream(content))
            {
                var image = Image.FromStream(ms);
                return new ImageSize { Width = image.Width, Height = image.Height };
            }
        }

        public byte[] GetFileBytes(IFormFile img)
        {
            if (img.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    img.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    return fileBytes;
                    //string s = Convert.ToBase64String(fileBytes);
                    // act on the Base64 data
                }
            }
            return null;
        }

        public string GetFileBase64Format(byte[] fileBytes)
        {
            return Convert.ToBase64String(fileBytes);
        }
    }
}