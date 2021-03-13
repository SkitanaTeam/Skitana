// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Numerics;

namespace Skitana.Input.Abstractions.Pointers
{
    public class PointerEventArgs : EventArgs
    {
        public TimeSpan Time { get; }
        public Vector2 Position { get; }
        public PointerId PointerId { get; }
        public long MouseButtonsDown { get; }

        public PointerEventArgs(TimeSpan time, Vector2 position, PointerId pointerId, long mouseButtonsDown)
        {
            Time = time;
            Position = position;
            PointerId = pointerId;
            MouseButtonsDown = mouseButtonsDown;
        }
    }
}