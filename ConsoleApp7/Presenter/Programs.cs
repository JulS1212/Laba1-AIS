using BusinessLogical;
using BusinessLogical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1;
using Ninject;
using System.Windows.Forms;


namespace Presenter
{
    internal class Programs
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("=== ЗАПУСК ПРИЛОЖЕНИЯ MVP ===");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 1. СОЗДАЁМ FORM1
            var view = new Form1();
            Console.WriteLine("Form1 создана");

            // 2. НАСТРАИВАЕМ NINJECT И СЕРВИСЫ
            IKernel ninjectKernel = new StandardKernel(new NinjectConfig());
            IPaintingService paintingService = ninjectKernel.Get<IPaintingService>();
            Console.WriteLine("Сервисы созданы");

            // 3. СОЗДАЁМ PRESENTER И СВЯЗЫВАЕМ
            var presenter = new PaintingPresenter(view, paintingService);
            Console.WriteLine("Presenter создан и подписан на события");

            // 4. ЗАПУСКАЕМ ФОРМУ ЧЕРЕЗ STARTER
            Console.WriteLine("Запускаем Form1 через Starter");
            Starter.StartForm(view);

            Console.WriteLine("Приложение завершено. Нажмите любую клавишу");
            Console.ReadKey();
        }
    }
}
