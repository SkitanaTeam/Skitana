// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Skitana.Renderer.Abstractions.Text
{
    public interface IFont : IDisposable
    {
        IFontFace FontFace { get; }
        float Size { get; }
        float BaseLine { get; }
        float LineHeight { get; }
        float GetTextWidth(string text);
    }
}
