// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Numerics;

namespace Skitana.App.Framework.Input
{
    internal class PointerDownElement
    {
        public Vector2 Origin;
        public Vector2 Position;

        public GestureType LockedGesture;
        public TimeSpan DownTime;

        public object LockedListener;
    }
}