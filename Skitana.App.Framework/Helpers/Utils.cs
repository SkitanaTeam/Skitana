// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Skitana.App.Framework.Helpers
{
    public static class Utils
    {
        public static void Swap<T>(ref T val1, ref T val2)
        {
            T temp = val1;
            val1 = val2;
            val2 = temp;
        }   
    }
}
