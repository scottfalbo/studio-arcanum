using Arcanum.Data;
using Arcanum.Models;
using Arcanum.Models.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Threading.Tasks;

namespace Arcanum.ImageBlob.Interfaces.Services
{
    public class UploadService : IUpload
    {
        public IConfiguration _config { get; }

        public UploadService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Resizes the gallery image and creates a second thumbnail file.
        /// Both images are uploaded to the blob and the returned Uri assigned to the object.
        /// </summary>
        /// <param name="file"> IFormFile from form input </param>
        /// <returns> new Image object </returns>
        public async Task<Models.Image> AddImage(IFormFile file)
        {
            Stream stream = ResizeImage(file, 1900);
            BlobClient blob = await UploadImage(stream, file.FileName, file.ContentType);

            string thumbFile = ThumbNailFileName(file.FileName);

            Stream thumbStream = ResizeImage(file, 100);
            BlobClient thumb = await UploadImage(thumbStream, thumbFile, file.ContentType);

            Models.Image image = new Models.Image()
            {
                SourceUrl = blob.Uri.ToString(),
                FileName = file.FileName,
                ThumbnailUrl = thumb.Uri.ToString(),
                ThumbFileName = thumbFile,
                Order = 0
            };
            return image;
        }

        /// <summary>
        /// Uploads the image to azure blob storage
        /// </summary>
        /// <param name="file"> file to upload </param>
        /// <returns> new BlobClient object </returns>
        private async Task<BlobClient> UploadImage(Stream stream, string filename, string contentType)
        {
            filename = AugmentFileName(filename);
            BlobContainerClient container = new BlobContainerClient(_config["StorageBlob:ConnectionString"], "images");

            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(filename);

            BlobUploadOptions options = new BlobUploadOptions()
            {
                HttpHeaders = new BlobHttpHeaders() { ContentType = contentType }
            };

            if (!blob.Exists())
                await blob.UploadAsync(stream, options);
            return blob;
        }
      
        /// <summary>
        /// Helper method that adds the time and date to the end of the filename to ensure it is unique.
        /// </summary>
        /// <param name="file"> string filename </param>
        /// <returns> augmented filename </returns>
        private string AugmentFileName(string file)
        {
            string timeStamp = DateTime.Now.ToString();
            timeStamp = Regex.Replace(timeStamp, "[^0-9]", "");

            string pattern = @"[^.]+$";
            string fileType = Regex.Match(file, pattern).ToString();

            file = Regex.Replace(file, $@"\b.{fileType}\b", "");
            file = file.Replace(" ", String.Empty);
            return file + $"{timeStamp}.{fileType}";
        }

        /// <summary>
        /// Helper method to insert "_thumb" before the file extension
        /// </summary>
        /// <param name="file"> string filename </param>
        /// <returns> string filename + _thumb </returns>
        private string ThumbNailFileName(string file)
        {
            string pattern = @"[^.]+$";
            string fileType = Regex.Match(file, pattern).ToString();
            string thumb = Regex.Replace(file, $@"\b.{fileType}\b", "");
            return $"{thumb}_thumb.{fileType}";
        }

        /// <summary>
        /// Create Image object from the upload file.
        /// Resize for max gallery height or thumbnail based on n parameter.
        /// Save the updated Image to a Stream for upload to blob.
        /// </summary>
        /// <param name="file"> IFormFile from form </param>
        /// <param name="n"> height </param>
        /// <returns> Steam of resized Image </returns>
        private Stream ResizeImage(IFormFile file, int n)
        {
            using var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());
            var stream = new MemoryStream();

            int width = FindWidth(image.Width, image.Height, n);
            image.Mutate(x => x.Resize(width, n));

            switch (file.ContentType)
            {
                case "image/jpeg":
                    image.SaveAsJpeg(stream);
                    break;
                case "image/png":
                    image.SaveAsPng(stream);
                    break;
                case "image/bmp":
                    image.SaveAsBmp(stream);
                    break;
                case "image/gif":
                    image.SaveAsGif(stream);
                    break;
                default:
                    throw new Exception("invalid file type");
            }
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Helper function to find width relative to new height.
        /// </summary>
        /// <param name="width"> image width </param>
        /// <param name="height"> image height </param>
        /// <param name="n"> new height </param>
        /// <returns> int new width </returns>
        private int FindWidth(int width, int height, int n)
        {
            float ratio = (float)height / (float)n;
            int newWidth = Convert.ToInt32((float)width / ratio);
            return newWidth;
        }

    }
}
