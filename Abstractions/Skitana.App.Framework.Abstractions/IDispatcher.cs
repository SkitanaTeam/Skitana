// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Skitana.App.Framework.Abstractions
{
    public interface IDispatcher
    {
        void Enqueue(Action action);
        void EnqueueDelayed(Action action, TimeSpan delay);
        void InvalidateContent();
        void GetIsContentInvalid(bool reset, out bool shouldRedraw);
    }
}
