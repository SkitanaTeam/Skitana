// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.App.Framework.Core;
using System;

namespace Skitana.App.Framework.Services
{
    public interface IUpdatablesService
    {
        void Register(IUpdatable updatable);
        void Unregister(IUpdatable updatable);

        void Update(TimeSpan globalTime, TimeSpan elapsedTime);
    }
}
