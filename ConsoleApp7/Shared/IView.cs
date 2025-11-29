using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IView
    {
        event Action FormLoaded;                    // событие загрузкип формы,консоли
                                                                            //событие при нажатии:
        event Action<string, string, int, string> AddPaintingRequested;     // кнопочка добавки картины
        event Action<string, string> DeletePaintingRequested;               // кнопка удаления картины 
        event Action<string, string, string, string, int, string> UpdatePaintingRequested; // кнопка изменить
        event Action<int, int> SearchByYearRangeRequested;                  // кнопка найти по году
        event Action GroupByGenreRequested;                                 // кнопка по жанру сортировку
        event Action SortByTitleAscendingRequested;                         // кнопка в алфавитном порядке сортировать
        event Action SortByTitleDescendingRequested;                        // кнопка в обратном порядке сортировать
        event Action ClearInputsRequested;                                  // кнопка очистки
        event Action<PaintingDto> PaintingSelected;

        void DisplayPaintings(List<PaintingDto> paintings);
        void ShowError(string message);
        void ShowMessage(string message);
        void ClearInputs();
        void SetSelectedPainting(PaintingDto painting);
    }
}
