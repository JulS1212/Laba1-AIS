using BusinessLogical.Interfaces;
using Model;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Controllers
{
    public class PaintingController
    {
        private readonly IPaintingService _paintingService;
        private IView _currentView;

        public PaintingController(IPaintingService paintingService, IView view)
        {
            _paintingService = paintingService;
            _currentView = view;

            // Подписываемся на изменения всех существующих картин
            SubscribeToExistingPaintings();
        }

        private void SubscribeToExistingPaintings()
        {
            var paintings = _paintingService.GetAllPaintings();
            foreach (var painting in paintings)
            {
                SubscribeToPaintingEvents(painting);//изменения каждой
            }
        }

        private void SubscribeToPaintingEvents(Painting painting)
        {
            painting.PaintingChanged += OnPaintingChanged;
        }

        private void UnsubscribeFromPainting(Painting painting)
        {
            painting.PaintingChanged -= OnPaintingChanged;
        }

        private void OnPaintingChanged(Painting painting)
        {
            // Модель изменилась - обновляем View
            NotifyView();
        }

        private void NotifyView(List<Painting> paintings = null)//GetAllPaintings() → DTO → DisplayPaintings()
        {
            if (_currentView == null) return;

            var paintingsToDisplay = paintings ?? _paintingService.GetAllPaintings().ToList();
            var dtos = ConvertToDtoList(paintingsToDisplay);

            _currentView.DisplayPaintings(dtos);
        }

        public void LoadPaintings()
        {
            NotifyView();
        }

        public void AddPainting(string title, string artist, int year, string genre)
        {
            try
            {
                _paintingService.AddPainting(title, artist, year, genre);

                var newPainting = _paintingService.GetPainting(title, artist);
                if (newPainting != null)
                {
                    SubscribeToPaintingEvents(newPainting);
                }
                NotifyView();
                ShowMessage("Картина добавлена!");
            }
            catch (Exception ex)
            {
                ShowError("Ошибка добавления: " + ex.Message);
            }
        }

        public void DeletePainting(string title, string artist)
        {
            try
            {
                var painting = _paintingService.GetPainting(title, artist);

                if (_paintingService.DeletePainting(title, artist))
                {
                    if (painting != null)
                    {
                        UnsubscribeFromPainting(painting);
                    }
                    NotifyView();
                    ClearInputs();
                    ShowMessage("Картина удалена!");
                }
                else
                {
                    ShowError("Картина не найдена!");
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка удаления: " + ex.Message);
            }
        }

        public void UpdatePainting(string oldTitle, string oldArtist,
                                   string newTitle, string newArtist,
                                   int newYear, string newGenre)
        {
            try
            {
                // Находим картину и обновляем её
                var painting = _paintingService.GetPainting(oldTitle, oldArtist);

                if (painting != null && _paintingService.UpdatePainting(oldTitle, oldArtist,
                    newTitle, newArtist, newYear, newGenre))
                {
                    // Модель сама вызовет PaintingChanged через сеттеры свойств
                    ShowMessage("Картина обновлена!");
                }
                else
                {
                    ShowError("Картина не найдена!");
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка обновления: " + ex.Message);
            }
        }

        public void GroupByGenre()
        {
            try
            {
                var groups = _paintingService.GroupByGenre();

                string msg = "";
                foreach (var g in groups)
                {
                    msg += $"Жанр: {g.Key}\n";
                    foreach (var p in g.Value)
                        msg += $"   - {p.Title} ({p.Artist})\n";
                    msg += "\n";
                }

                if (string.IsNullOrEmpty(msg))
                    msg = "Нет данных для группировки";

                ShowMessage(msg);

                var all = groups.SelectMany(g => g.Value).ToList();
                NotifyView(all);
            }
            catch (Exception ex)
            {
                ShowError("Ошибка группировки: " + ex.Message);
            }
        }

        public void SearchByYearRange(int startYear, int endYear)
        {
            try
            {
                var list = _paintingService.GetPaintingsByYearRange(startYear, endYear).ToList();

                if (list.Count == 0)
                {
                    ShowMessage("Ничего не найдено!");
                }
                else
                {
                    string msg = $"Картины c {startYear} по {endYear}:\n\n";
                    foreach (var p in list)
                        msg += $" - {p.Title} ({p.Year}), автор: {p.Artist}\n";

                    ShowMessage(msg);
                }

                NotifyView(list);
            }
            catch (Exception ex)
            {
                ShowError("Ошибка поиска: " + ex.Message);
            }
        }

        public void SortByTitleAscending()
        {
            try
            {
                var list = _paintingService.SortByTitleAscending().ToList();
                ShowMessage("Отсортировано А–Я");
                NotifyView(list);
            }
            catch (Exception ex)
            {
                ShowError("Ошибка сортировки: " + ex.Message);
            }
        }

        public void SortByTitleDescending()
        {
            try
            {
                var list = _paintingService.SortByTitleDescending().ToList();
                ShowMessage("Отсортировано Я–А");
                NotifyView(list);
            }
            catch (Exception ex)
            {
                ShowError("Ошибка сортировки: " + ex.Message);
            }
        }

        private void ShowMessage(string msg)
        {
            _currentView?.ShowMessage(msg);
        }

        private void ShowError(string msg)
        {
            _currentView?.ShowError(msg);
        }

        private List<PaintingDto> ConvertToDtoList(List<Painting> paintings)
        {
            return paintings.Select(p => new PaintingDto
            {
                Id = p.Id,
                Title = p.Title,
                Artist = p.Artist,
                Year = p.Year,
                Genre = p.Genre
            }).ToList();
        }

        public void ClearInputs()
        {
            _currentView?.ClearInputs();
        }
    }
}