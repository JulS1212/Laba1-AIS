using Controllers;
using Shared;
using System;
using System.Collections.Generic;

namespace ConsoleApp7
{
    public class ConsolePaintingView : IView
    {
        private PaintingController _controller;

       
        // Метод для установки контроллера
        public void SetController(PaintingController controller)
        {
            _controller = controller;
        }

        public void Start()
        {
            Console.WriteLine("=== КОНСОЛЬНОЕ ПРИЛОЖЕНИЕ MVC ===");

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
            try
            {
                switch (choice)
                {
                    case "1":
                        _controller.LoadPaintings();
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
                        _controller.GroupByGenre();
                        break;
                    case "6":
                        _controller.SortByTitleAscending();
                        break;
                    case "7":
                        _controller.SortByTitleDescending();
                        break;
                    case "8":
                       
                        Environment.Exit(0);
                        break;
                    default:
                        ShowError("Неверный выбор!");
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
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

            _controller.AddPainting(title, artist, year, genre);
        }

        private void DeletePaintingFromConsole()
        {
            Console.Write("Название для удаления: ");
            string title = Console.ReadLine();

            Console.Write("Автор для удаления: ");
            string artist = Console.ReadLine();

            _controller.DeletePainting(title, artist);
        }

        private void SearchByYearFromConsole()
        {
            Console.Write("Год ОТ: ");
            if (!int.TryParse(Console.ReadLine(), out int startYear))
            {
                ShowError("Некорректный год!");
                return;
            }

            Console.Write("Год ДО: ");
            if (!int.TryParse(Console.ReadLine(), out int endYear))
            {
                ShowError("Некорректный год!");
                return;
            }

            _controller.SearchByYearRange(startYear, endYear);
        }

        // Реализация IView интерфейса
        public void DisplayPaintings(List<PaintingDto> paintings)
        {
            Console.WriteLine("\n=== КАРТИНЫ ===");
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
            // Для консоли очистка ввода не требуется
            Console.Clear();
        }
    }
}