using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week44Project.ViewModel
{
    internal class AddCommands
    {
        public event EventHandler? CanExecuteChanged;

        public void RaisCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.actionToInvoke(parameter);
        }

        private Action<Object> actionToInvoke = null;
        public AddCommands(Action<Object> actionToInvoke)
        {
            this.actionToInvoke = actionToInvoke;
        }
    }
}
