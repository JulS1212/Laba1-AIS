using System;

namespace Model
{
    public class Painting : IDomainObject
    {
        // ТОНКАЯ МОДЕЛЬ - только данные
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        
        
        public event Action<Painting> PaintingChanged;//делегат события об изменении
        public void UpdateInfo(string title, string artist, int year, string genre)//ну и метод типо сначала свойтва обнавляем и потом уведомляем подписчиков об этом
        {
            Title = title;
            Artist = artist;
            Year = year;
            Genre = genre;

            PaintingChanged?.Invoke(this);
            //? типо если есть подписота то она получает этот обьект пэинтинг точнее ссылку на него
        }
    }
}