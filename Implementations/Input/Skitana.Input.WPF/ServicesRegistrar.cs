using Skitana.DependencyInjection.Abstractions;
using Skitana.Input.Abstractions.Pointers;
using System.Windows;

namespace Skitana.Input.WPF
{
    public static class ServicesRegistrar
    {
        public static IServicesContainerBuilder RegisterWpfInputServices(this IServicesContainerBuilder builder, FrameworkElement element)
        {
            return builder
                .RegisterSingleton(element)
                .RegisterSingleton<InputPanel, IInputPanel>();
        }
    }
}
