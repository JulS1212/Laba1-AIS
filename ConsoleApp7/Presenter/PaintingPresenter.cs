using BusinessLogical.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class PaintingPresenter
    {
        private readonly IView _view;
        private readonly IPaintingService _paintingService;

        public PaintingPresenter(IView view, IPaintingService paintingService)
        {
            _view = view;
            _paintingService = paintingService; // ← Presenter работает с бизнес-логикой через интерфейс

            // ПОДПИСКА НА СОБЫТИЯ VIEW (когда пользователь что-то делает)
            _view.FormLoaded += OnFormLoaded;                           // Форма загрузилась
            _view.AddPaintingRequested += OnAddPainting;               // Нажали "Добавить картину"
            _view.DeletePaintingRequested += OnDeletePainting;         // Нажали "Удалить картину"
            _view.UpdatePaintingRequested += OnUpdatePainting;         // Нажали "Изменить"
            _view.SearchByYearRangeRequested += OnSearchByYearRange;   // Нажали "Найти" (по годам)
            _view.GroupByGenreRequested += OnGroupByGenre;             // Нажали "По жанрам"
            _view.SortByTitleAscendingRequested += OnSortByTitleAscending;     // Нажали "В алфавитном"
            _view.SortByTitleDescendingRequested += OnSortByTitleDescending;   // Нажали "В обратном алфавитном"
            _view.ClearInputsRequested += OnClearInputs;               // Нажали "Очистить"

            _view.PaintingSelected += OnPaintingSelected;
        }

        private void OnPaintingSelected(PaintingDto selectedPainting)
        {
            // Presenter решает что делать с выбранной картиной:
            _view.SetSelectedPainting(selectedPainting);
        }

        // ОБРАБОТЧИК: Когда форма загрузилась
        private void OnFormLoaded()
        {
            try
            {
                // 1. Получаем все картины через бизнес-логику
                var paintings = _paintingService.GetAllPaintings();

                // 2. Преобразуем доменные объекты в DTO
                var paintingDtos = paintings.Select(p => new PaintingDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Artist = p.Artist,
                    Year = p.Year,
                    Genre = p.Genre
                }).ToList();

                // 3. Просим View отобразить картины
                _view.DisplayPaintings(paintingDtos);
            }
            catch (Exception ex)
            {
                // Если ошибка - показываем сообщение
                _view.ShowError($"Ошибка при загрузке картин: {ex.Message}");
            }
        }

        // ОБРАБОТЧИК: Когда нажали "Добавить картину"
        private void OnAddPainting(string title, string artist, int year, string genre)
        {
            try
            {
                // 1. Вызываем бизнес-логику для добавления
                _paintingService.AddPainting(title, artist, year, genre);

                // 2. Показываем сообщение об успехе
                _view.ShowMessage("Картина успешно добавлена!");

                // 3. Очищаем поля ввода
                _view.ClearInputs();

                // 4. Обновляем список картин (показываем новую)
                OnFormLoaded();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при добавлении: {ex.Message}");
            }
        }

        // ОБРАБОТЧИК: Когда нажали "Удалить картину"
        private void OnDeletePainting(string title, string artist)
        {
            try
            {
                // Пытаемся удалить картину
                if (_paintingService.DeletePainting(title, artist))
                {
                    // Если удалено - сообщение и обновление
                    _view.ShowMessage("Картина успешно удалена!");
                    OnFormLoaded();
                }
                else
                {
                    // Если не найдено - ошибка
                    _view.ShowError("Картина не найдена!");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при удалении: {ex.Message}");
            }
        }

        // ОБРАБОТЧИК: Когда нажали "Изменить"
        private void OnUpdatePainting(string oldTitle, string oldArtist,
                                     string newTitle, string newArtist,
                                     int newYear, string newGenre)
        {
            try
            {
                // Пытаемся обновить картину
                if (_paintingService.UpdatePainting(oldTitle, oldArtist,
                                                   newTitle, newArtist,
                                                   newYear, newGenre))
                {
                    _view.ShowMessage("Картина успешно обновлена!");
                    _view.ClearInputs();
                    OnFormLoaded();
                }
                else
                {
                    _view.ShowError("Картина не найдена!");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при обновлении: {ex.Message}");
            }
        }

        // ОБРАБОТЧИК: Когда нажали "Найти" (по диапазону лет)
        private void OnSearchByYearRange(int startYear, int endYear)
        {
            try
            {
                // 1. Получаем картины по диапазону лет
                var paintings = _paintingService.GetPaintingsByYearRange(startYear, endYear);

                // 2. Преобразуем в DTO
                var paintingDtos = paintings.Select(p => new PaintingDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Artist = p.Artist,
                    Year = p.Year,
                    Genre = p.Genre
                }).ToList();

                // 3. Показываем результат
                _view.DisplayPaintings(paintingDtos);
                _view.ShowMessage($"Найдено {paintings.Count} картин");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при поиске: {ex.Message}");
            }
        }

        // ОБРАБОТЧИК: Когда нажали "По жанрам"
        private void OnGroupByGenre()
        {
            try
            {
                var grouped = _paintingService.GroupByGenre();

                string result = "=== КАРТИНЫ ПО ЖАНРАМ ===\n\n";

                foreach (var genreGroup in grouped)
                {
                    result += $"{genreGroup.Key.ToUpper()} ({genreGroup.Value.Count} картин):\n";

                    foreach (var painting in genreGroup.Value)
                    {
                        result += $"   • {painting.Title} - {painting.Artist} ({painting.Year})\n";
                    }
                    result += "\n";
                }

                _view.ShowMessage(result);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при группировке: {ex.Message}");
            }
        }

        // ОБРАБОТЧИК: Когда нажали "В алфавитном"
        private void OnSortByTitleAscending()
        {
            try
            {
                // 1. Получаем отсортированный список
                var paintings = _paintingService.SortByTitleAscending();

                // 2. Преобразуем в DTO
                var paintingDtos = paintings.Select(p => new PaintingDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Artist = p.Artist,
                    Year = p.Year,
                    Genre = p.Genre
                }).ToList();

                // 3. Показываем результат
                _view.DisplayPaintings(paintingDtos);
                _view.ShowMessage("Сортировка по возрастанию выполнена!");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при сортировке: {ex.Message}");
            }
        }

        // ОБРАБОТЧИК: Когда нажали "В обратном алфавитном"
        private void OnSortByTitleDescending()
        {
            try
            {
                // Аналогично предыдущему, но сортировка по убыванию
                var paintings = _paintingService.SortByTitleDescending();
                var paintingDtos = paintings.Select(p => new PaintingDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Artist = p.Artist,
                    Year = p.Year,
                    Genre = p.Genre
                }).ToList();

                _view.DisplayPaintings(paintingDtos);
                _view.ShowMessage("Сортировка по убыванию выполнена!");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при сортировке: {ex.Message}");
            }
        }

        // ОБРАБОТЧИК: Когда нажали "Очистить"
        private void OnClearInputs()
        {
            // Просто делегируем очистку View
            _view.ClearInputs();
        }

    }
}
