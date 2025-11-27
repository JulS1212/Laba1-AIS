using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IView
    {
        event Action FormLoaded;                    // Когда форма загрузилась
        event Action<string, string, int, string> AddPaintingRequested;     // Кнопка "Добавить картину"
        event Action<string, string> DeletePaintingRequested;               // Кнопка "Удалить картину"  
        event Action<string, string, string, string, int, string> UpdatePaintingRequested; // Кнопка "Изменить"
        event Action<int, int> SearchByYearRangeRequested;                  // Кнопка "Найти" (по годам)
        event Action GroupByGenreRequested;                                 // Кнопка "По жанрам"
        event Action SortByTitleAscendingRequested;                         // Кнопка "В алфавитном"
        event Action SortByTitleDescendingRequested;                        // Кнопка "В обратном алфавитном"
        event Action ClearInputsRequested;                                  // Кнопка "Очистить"
        event Action<PaintingDto> PaintingSelected;

        void DisplayPaintings(List<PaintingDto> paintings);
        void ShowError(string message);
        void ShowMessage(string message);
        void ClearInputs();
        void SetSelectedPainting(PaintingDto painting);
    }
}
