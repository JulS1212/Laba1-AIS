using BusinessLogical;
using BusinessLogical.Interfaces;
using ConsoleApp7;
using Ninject;
using Shared;
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
            IPaintingService paintingService = ninjectKernel.Get<IPaintingService>();

            while (true)
            {
                Console.WriteLine("=== ВЫБЕРИТЕ ИНТЕРФЕЙС ===");
                Console.WriteLine("1. винформочка");
                Console.WriteLine("2. консолечка");
                Console.Write("Ваш выбор 1 или 2: ");

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
                    Console.WriteLine("Неверный выбор! Пожалуйста, введите 1 или 2.");
                    Console.WriteLine(); 
                }
            }
        }


        private static void LaunchWindowsForms(IPaintingService paintingService)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var view = new Form1();
            var presenter = new PaintingPresenter(view, paintingService);

            Console.WriteLine("Запуск Windows Forms...");
            Application.Run(view);
        }

        private static void LaunchConsole(IPaintingService paintingService)
        {
            var view = new ConsolePaintingView();
            var presenter = new PaintingPresenter(view, paintingService);

            Console.Clear();
            view.Start();
        }
    }
}