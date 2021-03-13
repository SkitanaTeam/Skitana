// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;
using System.Numerics;

namespace Skitana.Input.Abstractions.Pointers
{
    public interface IInputPanel
    {
        Vector2 Scale { set; }

        event EventHandler<PointerEventArgs> PointerDown;
        event EventHandler<PointerEventArgs> PointerUp;
        event EventHandler<PointerEventArgs> PointerLost;
        event EventHandler<PointerEventArgs> PointerMove;
    }
}
