using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;

namespace ImageCombine
{
    public class ImageHelper
    {
        /// <summary>
        /// 合并图片(暂时仅实现: 垂直拼接、目标图片宽度采用源图片的最小宽度)
        /// </summary>
        /// <param name="outputFile"></param>
        /// <param name="imageFiles"></param>
        public static void Combin(string outputFile, params string[] imageFiles)
        {
            int targetWidth = 0; // 目标图片宽度
            int targetHeight = 0; // 目标图片宽度

            // 加载图片
            List<Image<Rgba32>> sourceImages = new List<Image<Rgba32>>();
            foreach (string filePath in imageFiles)
            {
                Image<Rgba32> image = Image.Load<Rgba32>(filePath);
                sourceImages.Add(image);
                if (targetWidth > 0)
                {
                    // 选取小的宽度
                    targetWidth = Math.Min(targetWidth, image.Width);
                }
                else
                {
                    // 第一张
                    targetWidth = image.Width;
                }
            }

            // 处理到等宽
            for (int idx = 0; idx < sourceImages.Count; idx++)
            {
                Image<Rgba32> image = sourceImages[idx];
                ResizeToWidth(ref image, targetWidth);
                // 更新目标图片宽度
                targetHeight += image.Height;
            }

            // 合并图片
            using (Image<Rgba32> outputImage = new Image<Rgba32>(targetWidth, targetHeight))
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
                // 保存图片
                outputImage.Save(outputFile);
            }
            foreach (Image<Rgba32> image in sourceImages)
            {
                image.Dispose();
            }
        }

        public static void ResizeToSize(ref Image<Rgba32> image, Size targetSize)
        {
            image.Mutate((operation) => operation.Resize(targetSize, KnownResamplers.Lanczos3, compand: false));
        }

        public static void ResizeToSize(ref Image<Rgba32> image, int targetWidth, int targetHeight)
        {
            image.Mutate((operation) => operation.Resize(targetWidth, targetHeight, KnownResamplers.Lanczos3));
        }

        #region 只提供宽或高, 自动处理宽高比
        // 参考:
        // - https://docs.sixlabors.com/articles/imagesharp/resize.html
        // - https://github.com/SixLabors/ImageSharp/blob/master/src/ImageSharp/Processing/Processors/Transforms/Resize/ResizeHelper.cs#L33
        // 当指定 宽 或 高 中一项(>0)，另一项为0时，另一项会根据原始宽高比自动计算
        /// <summary>
        /// 缩放到指定宽度(自动处理宽高比(aspect ratio))
        /// </summary>
        /// <param name="image"></param>
        /// <param name="targetWidth"></param>
        public static void ResizeToWidth(ref Image<Rgba32> image, int targetWidth)
        {
            ResizeToSize(ref image, targetWidth, 0);
        }

        /// <summary>
        /// 缩放到指定高度(自动处理宽高比(aspect ratio))
        /// </summary>
        /// <param name="image"></param>
        /// <param name="targetHeight"></param>
        public static void ResizeToHeight(ref Image<Rgba32> image, int targetHeight)
        {
            ResizeToSize(ref image, 0, targetHeight);
        }
        #endregion
    }
}
