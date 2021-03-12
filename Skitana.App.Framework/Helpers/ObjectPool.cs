// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Concurrent;

namespace Skitana.App.Framework.Helpers
{
    public class ObjectPool<TObj> where TObj: class, new()
    {
        private ConcurrentQueue<TObj> objects = new ConcurrentQueue<TObj>();

        public TObj Get()
        {
            if (objects.TryDequeue(out var obj)) return obj;
            return new TObj();
        }

        public void Return(TObj obj) => objects.Enqueue(obj);
    }
}
