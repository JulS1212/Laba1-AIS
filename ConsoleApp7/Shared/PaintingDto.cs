using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PaintingDto : INotifyPropertyChanged // Интерфейс из .NET для уведомлений об изменении свойств.
    {
        private int _id;
        private string _title;
        private string _artist;
        private int _year;
        private string _genre;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Artist
        {
            get => _artist;
            set { _artist = value; OnPropertyChanged(); }
        }

        public int Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); }
        }

        public string Genre
        {
            get => _genre;
            set { _genre = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {                                                                                       //virtual - можно переопределить в наследниках
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));          //[CallerMemberName] (атрибут) - set { _title = value; OnPropertyChanged(); } // Автоматически подставится "Title"
        }
    }                                                                                           //  string propertyName = null - чтобы вызывать без параметра
}
