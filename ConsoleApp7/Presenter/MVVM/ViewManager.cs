using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presenter
{
    public class ViewManager
    {
        // Словарь: тип ViewModel → тип View (окна)
        private readonly Dictionary<Type, Type> _viewModelToViewMap = new Dictionary<Type, Type>();

        /// <summary>
        /// Регистрирует связь между ViewModel и View
        /// </summary>
        public void Register<TViewModel, TView>()
            where TViewModel : BaseViewModel
            where TView : Window
        {
            _viewModelToViewMap[typeof(TViewModel)] = typeof(TView);
        }

        /// <summary>
        /// Показывает немодальное окно для указанной ViewModel
        /// </summary>
        public void Show(BaseViewModel viewModel)
        {
            var view = CreateViewForViewModel(viewModel);
            view.DataContext = viewModel; // Разрешено по заданию
            view.Show();
        }

        /// <summary>
        /// Показывает модальное окно (диалог) для указанной ViewModel
        /// </summary>
        public bool? ShowDialog(BaseViewModel viewModel)
        {
            var view = CreateViewForViewModel(viewModel);
            view.DataContext = viewModel; // Разрешено по заданию
            return view.ShowDialog();
        }

        /// <summary>
        /// Создаёт View (окно) для указанной ViewModel
        /// </summary>
        private Window CreateViewForViewModel(BaseViewModel viewModel)
        {
            Type viewModelType = viewModel.GetType();

            if (!_viewModelToViewMap.ContainsKey(viewModelType))
            {
                throw new InvalidOperationException(
                    $"Не зарегистрирован View для ViewModel: {viewModelType.Name}");
            }

            Type viewType = _viewModelToViewMap[viewModelType];
            return (Window)Activator.CreateInstance(viewType);
        }
    }
}
