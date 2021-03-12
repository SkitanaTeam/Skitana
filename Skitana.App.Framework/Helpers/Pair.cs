// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Skitana.App.Framework.Helpers
{
    public struct Pair
    {
        public static Pair<T1, T2> Create<T1, T2>(T1 v1, T2 v2) => new Pair<T1, T2>(v1, v2);
    }

    public struct Pair<T1, T2>
    {
        public T1 V1 {get;}
        public T2 V2 {get;}
        public Pair(T1 v1, T2 v2)
        {
            V1 = v1;
            V2 = v2;
        }
    }
}
