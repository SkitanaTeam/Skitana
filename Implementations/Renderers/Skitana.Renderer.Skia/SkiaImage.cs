// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Drawing;
using SkiaSharp;
using System.IO;
using Skitana.Renderer.Abstractions;

namespace Skitana.Renderer.Skia
{
    internal class SkiaImage : IImage
    {
        public Size Size { get; }

        public SKImage SKImage { get; }

        public SkiaImage(Stream stream)
        {
            using (var bitmap = SKBitmap.Decode(stream))
            {
                SKImage = SKImage.FromBitmap(bitmap);
            }
            Size = new Size(SKImage.Width, SKImage.Height);
        }

        public void Dispose() => SKImage.Dispose();
    }
}
