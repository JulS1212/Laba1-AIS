using BusinessLogical.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Presenter
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IPaintingService _paintingService;
        private readonly ViewManager _viewManager;

        // коллекция всех картин, для отображения
        private ObservableCollection<PaintingDto> _paintings;
        public ObservableCollection<PaintingDto> Paintings { get => _paintings; set { _paintings = value; OnPropertyChanged(); } }

        // выбранная картина, заполняет форму при выборе
        private PaintingDto _selectedPainting;
        public PaintingDto SelectedPainting
        {
            get => _selectedPainting;
            set
            {
                _selectedPainting = value;
                OnPropertyChanged();

                // заполняем поля формы данными выбранной картины
                if (value != null)
                {
                    Title = value.Title;
                    Artist = value.Artist;
                    Year = value.Year;
                    Genre = value.Genre;
                }
            }
        }

        // свойства полей формы, то что пользователь видит/вводит
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

        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand SearchByYearCommand { get; }
        public ICommand GroupByGenreCommand { get; }
        public ICommand SortAscendingCommand { get; }
        public ICommand SortDescendingCommand { get; }
        public ICommand ClearCommand { get; }

        public MainViewModel(IPaintingService paintingService, ViewManager viewManager)
        {
            _paintingService = paintingService ?? throw new ArgumentNullException(nameof(paintingService));
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));

            // инициализируем коллекцию
            Paintings = new ObservableCollection<PaintingDto>();

            // создаем команды и связываем с методами
            LoadCommand = new RelayCommand(LoadPaintings);
            AddCommand = new RelayCommand(AddPainting);
            DeleteCommand = new RelayCommand(DeletePainting);
            UpdateCommand = new RelayCommand(UpdatePainting);
            SearchByYearCommand = new RelayCommand(SearchByYear);
            GroupByGenreCommand = new RelayCommand(GroupByGenre);
            SortAscendingCommand = new RelayCommand(SortAscending);
            SortDescendingCommand = new RelayCommand(SortDescending);
            ClearCommand = new RelayCommand(ClearForm);

            // загружаем картины при старте
            LoadPaintings();

        }

        /// <summary>
        /// Загружает все картины из БД
        /// </summary>
        private void LoadPaintings()
        {
            try
            {
                var paintings = _paintingService.GetAllPaintings();
                Paintings = new ObservableCollection<PaintingDto>(
                    paintings.Select(p => new PaintingDto
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Artist = p.Artist,
                        Year = p.Year,
                        Genre = p.Genre
                    }).ToList()
                );
                // ObservableCollection САМ подпишется на INPC всех элементов
            }
            catch (Exception ex)
            {
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

                var newPainting = _paintingService.GetPainting(Title, Artist);

                if (newPainting != null)
                {
                    Paintings.Add(new PaintingDto
                    {
                        Id = newPainting.Id,
                        Title = newPainting.Title,
                        Artist = newPainting.Artist,
                        Year = newPainting.Year,
                        Genre = newPainting.Genre
                    });
                }

                ClearForm();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("уже существует"))
            {
                System.Diagnostics.Debug.WriteLine("Картина уже существует!");
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
                // сохраняем ссылку на удаляемый элемент
                var paintingToDelete = SelectedPainting;

                if (_paintingService.DeletePainting(
                    paintingToDelete.Title,
                    paintingToDelete.Artist))
                {
                    // находим DTO в коллекции
                    var dtoToRemove = Paintings.FirstOrDefault(p =>
                        p.Title == paintingToDelete.Title &&
                        p.Artist == paintingToDelete.Artist);

                    if (dtoToRemove != null)
                    {
                        Paintings.Remove(dtoToRemove);
                        
                    }

                    ClearForm();
                    SelectedPainting = null;
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
                // сохраняем старые значения для поиска в списке
                var oldTitle = SelectedPainting.Title;
                var oldArtist = SelectedPainting.Artist;

                if (_paintingService.UpdatePainting(
                    oldTitle,
                    oldArtist,
                    Title,
                    Artist,
                    Year,
                    Genre))
                {
                    // находим dto в коллекции
                    var dtoToUpdate = Paintings.FirstOrDefault(p =>
                        p.Title == oldTitle &&
                        p.Artist == oldArtist);

                    if (dtoToUpdate != null)
                    {
                        // обновляем только этот dto
                        dtoToUpdate.Title = Title;
                        dtoToUpdate.Artist = Artist;
                        dtoToUpdate.Year = Year;
                        dtoToUpdate.Genre = Genre;

                        // INPC В PaintingDto СРАБОТАЕТ!
                        // ObservableCollection УСЛЫШИТ И ОБНОВИТ ТОЛЬКО ЭТУ СТРОКУ В LISTVIEW
                    }

                    ClearForm();
                    SelectedPainting = null;
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
                Paintings = new ObservableCollection<PaintingDto>(
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
        private void GroupByGenre()
        {
            try
            {
                var groupedViewModel = new GroupedViewModel(_paintingService);
                _viewManager.ShowDialog(groupedViewModel);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка открытия группировки: {ex.Message}");

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
                Paintings = new ObservableCollection<PaintingDto>(
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
                Paintings = new ObservableCollection<PaintingDto>(
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
            //SelectedPainting = null;
        }

    }
}
