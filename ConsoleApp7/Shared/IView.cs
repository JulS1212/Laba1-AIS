// IView.cs - если удалили, нужно восстановить
using Shared;
using System;
using System.Collections.Generic;

namespace Shared
{
    public interface IView
    {
        void DisplayPaintings(List<PaintingDto> paintings);
        void ShowError(string message);
        void ShowMessage(string message);
        void ClearInputs();
    }
}