using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;

namespace ImageCombine
{
    public class ImageHelper
    {
        public static void Combin(string outputFile, params string[] imageFiles)
        {
            List<Image<Rgba32>> sourceImages = new List<Image<Rgba32>>();
            int width = 0;
            int height = 0;
            foreach (string filePath in imageFiles)
            {
                Image<Rgba32> image = Image.Load<Rgba32>(filePath);
                sourceImages.Add(image);
                width = image.Width;
                height += image.Height;
            }
            // TOOD: 处理到统一宽度
            using (Image<Rgba32> outputImage = new Image<Rgba32>(width, height))
            {
                outputImage.Mutate((IImageProcessingContext iipc) =>
                {
                    int drawX = 0;
                    int drawY = 0;
                    foreach (Image<Rgba32> image in sourceImages)
                    {
                        iipc = iipc.DrawImage(image, new Point(drawX, drawY), 1f);
                        drawY += image.Height;
                    }
                });
                outputImage.Save(outputFile);
            }
            foreach (Image<Rgba32> image in sourceImages)
            {
                image.Dispose();
            }
        }
    }
}
