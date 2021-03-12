// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Skitana.App.Framework.Core;
using Skitana.App.Framework.Services;
using Skitana.App.Framework.Text;
using Skitana.DependencyInjection.Abstractions;

namespace Skitana.App.Framework
{
    public static class ServicesRegistrar
    {
        public static IServicesContainerBuilder RegisterSkitanaAppFramework(this IServicesContainerBuilder builder)
        {
            return builder
                    .RegisterSingleton<Dispatcher, IDispatcher>()
                    .RegisterSingleton<UpdatablesService, IUpdatablesService>()
                    .RegisterSingleton<ApplicationStopwatch, IApplicationStopwatch>()
                    .RegisterSingleton<FontManager, IFontManager>();
        }
    }
}
