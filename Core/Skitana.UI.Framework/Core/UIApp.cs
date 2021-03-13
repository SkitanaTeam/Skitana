// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.App.Framework.Core;
using Skitana.DependencyInjection.Abstractions;
using Skitana.IO.Abstractions;
using Skitana.Renderer.Abstractions;
using System;
using System.Drawing;
using System.Numerics;

namespace Skitana.UI.Framework.Core
{
    public class UIApp : IApp
    {
        public Size Size { get; set; }
        private bool loaded = false;

        private IImage splashScreen;
        private Color splashScreenBackground;
        private bool splashScreenFill;

        public UIApp(UIAppParameters parameters, IIoCFactory factory, IFilesRepository filesRepository)
        {
            if (parameters.SplashScreenPath != null)
            {
                using (var stream = filesRepository.Open(parameters.SplashScreenPath))
                {
                    splashScreen = factory.Create<IImage>(stream);
                }
            }

            splashScreenBackground = parameters.SplashScreenBackground;
            splashScreenFill = parameters.SplashScreenFill;
        }

        public void Draw(ICanvas canvas, TimeSpan elapsedTime)
        {
            if (!loaded)
            {
                if (splashScreen != null)
                {
                    canvas.Clear(splashScreenBackground);
                    float scale = Math.Min((float)Size.Width / splashScreen.Size.Width, (float)Size.Height / splashScreen.Size.Height);

                    if (!splashScreenFill) scale = Math.Min(1, scale);

                    var position = new Vector2((Size.Width - scale * splashScreen.Size.Width) / 2, (Size.Height - scale * splashScreen.Size.Height) / 2);
                    canvas.DrawImage(splashScreen, position, new Vector2(scale));
                }
                return;
            }

            // TODO: Draw loaded UI...
        }

        public void Load()
        {


            loaded = true;
            splashScreen?.Dispose();
            splashScreen = null;
        }

        public void Update(TimeSpan globalTime, TimeSpan ellapsedTime)
        {
            
        }
    }
}
