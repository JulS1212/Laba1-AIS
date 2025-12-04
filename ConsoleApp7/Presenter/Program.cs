using BusinessLogical;
using BusinessLogical.Interfaces;
using ConsoleApp7;
using Controllers;
using Ninject;
using System;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace Presenter
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            IKernel ninjectKernel = new StandardKernel(new NinjectConfig());
            //создает ВСЮ цепочку зависимостей
            //// IPaintingService → PaintingService → PaintingRepository → DapperRepository → БД
            IPaintingService paintingService = ninjectKernel.Get<IPaintingService>();
            //"Так, нужен IPaintingService... это PaintingService"

            while (true)
            {
                Console.WriteLine("=== ВЫБЕРИТЕ ИНТЕРФЕЙС ===");
                Console.WriteLine("1. Винформы");
                Console.WriteLine("2. Консоль");
                Console.Write("Ваш выбор: ");

                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    LaunchWindowsForms(paintingService);
                    break;
                }
                else if (choice == "2")
                {
                    LaunchConsole(paintingService);
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Неверный выбор! Введите 1 или 2.");
                }
            }
        }

        private static void LaunchWindowsForms(IPaintingService paintingService)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создаем Form без контроллера
            var form = new Form1();

            // Создаем Controller с передачей формы как IView
            var controller = new PaintingController(paintingService, form);

            // Передаем контроллер обратно во View
            form.SetController(controller);

            Application.Run(form);
        }

        private static void LaunchConsole(IPaintingService paintingService)
        {
            Console.Clear();

            // Создаём Console View
            var consoleView = new ConsolePaintingView();

            // Создаём Controller и передаём ему View
            var controller = new PaintingController(paintingService, consoleView);

            // Передаём контроллер обратно во View
            consoleView.SetController(controller);

            // Запускаем View
            consoleView.Start();
        }
    }
}