// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using SkiaSharp;
using Skitana.Renderer.Abstractions.Text;

namespace Skitana.Renderer.Skia.Text
{
    internal class SkiaFont : IFont
    {
        public SKFont Font { get; }
        public SKPaint Paint { get; }

        public float Size => Font.Size;
        public float BaseLine => -Font.Metrics.Top;
        public float LineHeight => Font.Metrics.Descent - Font.Metrics.Top;

        public IFontFace FontFace { get; }

        public SkiaFont(CreateFontOptions fontOptions)
        {
            var fontFace = fontOptions.FontFace as SkiaFontFace;
            var typeface = fontFace?.SKTypeface;

            if (fontFace == null)
            {
                fontFace = new SkiaFontFace(SKTypeface.Default);
            }

            Font = new SKFont(fontFace.SKTypeface, fontOptions.FontSize);
            Paint = new SKPaint(Font);
            FontFace = fontFace;
        }

        public float GetTextWidth(string text) => Paint.MeasureText(text);

        public void Dispose()
        {
            Font.Dispose();
            Paint.Dispose();
        }
    }
}
