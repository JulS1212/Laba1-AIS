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
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
        }

        private void ConfigureContainer()
        {
            // Используем ту же конфигурацию Ninject
            _container = new StandardKernel(new BusinessLogical.NinjectConfig());
        }

        // App.xaml.cs - метод ComposeObjects()
        private void ComposeObjects()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Настройка приложения ===");

                // 1. СОЗДАЁМ ViewManager
                var viewManager = new ViewManager();
                System.Diagnostics.Debug.WriteLine("ViewManager создан");

                // 2. РЕГИСТРИРУЕМ связки ViewModel → View
                viewManager.Register<MainViewModel, MainWindow>();
                viewManager.Register<GroupedViewModel, GroupedView>();
                System.Diagnostics.Debug.WriteLine("Связки зарегистрированы");

                // 3. ПОЛУЧАЕМ сервис из Ninject
                var paintingService = _container.Get<IPaintingService>();
                System.Diagnostics.Debug.WriteLine($"Сервис получен: {paintingService != null}");

                // 4. СОЗДАЁМ главную ViewModel (передаём viewManager!)
                var mainViewModel = new MainViewModel(paintingService, viewManager);
                System.Diagnostics.Debug.WriteLine($"MainViewModel создан: {mainViewModel != null}");

                // 5. ПОКАЗЫВАЕМ главное окно через ViewManager
                viewManager.Show(mainViewModel);
                System.Diagnostics.Debug.WriteLine("Главное окно показано");
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
