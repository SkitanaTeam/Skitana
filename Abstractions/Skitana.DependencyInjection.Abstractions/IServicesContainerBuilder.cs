// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Skitana.DependencyInjection.Abstractions
{
    public interface IServicesContainerBuilder
    {
        IServicesContainerBuilder RegisterSingleton<Type, InterfaceType>() where Type : class, InterfaceType where InterfaceType : class;
        IServicesContainerBuilder RegisterSingleton<InterfaceType>(InterfaceType instance) where InterfaceType : class;
        IServicesContainerBuilder RegisterImplementation<Type, AbstractType>() where Type : class, AbstractType where AbstractType : class;
    }
}
