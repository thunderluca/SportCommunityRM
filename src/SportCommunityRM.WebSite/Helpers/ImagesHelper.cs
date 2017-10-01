using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SportCommunityRM.WebSite.Helpers
{
    public static class ImagesHelper
    {
        public const string JpegMimeType = "image/jpeg";
        public const string PngMimeType = "image/png";

        public static byte[] GetImageBytesFromBase64String(string base64Image)
        {
            if (base64Image.Contains(','))
                base64Image = base64Image.Substring(base64Image.IndexOf(',') + 1);
            return Convert.FromBase64String(base64Image);
        }

        public static async Task<byte[]> ResizeImageAsync(byte[] originalBuffer, int size, int quality)
        {
            return await Task.Run(() =>
            {
                using (var image = Image.Load(originalBuffer))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var scaled = ScaledSize(image.Width, image.Height, size);

                        image.Mutate(i => i.Resize(scaled.width, scaled.height));

                        image.SaveAsJpeg(memoryStream);

                        return memoryStream.ToArray();
                    }
                }
            });
        }

        private static (int width, int height) ScaledSize(int originalWidth, int originalHeight, int outputSize)
        {
            var width = originalWidth > originalHeight ? outputSize : (int)Math.Round(originalWidth * outputSize / (double)originalHeight);
            var height = originalWidth > originalHeight ? (int)Math.Round(originalHeight * outputSize / (double)originalWidth) : outputSize;

            return (width, height);
        }
    }
}
