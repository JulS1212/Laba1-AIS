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
        private readonly Action _execute; // "Что делать?"

        public RelayCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public bool CanExecute(object parameter) => true; //по умолчанию считаем что команду можно выполнить всегда

        public void Execute(object parameter) => _execute(); //просто вызываем сохраненный метод

        public event EventHandler CanExecuteChanged; // Когда CanExecute меняется, у нас не используется, но интерфейс  ICommand требует его реализации
    }
}
