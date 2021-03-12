// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Skitana.UI.Framework.Core
{
    public abstract class NavigationViewModel: DataContext
    {
        protected INavigation Navigation { get; }

        public NavigationViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
