// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Skitana.App.Framework.Input
{
    [Flags]
    public enum GestureType
    {
        None = 0x00,
        Down = 0x01,
        Move = 0x02,
        Up = 0x04,
        HorizontalDrag = 0x08,
        VerticalDrag = 0x10,
        FreeDrag = 0x20,
        Tap = 0x40,
        HoldStart = 0x80,
        Hold = 0x100,
        Swipe = 0x200,
        PointerLost = 0x400,
        HoldCancel = 0x800,
        CapturedByOther = 0x1000,

        HoldGestures = Hold | HoldStart | HoldCancel,
        DragGestures = HorizontalDrag | VerticalDrag | FreeDrag
    }
}