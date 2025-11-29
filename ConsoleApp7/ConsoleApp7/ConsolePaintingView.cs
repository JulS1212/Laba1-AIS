// ConsoleApp7/ConsolePaintingView.cs
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp7
{
    public class ConsolePaintingView : IView
    {
        // реализуем айвьюшку
        public event Action FormLoaded;
        public event Action<string, string, int, string> AddPaintingRequested;
        public event Action<string, string> DeletePaintingRequested;
        public event Action<string, string, string, string, int, string> UpdatePaintingRequested;
        public event Action<int, int> SearchByYearRangeRequested;
        public event Action GroupByGenreRequested;
        public event Action SortByTitleAscendingRequested;
        public event Action SortByTitleDescendingRequested;
        public event Action ClearInputsRequested;
        public event Action<PaintingDto> PaintingSelected;

        public void DisplayPaintings(List<PaintingDto> paintings)
        {
            Console.WriteLine("\n=== ВСЕ КАРТИНЫ ===");
            if (paintings.Count == 0)
            {
                Console.WriteLine("Коллекция пуста!");
                return;
            }

            foreach (var painting in paintings)
            {
                Console.WriteLine($"{painting.Title} - {painting.Artist} ({painting.Year}), {painting.Genre}");
            }
            Console.WriteLine($"Всего: {paintings.Count} картин");
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ОШИБКА: {message}");
            Console.ResetColor();
        }

        public void ShowMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public void ClearInputs()
        {
            // Для консоли не нужно
        }

        public void SetSelectedPainting(PaintingDto painting)
        {
            Console.WriteLine($"Выбрано: {painting.Title} - {painting.Artist}");
        }

        // для запуска консольки
        public void Start()
        {
            Console.WriteLine("=== КОНСОЛЬНОЕ ПРИЛОЖЕНИЕ MVP ===");

            // событие загрузки
            FormLoaded?.Invoke();
            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine();
                ProcessMenuChoice(choice);
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Показать все картины");
            Console.WriteLine("2. Добавить картину");
            Console.WriteLine("3. Удалить картину");
            Console.WriteLine("4. Найти по годам");
            Console.WriteLine("5. Группировать по жанрам");
            Console.WriteLine("6. Сортировать А-Я");
            Console.WriteLine("7. Сортировать Я-А");
            Console.WriteLine("8. Выход");
            Console.Write("Выберите действие: ");
        }

        private void ProcessMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    FormLoaded?.Invoke();
                    break;

                case "2":
                    AddPaintingFromConsole();
                    break;

                case "3":
                    DeletePaintingFromConsole();
                    break;

                case "4":
                    SearchByYearFromConsole();
                    break;

                case "5":
                    GroupByGenreRequested?.Invoke();
                    break;

                case "6":
                    SortByTitleAscendingRequested?.Invoke();
                    break;

                case "7":
                    SortByTitleDescendingRequested?.Invoke();
                    break;

                case "8":
                    Environment.Exit(0);
                    break;

                default:
                    ShowError("Неверный выбор!");
                    break;
            }
        }

        private void AddPaintingFromConsole()
        {
            Console.Write("Название: ");
            string title = Console.ReadLine();

            Console.Write("Автор: ");
            string artist = Console.ReadLine();

            Console.Write("Год: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                ShowError("Некорректный год!");
                return;
            }

            Console.Write("Жанр: ");
            string genre = Console.ReadLine();

            AddPaintingRequested?.Invoke(title, artist, year, genre);
        }

        private void DeletePaintingFromConsole()
        {
            Console.Write("Название для удаления: ");
            string title = Console.ReadLine();

            Console.Write("Автор для удаления: ");
            string artist = Console.ReadLine();

            DeletePaintingRequested?.Invoke(title, artist);
        }

        private void SearchByYearFromConsole()
        {
            Console.Write("Год ОТ: ");
            if (!int.TryParse(Console.ReadLine(), out int startYear)) return;

            Console.Write("Год ДО: ");
            if (!int.TryParse(Console.ReadLine(), out int endYear)) return;

            SearchByYearRangeRequested?.Invoke(startYear, endYear);
        }
    }
}