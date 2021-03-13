// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.DependencyInjection.Abstractions;
using Skitana.Renderer.Abstractions.Text;
using System.Collections.Generic;

namespace Skitana.App.Framework.Text
{
    internal class FontManager : IFontManager
    {
        private readonly IIoCFactory objectFactory;
        private Dictionary<string, IFontFace> fontFaces = new Dictionary<string, IFontFace>();

        private Dictionary<IFontFace, Dictionary<decimal, IFont>> fonts = new Dictionary<IFontFace, Dictionary<decimal, IFont>>();

        private Dictionary<string, string> mappedFontFamilies = new Dictionary<string, string>();

        public FontManager(IIoCFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public void Dispose()
        {
            foreach (var fontsMap in fonts)
            {
                foreach (var font in fontsMap.Value)
                {
                    font.Value.Dispose();
                }
            }
            fonts.Clear();

            foreach (var ff in fontFaces)
            {
                ff.Value.Dispose();
            }
            fontFaces.Clear();
        }

        public IFont GetFont(string name, decimal size, bool bold, bool italic)
        {
            string key = $"{name}/{bold}/{italic}";

            if (!fontFaces.TryGetValue(key, out var fontFace))
            {
                if (!mappedFontFamilies.TryGetValue(name, out var fontFamilies))
                {
                    fontFamilies = name;
                }

                fontFace = objectFactory.Create<IFontFace>(new CreateFontFaceOptions
                {
                    FontFamily = fontFamilies,
                    Bold = bold,
                    Italic = italic
                });

                fontFaces.Add(key, fontFace);
            }

            if (!fonts.TryGetValue(fontFace, out var fontsMap))
            {
                fontsMap = new Dictionary<decimal, IFont>();
                fonts.Add(fontFace, fontsMap);
            }

            if (fontsMap.TryGetValue(size, out var font)) return font;

            font = objectFactory.Create<IFont>(new CreateFontOptions
            {
                FontFace = fontFace,
                FontSize = (float)size
            });

            fontsMap.Add(size, font);
            return font;
        }

        public void MapFontFamily(string commonName, string familyName)
        {
            mappedFontFamilies.Add(commonName, familyName);
        }

        public void RegisterFontFace(string commonName, IFontFace fontFace)
        {
            string key = $"{commonName}/{fontFace.Bold}/{fontFace.Italic}";
            fontFaces.Add(key, fontFace);
        }
    }
}
