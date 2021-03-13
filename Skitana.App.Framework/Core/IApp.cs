// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.App.Framework.Abstractions;
using Skitana.Renderer.Abstractions;
using System;
using System.Drawing;

namespace Skitana.App.Framework.Core
{
    public interface IApp : IUpdatable
    {
        Size Size { set; }
        void Draw(ICanvas canvas, TimeSpan elapsedTime);
        void Load();
    }
}
