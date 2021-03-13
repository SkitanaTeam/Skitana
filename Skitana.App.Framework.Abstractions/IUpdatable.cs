// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Skitana.App.Framework.Abstractions
{
    public interface IUpdatable
    {
        void Update(TimeSpan globalTime, TimeSpan ellapsedTime);
    }
}
