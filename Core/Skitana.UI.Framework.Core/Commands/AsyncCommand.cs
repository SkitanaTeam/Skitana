// MIT License - Copyright © Skitana Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Skitana.UI.Framework.Core.Commands
{
    public class AsyncCommand: ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Func<object, bool> canExecute;
        private Func<object, Task> action;
        private readonly Action<Exception> onException;

        public void FireCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                var task = action?.Invoke(parameter);
                if(task != null)
                {
                    try
                    {
                        await task;
                    }
                    catch (AggregateException aex)
                    {
                        Exception ex = aex;
                        while (ex is AggregateException)
                        {
                            ex = ex.InnerException;
                        }

                        if (ex != null)
                        {
                            onException?.Invoke(ex);
                        }
                    }
                }
            }
        }

        public AsyncCommand(Func<object, Task> action, Func<object, bool> canExecute, Action<Exception> onException = null)
        {
            this.action = action;
            this.canExecute = canExecute;
            this.onException = onException;
        }

        public AsyncCommand(Func<Task> action, Func<bool> canExecute, Action<Exception> onException = null) : this(o => action?.Invoke(), o => canExecute?.Invoke() ?? true, onException)
        {

        }

        public AsyncCommand(Func<object, Task> action, Action<Exception> onException = null) : this(action, null, onException)
        {

        }

        public AsyncCommand(Func<Task> action, Action<Exception> onException = null) : this(action, null, onException)
        {

        }
    }
}
