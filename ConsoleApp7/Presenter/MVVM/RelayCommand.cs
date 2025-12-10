using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presenter
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute; // что делать
        private readonly Func<bool> _canExecute;  // можно ли выполнить

        public RelayCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public bool CanExecute(object parameter) => true; //по умолчанию считаем что команду можно выполнить всегда
        //public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;  // Если проверка есть - используем

        public void Execute(object parameter) => _execute(); //просто вызываем сохраненный метод

        public event EventHandler CanExecuteChanged; // Когда CanExecute меняется, у нас не используется, но интерфейс  ICommand требует его реализации
    }
}
