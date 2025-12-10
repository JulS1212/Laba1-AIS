using BusinessLogical.Interfaces;
using Ninject;
using Presenter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ArtGallery.WPF
{
    public partial class App : Application // точка входа, при запуски автоматически создается экзмепляр и вызывается OnStartup
    {
        private IKernel _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _container = new StandardKernel(new BusinessLogical.NinjectConfig());
            ComposeObjects();
        }
        private void ComposeObjects()
        {
            try
            {

                // создаем ViewManager
                var viewManager = new ViewManager();

                // регистстрируем соотвествиее VM и V
                viewManager.Register<MainViewModel, MainWindow>(); //если MainViewModel, то MainWindow
                viewManager.Register<GroupedViewModel, GroupedView>();

                // получаем сервис
                var paintingService = _container.Get<IPaintingService>();

                // создаем главную ViewModel (передаём viewManager!)
                var mainViewModel = new MainViewModel(paintingService, viewManager);

                // показываем главное окно через ViewManager
                viewManager.Show(mainViewModel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ОШИБКА в ComposeObjects: {ex.Message}");
                MessageBox.Show($"Ошибка запуска: {ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
