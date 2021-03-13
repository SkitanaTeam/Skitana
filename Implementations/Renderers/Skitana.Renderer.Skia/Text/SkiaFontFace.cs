// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using SkiaSharp;
using Skitana.Renderer.Abstractions.Text;
using System.IO;
using System.Linq;

namespace Skitana.Renderer.Skia.Text
{
    internal class SkiaFontFace : IFontFace
    {
        public string FamilyName => SKTypeface.FamilyName;
        public bool Bold { get; }
        public bool Italic { get; }
        public SKTypeface SKTypeface { get; }

        public SkiaFontFace(CreateFontFaceOptions options)
        {
            if (options.FontFamily?.Contains(",") ?? false)
            {
                var families = options.FontFamily.Split(',').Where(o => !string.IsNullOrWhiteSpace(o));
                var manager = SKFontManager.Default;

                foreach (var f in families)
                {
                    using (var typeface = manager.MatchFamily(f.Trim()))
                    {
                        if (typeface != null)
                        {
                            options.FontFamily = typeface.FamilyName;
                            break;
                        }
                    }
                }

                if (options.FontFamily.Contains(","))
                {
                    options.FontFamily = null;
                }
            }

            SKFontStyle style = SKFontStyle.Normal;

            if (options.Bold && options.Italic) style = SKFontStyle.BoldItalic;
            else if (options.Bold) style = SKFontStyle.Bold;
            else if (options.Italic) style = SKFontStyle.Italic;

            SKTypeface = SKTypeface.FromFamilyName(options.FontFamily, style);

            Bold = SKTypeface.FontStyle.Weight >= SKFontStyle.Bold.Weight;
            Italic = SKTypeface.FontStyle.Slant.HasFlag(SKFontStyleSlant.Italic) || SKTypeface.FontStyle.Slant.HasFlag(SKFontStyleSlant.Oblique);
        }

        public SkiaFontFace(Stream stream)
        {
            SKTypeface = SKTypeface.FromStream(stream);
            Bold = SKTypeface.FontStyle.Weight >= SKFontStyle.Bold.Weight;
            Italic = SKTypeface.FontStyle.Slant.HasFlag(SKFontStyleSlant.Italic) || SKTypeface.FontStyle.Slant.HasFlag(SKFontStyleSlant.Oblique);
        }

        public SkiaFontFace(SKTypeface typeface)
        {
            SKTypeface = typeface;
            Bold = SKTypeface.FontStyle.Weight >= SKFontStyle.Bold.Weight;
            Italic = SKTypeface.FontStyle.Slant.HasFlag(SKFontStyleSlant.Italic) || SKTypeface.FontStyle.Slant.HasFlag(SKFontStyleSlant.Oblique);
        }
        public void Dispose() => SKTypeface.Dispose();
    }
}
