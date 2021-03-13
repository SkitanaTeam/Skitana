// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.DependencyInjection.Abstractions;
using Skitana.Renderer.Abstractions;
using Skitana.Renderer.Abstractions.Text;
using Skitana.Renderer.Skia.Text;

namespace Skitana.Renderer.Skia
{
    public static class ServicesRegistrar
    {
        public static IServicesContainerBuilder RegisterSkiaRenderer(this IServicesContainerBuilder builder)
        {
            return builder
                    .RegisterImplementation<SkiaFont, IFont>()
                    .RegisterImplementation<SkiaFontFace, IFontFace>()
                    .RegisterImplementation<SkiaImage, IImage>()
                    .RegisterImplementation<SkiaCanvas, ICanvas>();
        }
    }
}
