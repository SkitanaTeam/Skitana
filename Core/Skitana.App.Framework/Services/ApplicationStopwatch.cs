// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.App.Framework.Abstractions;
using System;
using System.Diagnostics;

namespace Skitana.App.Framework.Services
{
    internal class ApplicationStopwatch : IApplicationStopwatch
    {
        public TimeSpan ElapsedTime => stopwatch.Elapsed;
        private readonly Stopwatch stopwatch;
        public ApplicationStopwatch()
        {
            stopwatch = Stopwatch.StartNew();
        }
        public void Dispose()
        {
            stopwatch.Stop();
        }
    }
}
