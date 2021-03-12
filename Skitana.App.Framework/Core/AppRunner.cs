// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.App.Framework.Services;
using Skitana.DependencyInjection.Abstractions;
using Skitana.Renderer.Abstractions;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Skitana.App.Framework.Core
{
    public abstract class AppRunner
    {
        private IApp app;
        private readonly IDispatcher dispatcher;
        private readonly IUpdatablesService updatablesService;
        private readonly IApplicationStopwatch applicationStopwatch;

        protected ICanvas Canvas { get; }
        private TimeSpan? lastRedraw;
        private readonly AutoResetEvent redrawnEvent = new AutoResetEvent(false);

        protected AppRunner(IDispatcher dispatcher, IUpdatablesService updatablesService, IApplicationStopwatch applicationStopwatch, IIoCFactory iocFactory)
        {
            this.dispatcher = dispatcher;
            this.updatablesService = updatablesService;
            this.applicationStopwatch = applicationStopwatch;

            Canvas = iocFactory.Create<ICanvas>();
        }

        public void OnActivated() => dispatcher.InvalidateContent();
        protected abstract void RenderRequest();

        protected void OnRender(object platformCanvas, Size size)
        {
            var time = applicationStopwatch.ElapsedTime;
            var diff = lastRedraw.HasValue ? (time - lastRedraw.Value) : TimeSpan.Zero;

            try
            {
                Canvas.BeginDraw(platformCanvas);
                app.Size = size;
                app.Draw(Canvas, diff);
            }
            finally
            {
                Canvas.EndDraw();
            }

            lastRedraw = time;
            redrawnEvent.Set();
        }

        public Task RunAsync(IApp app, CancellationToken cancellationToken)
        {
            this.app = app;
            cancellationToken.Register(() => redrawnEvent.Set());
            return Task.Run(() => Loop(cancellationToken), cancellationToken);
        }

        public void Run(IApp app)
        {
            this.app = app;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Loop(cancellationTokenSource.Token);
        }

        private void Loop(CancellationToken cancellationToken)
        {
            const double targetFps = 120;
            var lastTimeFrame = applicationStopwatch.ElapsedTime;
            var waitHandle = new AutoResetEvent(false);

            redrawnEvent.WaitOne();
            app.Load();

            while (!cancellationToken.IsCancellationRequested)
            {
                TimeSpan time = applicationStopwatch.ElapsedTime;
                TimeSpan diff = time - lastTimeFrame;

                if (dispatcher is IUpdatable updatable)
                {
                    updatable.Update(time, diff);
                }

                updatablesService.Update(time, diff);
                app.Update(time, diff);

                dispatcher.GetIsContentInvalid(true, out var shouldRedraw);

                TimeSpan newTime = applicationStopwatch.ElapsedTime;
                diff = newTime - time;
                int msToWait = Math.Max(1, (int)(1000.0 / targetFps - diff.TotalMilliseconds));

                if (shouldRedraw)
                {
                    RenderRequest();
                    redrawnEvent.WaitOne();
                }
                else
                {
                    waitHandle.WaitOne(msToWait);
                }

                lastTimeFrame = time;
            }
        }
    }
}
