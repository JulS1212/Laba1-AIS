using BusinessLogical.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class GroupedViewModel : BaseViewModel
    {
        private readonly IPaintingService _paintingService;

        // Коллекция групп (BindingList как в задании)
        private BindingList<GenreGroup> _genreGroups;
        public BindingList<GenreGroup> GenreGroups
        {
            get => _genreGroups;
            set
            {
                _genreGroups = value;
                OnPropertyChanged();
            }
        }

        public GroupedViewModel(IPaintingService paintingService)
        {
            _paintingService = paintingService ?? throw new ArgumentNullException(nameof(paintingService));
            GenreGroups = new BindingList<GenreGroup>();
            LoadGroups();
        }

        private void LoadGroups()
        {
            try
            {
                var grouped = _paintingService.GroupByGenre();

                foreach (var group in grouped.OrderBy(g => g.Key))
                {
                    var genreGroup = new GenreGroup
                    {
                        GenreName = group.Key,
                        Paintings = new BindingList<PaintingDto>(
                            group.Value.Select(p => new PaintingDto
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Artist = p.Artist,
                                Year = p.Year,
                                Genre = p.Genre
                            }).ToList()
                        )
                    };

                    GenreGroups.Add(genreGroup);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки групп: {ex.Message}");
            }
        }
    }

    // Класс для представления группы
    public class GenreGroup : INotifyPropertyChanged
    {
        private string _genreName;
        private BindingList<PaintingDto> _paintings;

        public string GenreName
        {
            get => _genreName;
            set
            {
                _genreName = value;
                OnPropertyChanged();
            }
        }

        public BindingList<PaintingDto> Paintings
        {
            get => _paintings;
            set
            {
                _paintings = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
