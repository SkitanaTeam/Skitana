// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Skitana.App.Framework.Input
{
    public interface IGesturesService
    {
        event Action<Gesture> Gesture;

        float MinDragSize { get; set; }
        TimeSpan HoldStartTime { get; set; }
        TimeSpan HoldTime { get; set; }
    }
}
