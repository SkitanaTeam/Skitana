// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using Skitana.DependencyInjection.Abstractions;
using System;
using XxIoC;

namespace ExampleIoC
{
    public class SkitanaScopeBuilder : IServicesContainerBuilder
    {
        private ScopeBuilder scopeBuilder = new ScopeBuilder();

        public SkitanaScopeBuilder()
        {
            scopeBuilder.WithType<SkitanaIoCFactory>().As<IIoCFactory>();
        }

        IServicesContainerBuilder IServicesContainerBuilder.RegisterSingleton<InterfaceType>(InterfaceType instance)
        {
            scopeBuilder.WithInstance(instance).As<InterfaceType>();
            return this;
        }

        IServicesContainerBuilder IServicesContainerBuilder.RegisterSingleton<Type, InterfaceType>()
        {
            scopeBuilder.WithType<Type>().As<InterfaceType>().AsSingleton();
            return this;
        }

        IServicesContainerBuilder IServicesContainerBuilder.RegisterImplementation<Type, AbstractType>()
        {
            scopeBuilder.WithType<Type>().As<AbstractType>();
            return this;
        }

        public IServiceProvider Build() => scopeBuilder.Build();
    }
}
