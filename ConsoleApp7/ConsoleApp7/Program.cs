using BusinessLogical;
using Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp7
{
    internal class Program
    {
        static Logic logic = new Logic();

        static void Main(string[] args)
        {
            //string currentDir = Environment.CurrentDirectory;
            //Console.WriteLine($"Текущая папка: {currentDir}");
            //Console.WriteLine($"Файл будет здесь: {Path.Combine(currentDir, "paintings.json")}");
            //Console.WriteLine($"Файл существует: {File.Exists(Path.Combine(currentDir, "paintings.json"))}");

            //// ДОБАВЬ ЭТУ СТРОКУ - чтобы увидеть сообщения до очистки экрана
            //Console.WriteLine("Нажмите любую клавишу чтобы продолжить...");
            //Console.ReadKey();

            Console.Title = "Управление коллекцией картин";

            // Главный цикл приложения
            while (true)
            {
                try
                {
                    ShowMainMenu();
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": AddPainting(); break;
                        case "2": ShowAllPaintings(); break;
                        case "3": DeletePainting(); break;
                        case "4": UpdatePainting(); break;
                        case "5": GroupByGenre(); break;
                        case "6": SearchByYearRange(); break;
                        case "7": Environment.Exit(0); break;
                        default:
                            Console.WriteLine("Неверный выбор! Нажмите любую клавишу...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Показать главное меню
        /// </summary>
        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== УПРАВЛЕНИЕ КОЛЛЕКЦИЕЙ КАРТИН ===");
            Console.WriteLine("1. Добавить картину");
            Console.WriteLine("2. Показать все картины");
            Console.WriteLine("3. Удалить картину");
            Console.WriteLine("4. Изменить картину");
            Console.WriteLine("5. Группировать по жанрам");
            Console.WriteLine("6. Поиск по диапазону лет");
            Console.WriteLine("7. Выход");
            Console.Write("Выберите действие: ");
        }

        /// <summary>
        /// Добавление новой картины
        /// </summary>
        static void AddPainting()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОЙ КАРТИНЫ ===");

            try
            {
                Console.Write("Название картины: ");
                string title = Console.ReadLine();

                Console.Write("Автор: ");
                string artist = Console.ReadLine();
                // Проверяем, не существует ли уже такая картина
                if (logic.GetPainting(title, artist) != null)
                {
                    Console.WriteLine("Картина с таким названием уже существует!");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Год создания: ");
                if (!int.TryParse(Console.ReadLine(), out int year) || year < 1000 || year > DateTime.Now.Year)
                {
                    Console.WriteLine("Некорректный год! Должен быть между 1000 и текущим годом.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Жанр: ");
                string genre = Console.ReadLine();

                

                // Добавляем картину через слой логики
                logic.AddPainting(title, artist, year, genre);

                Console.WriteLine("\nКартина успешно добавлена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        ///Показать все картины
        /// </summary>
        static void ShowAllPaintings()
        {
            Console.Clear();
            Console.WriteLine("=== ВСЕ КАРТИНЫ В КОЛЛЕКЦИИ ===");

            var paintings = logic.GetAllPaintings();

            if (paintings.Count == 0)
            {
                Console.WriteLine("Коллекция пуста! Добавьте первую картину.");
            }
            else
            {
                Console.WriteLine($"Найдено картин: {paintings.Count}\n");

                foreach (var painting in paintings)
                {
                    Console.WriteLine($"{painting.Title}");
                    Console.WriteLine($"{painting.Artist}");
                    Console.WriteLine($"{painting.Year}");
                    Console.WriteLine($"{painting.Genre}");
                    Console.WriteLine("   " + new string('─', 40));
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        ///Удаление картины
        /// </summary>
        static void DeletePainting()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ КАРТИНЫ ===");

            Console.Write("Введите название картины для удаления: ");
            string title = Console.ReadLine();

            Console.Write("Введите автора картины для удаления: ");
            string artist = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Название не может быть пустым!");
                Console.ReadKey();
                return;
            }

            if (string.IsNullOrWhiteSpace(artist))
            {
                Console.WriteLine("Автор не может быть пустым!");
                Console.ReadKey();
                return;
            }

            // Проверяем, существует ли картина
            var painting = logic.GetPainting(title,artist);
            if (painting == null)
            {
                Console.WriteLine("Картина с таким названием не найдена!");
                Console.ReadKey();
                return;
            }

            // Показываем информацию о картине для подтверждения
            Console.WriteLine("\nНайдена картина:");
            Console.WriteLine($"Название: {painting.Title}");
            Console.WriteLine($"Автор: {painting.Artist}");
            Console.WriteLine($"Год: {painting.Year}");
            Console.WriteLine($"Жанр: {painting.Genre}");

            Console.Write("\nВы уверены, что хотите удалить эту картину? (д/н): ");
            string confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "д")
            {
                if (logic.DeletePainting(title, artist))
                {
                    Console.WriteLine(" Картина успешно удалена!");
                }
                else
                {
                    Console.WriteLine(" Не удалось удалить картину!");
                }
            }
            else
            {
                Console.WriteLine("Удаление отменено.");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        ///Изменение картины
        /// </summary>
        static void UpdatePainting()
        {
            Console.Clear();
            Console.WriteLine("=== ИЗМЕНЕНИЕ КАРТИНЫ ===");

            Console.Write("Введите название картины для изменения: ");
            string oldTitle = Console.ReadLine();

            Console.Write("Введите автора картины для изменения: ");
            string oldArtist = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(oldTitle))
            {
                Console.WriteLine("Название не может быть пустым!");
                Console.ReadKey();
                return;
            }
            if (string.IsNullOrWhiteSpace(oldArtist))
            {
                Console.WriteLine("Автор не может быть пустым!");
                Console.ReadKey();
                return;
            }

            // Проверяем, существует ли картина
            var oldPainting = logic.GetPainting(oldTitle,oldArtist);
            if (oldPainting == null)
            {
                Console.WriteLine("Картина с таким названием не найдена!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nТекущие данные:");
            Console.WriteLine($"Название: {oldPainting.Title}");
            Console.WriteLine($"Автор: {oldPainting.Artist}");
            Console.WriteLine($"Год: {oldPainting.Year}");
            Console.WriteLine($"Жанр: {oldPainting.Genre}");

            Console.WriteLine("\nВведите новые данные (оставьте пустым, чтобы не менять):");

            Console.Write("Новое название: ");
            string newTitle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newTitle)) newTitle = oldPainting.Title;

            Console.Write("Новый автор: ");
            string newArtist = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newArtist)) newArtist = oldPainting.Artist;

            Console.Write("Новый год: ");
            string yearInput = Console.ReadLine();
            int newYear = string.IsNullOrWhiteSpace(yearInput) ? oldPainting.Year : int.Parse(yearInput);

            Console.Write("Новый жанр: ");
            string newGenre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newGenre)) newGenre = oldPainting.Genre;

            try
            {
                if (logic.UpdatePainting(oldTitle, oldArtist, newTitle, newArtist, newYear, newGenre))
                {
                    Console.WriteLine(" Картина успешно обновлена!");
                }
                else
                {
                    Console.WriteLine(" Не удалось обновить картину!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка при обновлении: {ex.Message}");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        ///Группировка по жанрам
        /// </summary>
        static void GroupByGenre()
        {
            Console.Clear();
            Console.WriteLine("=== ГРУППИРОВКА КАРТИН ПО ЖАНРАМ ===");

            var grouped = logic.GroupByGenre();

            if (grouped.Count == 0)
            {
                Console.WriteLine("Нет картин для группировки!");
            }
            else
            {
                foreach (var genreGroup in grouped)
                {
                    Console.WriteLine($"\nЖАНР: {genreGroup.Key.ToUpper()}");
                    Console.WriteLine($"   Картин в жанре: {genreGroup.Value.Count}");
                    Console.WriteLine("   " + new string('═', 30));

                    foreach (var painting in genreGroup.Value)
                    {
                        Console.WriteLine($"{painting.Title} - {painting.Artist} ({painting.Year})");
                    }
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        /// Поиск по диапазону лет
        /// </summary>
        static void SearchByYearRange()
        {
            Console.Clear();
            Console.WriteLine("=== ПОИСК КАРТИН ПО ДИАПАЗОНУ ЛЕТ ===");

            try
            {
                Console.Write("Год ОТ: ");
                if (!int.TryParse(Console.ReadLine(), out int startYear))
                {
                    Console.WriteLine("Некорректный год!");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Год ДО: ");
                if (!int.TryParse(Console.ReadLine(), out int endYear))
                {
                    Console.WriteLine("Некорректный год!");
                    Console.ReadKey();
                    return;
                }

                if (startYear > endYear)
                {
                    Console.WriteLine("Год 'ОТ' не может быть больше года 'ДО'!");
                    Console.ReadKey();
                    return;
                }

                var paintings = logic.GetPaintingsByYearRange(startYear, endYear);

                Console.WriteLine($"\nНайдено картин с {startYear} по {endYear} год: {paintings.Count}");

                if (paintings.Count > 0)
                {
                    foreach (var painting in paintings)
                    {
                        Console.WriteLine($"\n {painting.Title}");
                        Console.WriteLine($" {painting.Artist}");
                        Console.WriteLine($" {painting.Year} год");
                        Console.WriteLine($" {painting.Genre}");
                    }
                }
                else
                {
                    Console.WriteLine("Картин в указанный период не найдено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}