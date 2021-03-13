// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using SkiaSharp.Views.WPF;
using Skitana.App.Framework.Core;
using Skitana.App.Framework.Services;
using Skitana.App.Framework.Abstractions;
using Skitana.DependencyInjection.Abstractions;
using Skitana.Input.Abstractions.Pointers;
using System.Drawing;
using System.Numerics;
using System.Windows.Threading;

namespace AllControls
{
    public class WpfAppRunner : AppRunner
    {
        private readonly Dispatcher winDispatcher;
        private readonly SKElement skElement;
        public WpfAppRunner(SKElement skElement, IDispatcher dispatcher, IUpdatablesService updatablesService, IInputPanel inputPanel, IApplicationStopwatch applicationStopwatch, IIoCFactory iocFactory)
            : base(dispatcher, updatablesService, applicationStopwatch, iocFactory)
        {
            winDispatcher = skElement.Dispatcher;
            this.skElement = skElement;

            skElement.PaintSurface += (o, args) =>
            {
                inputPanel.Scale = new Vector2((float)(args.Info.Width / skElement.ActualWidth),
                    (float)(args.Info.Height / skElement.ActualHeight));

                OnRender(args.Surface.Canvas, new Size(args.Info.Width, args.Info.Height));
            };
        }

        protected override void RenderRequest() => winDispatcher.BeginInvoke(skElement.InvalidateVisual, DispatcherPriority.ApplicationIdle);
    }
}
