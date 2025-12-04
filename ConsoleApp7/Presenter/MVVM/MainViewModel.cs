using BusinessLogical.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presenter
{
    public class MainViewModel : BaseViewModel
    {
        // Сервис бизнес-логики (получаем через Ninject)
        private readonly IPaintingService _paintingService;
        private readonly ViewManager _viewManager;

        // 1. КОЛЛЕКЦИЯ ВСЕХ КАРТИН (для отображения в списке)
        private BindingList<PaintingDto> _paintings;
        public BindingList<PaintingDto> Paintings
        {
            get => _paintings;
            set
            {
                _paintings = value;
                OnPropertyChanged();
            }
        }

        // 2. ВЫБРАННАЯ КАРТИНА В СПИСКЕ (при выборе заполняет форму)
        private PaintingDto _selectedPainting;
        public PaintingDto SelectedPainting
        {
            get => _selectedPainting;
            set
            {
                _selectedPainting = value;
                OnPropertyChanged();

                // Автоматически заполняем поля формы данными выбранной картины
                if (value != null)
                {
                    Title = value.Title;
                    Artist = value.Artist;
                    Year = value.Year;
                    Genre = value.Genre;
                }
            }
        }

        // 3. СВОЙСТВА ДЛЯ ПОЛЕЙ ФОРМЫ (то, что пользователь вводит/видит)

        // Название картины
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        // Автор картины
        private string _artist;
        public string Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                OnPropertyChanged();
            }
        }

        // Год создания
        private int _year;
        public int Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }

        // Жанр
        private string _genre;
        public string Genre
        {
            get => _genre;
            set
            {
                _genre = value;
                OnPropertyChanged();
            }
        }

        // Год ОТ для поиска
        private int _startYear;
        public int StartYear
        {
            get => _startYear;
            set
            {
                _startYear = value;
                OnPropertyChanged();
            }
        }

        // Год ДО для поиска
        private int _endYear;
        public int EndYear
        {
            get => _endYear;
            set
            {
                _endYear = value;
                OnPropertyChanged();
            }
        }

        // 4. КОМАНДЫ (вместо событий из IView)
        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand SearchByYearCommand { get; }
        public ICommand GroupByGenreCommand { get; }
        public ICommand SortAscendingCommand { get; }
        public ICommand SortDescendingCommand { get; }
        public ICommand ClearCommand { get; }

        // 5. КОНСТРУКТОР (зависимость через Ninject)
        public MainViewModel(IPaintingService paintingService, ViewManager viewManager)
        {
            _paintingService = paintingService ?? throw new ArgumentNullException(nameof(paintingService));
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));

            // Инициализируем коллекцию
            Paintings = new BindingList<PaintingDto>();

            // Создаем команды и связываем с методами
            LoadCommand = new RelayCommand(LoadPaintings);
            AddCommand = new RelayCommand(AddPainting);
            DeleteCommand = new RelayCommand(DeletePainting);
            UpdateCommand = new RelayCommand(UpdatePainting);
            SearchByYearCommand = new RelayCommand(SearchByYear);
            GroupByGenreCommand = new RelayCommand(GroupByGenre);
            SortAscendingCommand = new RelayCommand(SortAscending);
            SortDescendingCommand = new RelayCommand(SortDescending);
            ClearCommand = new RelayCommand(ClearForm);

            // Загружаем картины при старте
            LoadPaintings();

        }

        // 6. МЕТОДЫ-ОБРАБОТЧИКИ КОМАНД

        /// <summary>
        /// Загружает все картины из БД
        /// </summary>
        private void LoadPaintings()
        {
            try
            {
                var paintings = _paintingService.GetAllPaintings();
                Paintings = new BindingList<PaintingDto>(
                    paintings.Select(p => new PaintingDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Artist = p.Artist,
                        Year = p.Year,
                        Genre = p.Genre
                    }).ToList()
                );
            }
            catch (Exception ex)
            {
                // В реальном приложении здесь будет вывод ошибки
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавляет новую картину
        /// </summary>
        private void AddPainting()
        {
            try
            {
                _paintingService.AddPainting(Title, Artist, Year, Genre);
                ClearForm();
                LoadPaintings(); // Перезагружаем список
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка добавления: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет выбранную картину
        /// </summary>
        private void DeletePainting()
        {
            if (SelectedPainting == null) return;

            try
            {
                if (_paintingService.DeletePainting(SelectedPainting.Title, SelectedPainting.Artist))
                {
                    ClearForm();
                    LoadPaintings(); // Перезагружаем список
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновляет выбранную картину
        /// </summary>
        private void UpdatePainting()
        {
            if (SelectedPainting == null) return;

            try
            {
                if (_paintingService.UpdatePainting(
                    SelectedPainting.Title,
                    SelectedPainting.Artist,
                    Title,
                    Artist,
                    Year,
                    Genre))
                {
                    ClearForm();
                    LoadPaintings(); // Перезагружаем список
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка обновления: {ex.Message}");
            }
        }

        /// <summary>
        /// Ищет картины по диапазону лет
        /// </summary>
        private void SearchByYear()
        {
            try
            {
                var paintings = _paintingService.GetPaintingsByYearRange(StartYear, EndYear);
                Paintings = new BindingList<PaintingDto>(
                    paintings.Select(p => new PaintingDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Artist = p.Artist,
                        Year = p.Year,
                        Genre = p.Genre
                    }).ToList()
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка поиска: {ex.Message}");
            }
        }

        /// <summary>
        /// Группирует картины по жанрам
        /// </summary>
        // В MainViewModel.cs (метод GroupByGenre)
        private void GroupByGenre()
        {
            try
            {
                // 1. СОЗДАЁМ ViewModel для окна группировки (ViewModel First!)
                var groupedViewModel = new GroupedViewModel(_paintingService);

                // 2. ПРОСИМ ViewManager показать эту ViewModel
                // ViewManager сам найдёт, что GroupedViewModel → GroupedView
                // Создаст окно, установит DataContext и покажет
                _viewManager.ShowDialog(groupedViewModel);

                // 3. (Опционально) После закрытия окна можно обновить главный список
                // Например, если в окне группировки что-то изменили
                // LoadPaintings();
            }
            catch (Exception ex)
            {
                // В реальном приложении здесь будет вывод ошибки в UI
                System.Diagnostics.Debug.WriteLine($"Ошибка открытия группировки: {ex.Message}");

                // Если хочешь показать ошибку пользователю (но это не совсем по MVVM):
                // MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                //                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Сортирует картины по названию (А-Я)
        /// </summary>
        private void SortAscending()
        {
            try
            {
                var paintings = _paintingService.SortByTitleAscending();
                Paintings = new BindingList<PaintingDto>(
                    paintings.Select(p => new PaintingDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Artist = p.Artist,
                        Year = p.Year,
                        Genre = p.Genre
                    }).ToList()
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сортировки: {ex.Message}");
            }
        }

        /// <summary>
        /// Сортирует картины по названию (Я-А)
        /// </summary>
        private void SortDescending()
        {
            try
            {
                var paintings = _paintingService.SortByTitleDescending();
                Paintings = new BindingList<PaintingDto>(
                    paintings.Select(p => new PaintingDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Artist = p.Artist,
                        Year = p.Year,
                        Genre = p.Genre
                    }).ToList()
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сортировки: {ex.Message}");
            }
        }

        /// <summary>
        /// Очищает поля формы
        /// </summary>
        private void ClearForm()
        {
            Title = string.Empty;
            Artist = string.Empty;
            Year = 0;
            Genre = string.Empty;
            StartYear = 0;
            EndYear = 0;
            SelectedPainting = null;
        }

    }
}
