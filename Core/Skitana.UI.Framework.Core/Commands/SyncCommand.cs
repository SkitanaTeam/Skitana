// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Windows.Input;

namespace Skitana.UI.Framework.Core.Commands
{
    public class SyncCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Func<object, bool> canExecute;
        private Action<object> action;

        public void FireCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                action?.Invoke(parameter);
            }
        }

        public SyncCommand(Action<object> action, Func<object, bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public SyncCommand(Action action, Func<bool> canExecute) : this(o => action?.Invoke(), o => canExecute?.Invoke() ?? true)
        {

        }

        public SyncCommand(Action<object> action) : this(action, null)
        {

        }

        public SyncCommand(Action action) : this(action, null)
        {

        }
    }
}
