// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.App.Framework.Core;
using Skitana.App.Framework.Helpers;
using System;
using System.Collections.Generic;

namespace Skitana.App.Framework.Services
{
    internal class Dispatcher : IDispatcher, IUpdatable
    {
        private TimeSpan currentTime;
        private List<Pair<TimeSpan, Action>> enqueuedActions = new List<Pair<TimeSpan, Action>>();
        private List<Pair<TimeSpan, Action>> tempEnqueuedActions = new List<Pair<TimeSpan, Action>>();

        private object listLock = new object();
        private object redrawLock = new object();
        private bool shouldRedraw;

        public void Enqueue(Action action)
        {
            lock (listLock)
            {
                enqueuedActions.Add(new Pair<TimeSpan, Action>(currentTime, action));
            }
        }

        public void EnqueueDelayed(Action action, TimeSpan delay)
        {
            lock (listLock)
            {
                enqueuedActions.Add(Pair.Create(currentTime + delay, action));
            }
        }

        public void GetIsContentInvalid(bool reset, out bool shouldRedraw)
        {
            lock (redrawLock)
            {
                shouldRedraw = this.shouldRedraw;
                if (reset) this.shouldRedraw = false;
            }
        }

        public void InvalidateContent()
        {
            lock (redrawLock) shouldRedraw = true;
        }

        public void Update(TimeSpan currentTime, TimeSpan _)
        {
            this.currentTime = currentTime;

            lock (listLock)
            {
                Utils.Swap(ref tempEnqueuedActions, ref enqueuedActions);
                enqueuedActions.Clear();
            }

            for (var idx = 0; idx < tempEnqueuedActions.Count;)
            {
                var action = tempEnqueuedActions[idx];

                if (action.V1 <= currentTime)
                {
                    action.V2.Invoke();
                    tempEnqueuedActions.RemoveAt(idx);
                    continue;
                }
                ++idx;
            }

            lock (listLock)
            {
                for (var idx = 0; idx < tempEnqueuedActions.Count;)
                {
                    enqueuedActions.Add(tempEnqueuedActions[idx]);
                }
                tempEnqueuedActions.Clear();
            }
        }
    }
}
