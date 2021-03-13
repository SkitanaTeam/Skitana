// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using SkiaSharp;
using Skitana.Renderer.Abstractions;
using Skitana.Renderer.Abstractions.Text;
using Skitana.Renderer.Skia.Text;
using System;
using System.Drawing;
using System.Numerics;

namespace Skitana.Renderer.Skia
{
    internal class SkiaCanvas : ICanvas
    {
        private SKPoint[][] helperPoints = new SKPoint[20][];
        private SKCanvas skCanvas;

        private SKPaint skPaint = new SKPaint
        {
            IsAntialias = true
        };

        public SkiaCanvas()
        {
            for (var idx = 0; idx < helperPoints.Length; ++idx)
            {
                helperPoints[idx] = new SKPoint[idx];
            }
        }

        public void BeginDraw(object platformCanvas)
        {
            skCanvas = (SKCanvas)platformCanvas;
            skCanvas.ResetMatrix();
            skCanvas.Save();
        }

        public void EndDraw()
        {
            while (skCanvas.SaveCount > 1)
            {
                skCanvas.Restore();
            }
            skCanvas = null;
        }

        public void Clear(Color color)
        {
            skCanvas.Clear(color.ToSKColor());
        }

        public void FillRect(RectangleF rect, Color color)
        {
            skPaint.IsStroke = false;
            skPaint.Color = color.ToSKColor();
            skCanvas.DrawRect(rect.ToSKRect(), skPaint);
        }

        public void DrawRect(RectangleF rect, Color color, float lineThickness)
        {
            skPaint.IsStroke = true;
            skPaint.Color = color.ToSKColor();
            skPaint.StrokeWidth = lineThickness;
            skCanvas.DrawRect(rect.ToSKRect(), skPaint);
        }

        public void DrawLine(Vector2 p1, Vector2 p2, Color color, float lineThickness)
        {
            skPaint.IsStroke = true;
            skPaint.Color = color.ToSKColor();
            skPaint.StrokeWidth = lineThickness;
            skCanvas.DrawLine(p1.ToSKPoint(), p2.ToSKPoint(), skPaint);
        }

        public void FillPolygon(Color color, params Vector2[] points)
        {
            var count = points.Length;
            var targetPoints = helperPoints[count];

            for (var idx = 0; idx < count; ++idx)
            {
                targetPoints[idx] = points[idx].ToSKPoint();
            }

            skPaint.IsStroke = false;
            skPaint.Color = color.ToSKColor();

            skCanvas.DrawPoints(SKPointMode.Polygon, targetPoints, skPaint);
        }

        public void DrawText(IFont font, Vector2 position, string text, Color color, TextAlign align)
        {
            var skiaFont = (SkiaFont)font;

            var paint = skiaFont.Paint;
            paint.Color = color.ToSKColor();

            switch (align)
            {
                case TextAlign.Left:
                    paint.TextAlign = SKTextAlign.Left;
                    break;

                case TextAlign.Right:
                    paint.TextAlign = SKTextAlign.Right;
                    break;

                case TextAlign.Center:
                    paint.TextAlign = SKTextAlign.Center;
                    break;

                default:
                    paint.TextAlign = SKTextAlign.Left;
                    break;
            }

            skCanvas.DrawText(text, position.ToSKPoint(), paint);
        }

        public void DrawText(IFont font, Rectangle target, string text, Color color, TextAlign align)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(IImage image, Vector2 position, Vector2? scale = null, Color? tint = null)
        {
            var targetScale = scale ?? Vector2.One;
            var targetTint = tint ?? Color.White;

            var skImage = (SkiaImage)image;

            skPaint.Color = targetTint.ToSKColor();

            skCanvas.DrawImage(skImage.SKImage,
                new SKRect(0, 0, skImage.Size.Width, skImage.Size.Height),
                new SKRect(position.X, position.Y,
                           position.X + skImage.Size.Width * targetScale.X,
                           position.Y + skImage.Size.Height * targetScale.Y), skPaint);
        }

        public void DrawImage(IImage image, Rectangle target, Rectangle? source = null, Color? tint = null)
        {
            var targetTint = tint.HasValue ? tint.Value : Color.White;
            var skImage = (SkiaImage)image;

            var targetSource = source ?? new Rectangle(0, 0, image.Size.Width, image.Size.Height);

            skPaint.Color = targetTint.ToSKColor();

            skCanvas.DrawImage(skImage.SKImage,
                target.ToSKRect(),
                targetSource.ToSKRect(),
                skPaint);
        }

        public void RotateDegrees(float degrees, Vector2 origin) => skCanvas.RotateDegrees(degrees, origin.X, origin.Y);
        public void RotateRadians(float radians, Vector2 origin) => skCanvas.RotateRadians(radians, origin.X, origin.Y);

        public void Translate(Vector2 offset) => skCanvas.Translate(offset.ToSKPoint());

        public void Scale(Vector2 scale, Vector2 origin) => skCanvas.Scale(scale.X, scale.Y, origin.X, origin.Y);

        public void Clip(RectangleF clip) => skCanvas.ClipRect(clip.ToSKRect(), SKClipOperation.Intersect);

        public void PushState() => skCanvas.Save();
        public void PopState() => skCanvas.Restore();
        public void Dispose() => skPaint.Dispose();
    }
}
