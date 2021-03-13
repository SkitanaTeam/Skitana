// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.Renderer.Abstractions.Text;
using System;

namespace Skitana.App.Framework.Text
{
    public interface IFontManager : IDisposable
    {
        void RegisterFontFace(string commonName, IFontFace fontFace);
        void MapFontFamily(string commonName, string familyName);
        IFont GetFont(string name, decimal size, bool bold, bool italic);
    }
}
