// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Skitana.App.Framework.Core;
using Skitana.App.Framework.Text;
using Skitana.App.Framework.Abstractions;
using Skitana.DependencyInjection.Abstractions;
using Skitana.IO.Abstractions;
using Skitana.Renderer.Abstractions;
using Skitana.Renderer.Abstractions.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Skitana.App.Framework.Input;

namespace AllControls
{
    public class AllControlsApp : IApp, IGestureConsumer
    {
        private readonly IDispatcher dispatcher;
        private readonly IIoCFactory objectFactory;
        private readonly IFilesRepository filesRepository;
        private readonly IFontManager fontManager;
        private IImage image;

        private float rotation = 0;
        private float fps = 0;

        private Dictionary<int, string> fpses = new Dictionary<int, string>();

        private Vector2 position = Vector2.Zero;

        public AllControlsApp(IDispatcher dispatcher, IIoCFactory objectFactory, IFilesRepository filesRepository, IFontManager fontManager, IGesturesService gesturesService)
        {
            this.dispatcher = dispatcher;
            this.objectFactory = objectFactory;
            this.filesRepository = filesRepository;
            this.fontManager = fontManager;

            gesturesService.Register(this);
        }

        public void OnGesture(Gesture gesture)
        {
            gesture.Rescale(new Vector2(720f / Size.Height));
            switch(gesture.GestureType)
            {
                case GestureType.FreeDrag:
                    position += gesture.Offset;
                    break;

                case GestureType.Tap:
                    break;

                case GestureType.Hold:
                    break;

                case GestureType.Up:
                    break;
            }
        }

        public Size Size { set; private get; }

        public void Draw(ICanvas canvas, TimeSpan elapsedTime)
        {
            if (image == null)
            {
                canvas.Clear(Color.Blue);
                return;
            }

            var scale = new Vector2(Size.Height / 720f);
            canvas.Scale(scale, Vector2.Zero);
            canvas.Clear(Color.Red);

            var font = fontManager.GetFont("Mono", 20, false, false);

            canvas.PushState();

            var center = position + new Vector2(Size.Width / 2, Size.Height / 2) / scale;

            //canvas.RotateDegrees(rotation, center);
            canvas.DrawImage(image, center - new Vector2(image.Size.Width, image.Size.Height)/2);
            canvas.PopState();

            for (var idx = 0; idx < 10; ++idx)
            {
                canvas.DrawText(font, new Vector2(5, font.Size * (idx + 1) + 5), "TEST TEXT!", Color.White, TextAlign.Left);
            }

            if (elapsedTime.TotalSeconds > 0)
            {
                fps = (fps + 1f / (float)elapsedTime.TotalSeconds) / 2;
            }

            if (!fpses.TryGetValue((int)fps, out var _))
            {
                fpses[(int)fps] = $"FPS: {(int)fps}";
            }

            font = fontManager.GetFont("Mono", 16, false, false);
            canvas.DrawText(font, new Vector2(0, font.Size), fpses[(int)fps], Color.White, TextAlign.Left);
        }

        public void Load()
        {
            fontManager.MapFontFamily("Mono", "Menlo, Courier, Roboto Mono");

            fontManager.GetFont("Mono", 16, false, false);
            fontManager.GetFont("Mono", 20, false, false);

            using (var stream = filesRepository.Open("Lama.png"))
            {
                image = objectFactory.Create<IImage>(stream);
            }
        }

        public void Update(TimeSpan globalTime, TimeSpan ellapsedTime)
        {
            rotation = (float)(globalTime.TotalSeconds * 45) % 360;
            dispatcher.InvalidateContent();
        }
    }
}
