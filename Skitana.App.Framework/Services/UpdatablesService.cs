// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.App.Framework.Core;
using System;
using System.Collections.Generic;

namespace Skitana.App.Framework.Services
{
    internal class UpdatablesService : IUpdatablesService
    {
        private List<IUpdatable> updatables = new List<IUpdatable>();
        private List<IUpdatable> tempUpdatables = new List<IUpdatable>();

        private object listLock = new object();

        public void Update(TimeSpan globalTime, TimeSpan elapsedTime)
        {
            lock (listLock)
            {
                tempUpdatables.Clear();
                tempUpdatables.AddRange(updatables);
            }

            for (var idx = 0; idx < tempUpdatables.Count; ++idx)
            {
                tempUpdatables[idx].Update(globalTime, elapsedTime);
            }
        }

        public void Register(IUpdatable updatable)
        {
            lock (listLock)
            {
                if (!updatables.Contains(updatable))
                {
                    updatables.Add(updatable);
                }
            }
        }

        public void Unregister(IUpdatable updatable)
        {
            lock (listLock)
            {
                updatables.Remove(updatable);
            }
        }
    }
}
