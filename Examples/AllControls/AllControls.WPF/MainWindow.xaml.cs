// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using ExampleIoC;
using Skitana.DependencyInjection.Abstractions;
using System.Windows;
using Skitana.Renderer.Skia;
using Skitana.Input.WPF;
using Skitana.App.Framework;
using Skitana.App.Framework.Core;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace AllControls
{
    public partial class MainWindow : Window
    {
        private readonly AppRunner appRunner;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly Task appLoopTask;
        private readonly IDisposable onCloseDisposable;

        public MainWindow()
        {
            InitializeComponent();

            var scopeBuilder = new SkitanaScopeBuilder();

            scopeBuilder.RegisterSkiaRenderer()
                        .RegisterWpfInputServices(skiaElement)
                        .RegisterSkitanaAppFramework()
                        .RegisterAllControlsServices()
                        .RegisterSingleton(skiaElement);

            var services = scopeBuilder.Build();
            onCloseDisposable = services as IDisposable;

            var factory = (IIoCFactory)services.GetService(typeof(IIoCFactory));

            appRunner = factory.Create<WpfAppRunner>();
            var app = factory.Create<AllControlsApp>();

            cancellationTokenSource = new CancellationTokenSource();
            appLoopTask = appRunner.RunAsync(app, cancellationTokenSource.Token);
        }

        protected override void OnClosed(EventArgs args)
        {
            cancellationTokenSource.Cancel();
            appLoopTask.Wait();

            base.OnClosed(args);

            onCloseDisposable?.Dispose();
        }
    }
}
