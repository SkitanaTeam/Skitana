// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.Input.Abstractions.Pointers;
using System;
using System.Numerics;

namespace Skitana.App.Framework.Input
{
    public class Gesture
    {
        public PointerId PointerId { internal set; get; }
        public GestureType GestureType { internal set; get; }
        public Vector2 Origin { internal set; get; }
        public Vector2 Position { internal set; get; }
        public Vector2 Offset { internal set; get; }
        public TimeSpan Time { internal set; get; }

        public object PointerCapturedBy { get; internal set; }

        public bool Handled { get; private set; }

        public void SetHandled() => Handled = true;

        internal void Reset() => Handled = false;

        public void CapturePointer(object captureBy)
        {
            if (PointerCapturedBy != null && PointerCapturedBy != captureBy)
            {
                throw new Exception("Pointer already captured by another object.");
            }

            PointerCapturedBy = captureBy;
        }

        internal void Init(PointerId pointerId, Vector2 origin, Vector2 position, TimeSpan time)
        {
            PointerId = PointerId;
            Origin = origin;
            Position = position;
            Time = time;
            Offset = Vector2.Zero;
            PointerCapturedBy = null;
            Handled = false;
        }

        public void Rescale(Vector2 scale)
        {
            Origin *= scale;
            Position *= scale;
            Offset *= scale;
        }
    }
}
