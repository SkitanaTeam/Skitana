// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Numerics;

namespace Skitana.Input.Abstractions.Pointers
{
    public class MouseWheelEventArgs : PointerEventArgs
    {
        public int WheelDelta { get; }

        public MouseWheelEventArgs(TimeSpan time, Vector2 position, PointerId pointerId, long mouseButtonsDown, int wheelDelta)
            : base(time, position, pointerId, mouseButtonsDown)
        {
            WheelDelta = wheelDelta;
        }
    }
}
