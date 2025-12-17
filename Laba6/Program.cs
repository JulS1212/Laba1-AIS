using System;
using System.Collections.Generic;
using System.Linq;

namespace Laba6
{
    internal class Program
    {
        private static List<Employee> employees = new List<Employee>();
        private static IBankService sberService = new SberbankService();
        private static IBankService gazpromService = new GazpromService();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("========== УПРАВЛЕНИЕ СОТРУДНИКАМИ ==========");
                Console.WriteLine("1. Добавить нового сотрудника");
                Console.WriteLine("2. Просмотреть всех сотрудников");
                Console.WriteLine("3. Добавить характеристику сотруднику");
                Console.WriteLine("4. Изменить банковский сервис сотруднику");
                Console.WriteLine("5. Удалить сотрудника");
                Console.WriteLine("6. Выход");
                Console.WriteLine("=============================================");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;
                    case "2":
                        ShowAllEmployees();
                        break;
                    case "3":
                        AddCharacteristic();
                        break;
                    case "4":
                        ChangeBankService();
                        break;
                    case "5":
                        DeleteEmployee();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Введите число от 1 до 6.");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddEmployee()
        {
            Console.Clear();
            Console.WriteLine("========== ДОБАВЛЕНИЕ НОВОГО СОТРУДНИКА ==========");

            Console.Write("Введите имя сотрудника: ");
            string name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Имя не может быть пустым. Введите имя сотрудника: ");
                name = Console.ReadLine();
            }

            Console.Write("Введите базовую зарплату: ");
            double salary;
            while (!double.TryParse(Console.ReadLine(), out salary) || salary <= 0)
            {
                Console.Write("Некорректный ввод. Введите положительное число: ");
            }

            string positionChoice;
            while (true)
            {
                Console.WriteLine("\nВыберите должность:");
                Console.WriteLine("1. Учёный");
                Console.WriteLine("2. Инженер");
                Console.WriteLine("3. Менеджер");
                Console.Write("Ваш выбор (1-3): ");

                positionChoice = Console.ReadLine();

                if (positionChoice == "1" || positionChoice == "2" || positionChoice == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите 1, 2 или 3.");
                }
            }

            string bankChoice;
            while (true)
            {
                Console.WriteLine("\nВыберите банковский сервис:");
                Console.WriteLine("1. Сбербанк (комиссия 1%)");
                Console.WriteLine("2. Газпромбанк (комиссия 1.5%)");
                Console.Write("Ваш выбор (1 или 2): ");

                bankChoice = Console.ReadLine();

                if (bankChoice == "1" || bankChoice == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите 1 или 2.");
                }
            }

            IBankService bankService = bankChoice == "2" ? gazpromService : sberService;
            Employee newEmployee = null;

            switch (positionChoice)
            {
                case "1":
                    newEmployee = new Scientist(name, salary, bankService);
                    break;
                case "2":
                    newEmployee = new Engineer(name, salary, bankService);
                    break;
                case "3":
                    newEmployee = new Manager(name, salary, bankService);
                    break;
            }

            employees.Add(newEmployee);
            Console.WriteLine($"\nСотрудник успешно добавлен!");
            Console.WriteLine($"Информация: {newEmployee.GetInfo()}");
            Console.WriteLine($"Зарплата после комиссии: {newEmployee.CalculateSalary():F2}");

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ShowAllEmployees()
        {
            Console.Clear();
            Console.WriteLine("========== СПИСОК ВСЕХ СОТРУДНИКОВ ==========");

            if (employees.Count == 0)
            {
                Console.WriteLine("Сотрудников нет.");
            }
            else
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    var emp = employees[i];
                    Console.WriteLine($"\n--- Сотрудник #{i} ---");
                    Console.WriteLine($"Информация: {emp.GetInfo()}");
                    Console.WriteLine($"Базовая зарплата: {emp.BaseSalary:F2}");
                    Console.WriteLine($"Банковский сервис: {emp.BankService.GetServiceName()}");
                    Console.WriteLine($"Зарплата после комиссии: {emp.CalculateSalary():F2}");
                }
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void AddCharacteristic()
        {
            Console.Clear();
            Console.WriteLine("========== ДОБАВЛЕНИЕ ХАРАКТЕРИСТИКИ ==========");

            if (employees.Count == 0)
            {
                Console.WriteLine("Сотрудников нет. Сначала добавьте сотрудника.");
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите сотрудника (введите номер):");
            for (int i = 0; i < employees.Count; i++)
            {
                Console.WriteLine($"{i}. {employees[i].GetInfo()}");
            }

            int empIndex;
            while (true)
            {
                Console.Write($"\nВведите номер от 0 до {employees.Count - 1}: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out empIndex) && empIndex >= 0 && empIndex < employees.Count)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Ошибка! Введите число от 0 до {employees.Count - 1}.");
                }
            }

            string choice;
            while (true)
            {
                Console.WriteLine("\nВыберите характеристику:");
                Console.WriteLine("1. Учёная степень");
                Console.WriteLine("2. Знание английского (Intermediate)");
                Console.Write("Ваш выбор (1 или 2): ");

                choice = Console.ReadLine();

                if (choice == "1" || choice == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите 1 или 2.");
                }
            }

            Employee employeeToDecorate = employees[empIndex];

            switch (choice)
            {
                case "1":
                    Console.Write("\nВведите область наук: ");
                    string scienceArea = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(scienceArea))
                    {
                        Console.Write("Область наук не может быть пустой: ");
                        scienceArea = Console.ReadLine();
                    }

                    Console.Write("Введите тему диссертации: ");
                    string dissertationTitle = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(dissertationTitle))
                    {
                        Console.Write("Тема диссертации не может быть пустой: ");
                        dissertationTitle = Console.ReadLine();
                    }

                    Console.Write("Введите год защиты: ");
                    int year;
                    while (!int.TryParse(Console.ReadLine(), out year) || year < 1900 || year > DateTime.Now.Year)
                    {
                        Console.Write($"Некорректный ввод. Введите год от 1900 до {DateTime.Now.Year}: ");
                    }

                    var employeeWithDegree = new AcademicDegree(employeeToDecorate, dissertationTitle, year, scienceArea);
                    employees[empIndex] = employeeWithDegree;
                    Console.WriteLine($"\nУчёная степень успешно добавлена!");
                    Console.WriteLine($"Новая информация: {employeeWithDegree.GetInfo()}");
                    break;

                case "2":
                    Console.Write("\nВведите название сертификата: ");
                    string certificateTitle = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(certificateTitle))
                    {
                        Console.Write("Название сертификата не может быть пустым: ");
                        certificateTitle = Console.ReadLine();
                    }

                    Console.Write("Введите год получения сертификата: ");
                    int certYear;
                    while (!int.TryParse(Console.ReadLine(), out certYear) || certYear < 1900 || certYear > DateTime.Now.Year)
                    {
                        Console.Write($"Некорректный ввод. Введите год от 1900 до {DateTime.Now.Year}: ");
                    }

                    var employeeWithEnglish = new IntramediateEnglishSerializer(employeeToDecorate, certificateTitle, certYear);
                    employees[empIndex] = employeeWithEnglish;
                    Console.WriteLine($"\nЗнание английского успешно добавлено!");
                    Console.WriteLine($"Новая информация: {employeeWithEnglish.GetInfo()}");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void ChangeBankService()
        {
            Console.Clear();
            Console.WriteLine("========== ИЗМЕНЕНИЕ БАНКОВСКОГО СЕРВИСА ==========");

            if (employees.Count == 0)
            {
                Console.WriteLine("Сотрудников нет. Сначала добавьте сотрудника.");
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите сотрудника (введите номер):");
            for (int i = 0; i < employees.Count; i++)
            {
                Console.WriteLine($"{i}. {employees[i].GetInfo()}");
                Console.WriteLine($" Текущий банк: {employees[i].BankService.GetServiceName()}");
            }

            int empIndex;
            while (true)
            {
                Console.Write($"\nВведите номер от 0 до {employees.Count - 1}: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out empIndex) && empIndex >= 0 && empIndex < employees.Count)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Ошибка! Введите число от 0 до {employees.Count - 1}.");
                }
            }

            string bankChoice;
            while (true)
            {
                Console.WriteLine("\nВыберите новый банковский сервис:");
                Console.WriteLine("1. Сбербанк (комиссия 1%)");
                Console.WriteLine("2. Газпромбанк (комиссия 1.5%)");
                Console.Write("Ваш выбор (1 или 2): ");

                bankChoice = Console.ReadLine();

                if (bankChoice == "1" || bankChoice == "2")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите 1 или 2.");
                }
            }

            IBankService newService = bankChoice == "2" ? gazpromService : sberService;

            // Получаем текущего сотрудника
            var oldEmployee = employees[empIndex];
            Employee newEmployee;

            if (oldEmployee is EmployeeDecorator decorator)
            {
                // Разбираем цепочку декораторов
                var decorators = new List<EmployeeDecorator>();
                Employee current = decorator;

                // Собираем все декораторы
                while (current is EmployeeDecorator currentDecorator)
                {
                    decorators.Add(currentDecorator);
                    current = currentDecorator._employee; // Теперь это работает!
                }

                // Базовый сотрудник (последний в цепочке)
                var baseEmployee = current;

                // Создаем нового базового сотрудника с новым банком
                Employee newBaseEmployee;
                if (baseEmployee is Scientist)
                    newBaseEmployee = new Scientist(baseEmployee.Name, baseEmployee.BaseSalary, newService);
                else if (baseEmployee is Engineer)
                    newBaseEmployee = new Engineer(baseEmployee.Name, baseEmployee.BaseSalary, newService);
                else if (baseEmployee is Manager)
                    newBaseEmployee = new Manager(baseEmployee.Name, baseEmployee.BaseSalary, newService);
                else
                    newBaseEmployee = new Scientist(baseEmployee.Name, baseEmployee.BaseSalary, newService);

                // Восстанавливаем декораторы в обратном порядке
                newEmployee = newBaseEmployee;
                for (int i = decorators.Count - 1; i >= 0; i--)
                {
                    if (decorators[i] is AcademicDegree degree)
                    {
                        newEmployee = new AcademicDegree(newEmployee, degree.DissertationTitle, degree.Year, degree.ScienceArea);
                    }
                    else if (decorators[i] is IntramediateEnglishSerializer english)
                    {
                        newEmployee = new IntramediateEnglishSerializer(newEmployee, english.ExaminationTitle, english.YearOfSertificate);
                    }
                }
            }
            else
            {
                // Простой сотрудник без декораторов
                if (oldEmployee is Scientist)
                    newEmployee = new Scientist(oldEmployee.Name, oldEmployee.BaseSalary, newService);
                else if (oldEmployee is Engineer)
                    newEmployee = new Engineer(oldEmployee.Name, oldEmployee.BaseSalary, newService);
                else if (oldEmployee is Manager)
                    newEmployee = new Manager(oldEmployee.Name, oldEmployee.BaseSalary, newService);
                else
                    newEmployee = new Scientist(oldEmployee.Name, oldEmployee.BaseSalary, newService);
            }

            employees[empIndex] = newEmployee;
            Console.WriteLine($"\nБанковский сервис успешно изменен!");
            Console.WriteLine($"Новый сервис: {newService.GetServiceName()}");
            Console.WriteLine($"Новая зарплата: {newEmployee.CalculateSalary():F2}");
            Console.WriteLine($"Информация: {newEmployee.GetInfo()}");
            Console.WriteLine($"\nХарактеристики сохранены!");

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void DeleteEmployee()
        {
            Console.Clear();
            Console.WriteLine("========== УДАЛЕНИЕ СОТРУДНИКА ==========");

            if (employees.Count == 0)
            {
                Console.WriteLine("Сотрудников нет.");
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Выберите сотрудника для удаления (введите номер):");
            for (int i = 0; i < employees.Count; i++)
            {
                Console.WriteLine($"{i}. {employees[i].GetInfo()}");
            }

            int empIndex;
            while (true)
            {
                Console.Write($"\nВведите номер от 0 до {employees.Count - 1}: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out empIndex) && empIndex >= 0 && empIndex < employees.Count)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Ошибка! Введите число от 0 до {employees.Count - 1}.");
                }
            }

            string confirmation;
            while (true)
            {
                Console.Write($"\nВы уверены, что хотите удалить сотрудника '{employees[empIndex].GetInfo()}'? (д/н): ");
                confirmation = Console.ReadLine().ToLower();

                if (confirmation == "д" || confirmation == "да" ||
                    confirmation == "н" || confirmation == "нет")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ошибка! Введите 'д' (да) или 'н' (нет).");
                }
            }

            if (confirmation == "д" || confirmation == "да")
            {
                employees.RemoveAt(empIndex);
                Console.WriteLine("Сотрудник успешно удален!");
            }
            else
            {
                Console.WriteLine("Удаление отменено.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}