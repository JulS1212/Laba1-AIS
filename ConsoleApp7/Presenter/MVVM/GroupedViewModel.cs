using BusinessLogical.Interfaces;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Presenter
{
    public class GroupedViewModel : BaseViewModel
    {
        private readonly IPaintingService _paintingService;

        // коллекция групп
        private ObservableCollection<GenreGroup>  _genreGroups;
        public ObservableCollection<GenreGroup> GenreGroups
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
            GenreGroups = new ObservableCollection<GenreGroup>();
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
                        Paintings = new ObservableCollection<PaintingDto>(
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

    // класс для представления группы
    public class GenreGroup : INotifyPropertyChanged
    {
        private string _genreName;
        private ObservableCollection<PaintingDto> _paintings;

        public string GenreName
        {
            get => _genreName;
            set
            {
                _genreName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PaintingDto> Paintings
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
