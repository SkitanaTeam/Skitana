// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Skitana.DependencyInjection.Abstractions;
using Skitana.IO;
using Skitana.IO.Abstractions;

namespace AllControls
{
    public static class ServicesRegistrar
    {
        public static IServicesContainerBuilder RegisterAllControlsServices(this IServicesContainerBuilder builder)
        {
            return builder
                .RegisterSingleton(InitFilesRepository());
        }

        private static IFilesRepository InitFilesRepository()
        {
            var aggregatedFilesRepository = new AggregatedFilesRepository
            {
                DefaultSource = new EmbededResourceRepository(typeof(ServicesRegistrar).Assembly, "AllControls.Assets")
            };

            return aggregatedFilesRepository;
        }
    }
}
