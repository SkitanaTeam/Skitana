// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using SkiaSharp;
using System.Drawing;
using System.Numerics;

namespace Skitana.Renderer.Skia
{
    internal static class ConverterExtensions
    {
        public static SKRect ToSKRect(this Rectangle rect) => new SKRect(rect.X, rect.Y, rect.Right, rect.Bottom);
        public static SKRect ToSKRect(this RectangleF rect) => new SKRect(rect.X, rect.Y, rect.Right, rect.Bottom);
        public static SKColor ToSKColor(this Color color) => new SKColor(color.R, color.G, color.B, color.A);
        public static SKPoint ToSKPoint(this Vector2 pt) => new SKPoint(pt.X, pt.Y);
        public static SKPoint ToSKPoint(this Point pt) => new SKPoint(pt.X, pt.Y);
        public static SKPoint ToSKPoint(this PointF pt) => new SKPoint(pt.X, pt.Y);
    }
}
