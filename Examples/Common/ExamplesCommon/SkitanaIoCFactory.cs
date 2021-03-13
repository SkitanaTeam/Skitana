// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Skitana.DependencyInjection.Abstractions;
using XxIoC;

namespace ExampleIoC
{
    internal class SkitanaIoCFactory : IIoCFactory
    {
        private readonly IObjectFactory objectsFactory;

        public SkitanaIoCFactory(IObjectFactory objectsFactory)
        {
            this.objectsFactory = objectsFactory;
        }

        public TObject Create<TObject>(params object[] parameters) => objectsFactory.Create<TObject>(parameters);
    }
}
