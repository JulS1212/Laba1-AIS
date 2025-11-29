using BusinessLogical.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Presenter
{
    public class PaintingPresenter
    {
        private readonly IView _view;
        private readonly IPaintingService _paintingService;

        public PaintingPresenter(IView view, IPaintingService paintingService)
        {
            _view = view;
            _paintingService = paintingService;

            // подписочка на события айвьюшечки
            _view.FormLoaded += OnFormLoaded;
            _view.AddPaintingRequested += OnAddPainting;
            _view.DeletePaintingRequested += OnDeletePainting;
            _view.UpdatePaintingRequested += OnUpdatePainting;
            _view.SearchByYearRangeRequested += OnSearchByYearRange;
            _view.GroupByGenreRequested += OnGroupByGenre;
            _view.SortByTitleAscendingRequested += OnSortByTitleAscending;
            _view.SortByTitleDescendingRequested += OnSortByTitleDescending;
            _view.ClearInputsRequested += OnClearInputs;
            _view.PaintingSelected += OnPaintingSelected;
        }

        private void OnPaintingSelected(PaintingDto selectedPainting)
        {
            _view.SetSelectedPainting(selectedPainting);
        }

        private void OnFormLoaded()
        {
            try
            {
                var paintings = _paintingService.GetAllPaintings();
                var paintingDtos = paintings.Select(p => new PaintingDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Artist = p.Artist,
                    Year = p.Year,
                    Genre = p.Genre
                }).ToList();

                _view.DisplayPaintings(paintingDtos);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при загрузке картин: {ex.Message}");
            }
        }

        private void OnAddPainting(string title, string artist, int year, string genre)
        {
            try
            {
                _paintingService.AddPainting(title, artist, year, genre);
                _view.ShowMessage("Картина успешно добавлена!");
                _view.ClearInputs();
                OnFormLoaded();
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при добавлении: {ex.Message}");
            }
        }

        private void OnDeletePainting(string title, string artist)
        {
            try
            {
                if (_paintingService.DeletePainting(title, artist))
                {
                    _view.ShowMessage("Картина успешно удалена!");
                    OnFormLoaded();
                }
                else
                {
                    _view.ShowError("Картина не найдена!");
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при удалении: {ex.Message}");
            }
        }

        private void OnUpdatePainting(string oldTitle, string oldArtist,
                                     string newTitle, string newArtist,
                                     int newYear, string newGenre)
        {
            try
            {
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

        private void OnSearchByYearRange(int startYear, int endYear)
        {
            try
            {
                var paintings = _paintingService.GetPaintingsByYearRange(startYear, endYear);
                var paintingDtos = paintings.Select(p => new PaintingDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Artist = p.Artist,
                    Year = p.Year,
                    Genre = p.Genre
                }).ToList();

                _view.DisplayPaintings(paintingDtos);
                _view.ShowMessage($"Найдено {paintings.Count} картин");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при поиске: {ex.Message}");
            }
        }

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

        private void OnSortByTitleAscending()
        {
            try
            {
                var paintings = _paintingService.SortByTitleAscending();
                var paintingDtos = paintings.Select(p => new PaintingDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Artist = p.Artist,
                    Year = p.Year,
                    Genre = p.Genre
                }).ToList();

                _view.DisplayPaintings(paintingDtos);
                _view.ShowMessage("Сортировка по возрастанию выполнена!");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при сортировке: {ex.Message}");
            }
        }

        private void OnSortByTitleDescending()
        {
            try
            {
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

        private void OnClearInputs()
        {
            _view.ClearInputs();
        }
    }
}