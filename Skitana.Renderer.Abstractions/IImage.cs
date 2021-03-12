// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Drawing;

namespace Skitana.Renderer.Abstractions
{
    public interface IImage : IDisposable
    {
        Size Size { get; }
    }
}
