// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Threading.Tasks;

namespace Skitana.UI.Framework.Core
{
    public interface INavigation
    {
        Task<TViewModel> Navigate<TViewModel>(params object[] parameters) where TViewModel : class;
        Task Navigate<TViewModel>(TViewModel viewModel) where TViewModel : class;
        Task NavigateBack();
        Task Clear();
    }
}