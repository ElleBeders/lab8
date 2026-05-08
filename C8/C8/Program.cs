using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CafeApp
{
    internal class Program
    {
        private static string _fileFolder;
        private static string _filePath;
        private static CafeManager _manager;
        private static bool _isRunning = true;

        private static void Main(string[] args)
        {
            _fileFolder = @"C:\Users\User\source\repos\CafeApp\Files\";
            _filePath = Path.Combine(_fileFolder, "cafe_menu.bin");

            if (!Directory.Exists(_fileFolder))
            {
                Directory.CreateDirectory(_fileFolder);
            }

            InputValidator.CheckFileExists(_filePath, out _manager);

            Console.WriteLine("Добро пожаловать в программу управления меню кафе!");

            while (_isRunning)
            {
                ShowMainMenu();
                int choice = InputValidator.GetValidAction("Ваш выбор: ", 0, 7, true);

                switch (choice)
                {
                    case 1:
                        ShowShortMenuAction();
                        break;
                    case 2:
                        ShowFullMenuAction();
                        break;
                    case 3:
                        ShowSingleDishAction();
                        break;
                    case 4:
                        EditDishAction();
                        break;
                    case 5:
                        AddDishAction();
                        break;
                    case 6:
                        DeleteDishAction();
                        break;
                    case 7:
                        AnalysisMenuAction();
                        break;
                    case 0:
                        ExitAction();
                        break;
                }
            }
        }

        private static void ShowMainMenu()
        {
            Console.WriteLine("\n--- Действия над меню ---");
            Console.WriteLine("1. Показать короткое меню");
            Console.WriteLine("2. Показать расширенное меню");
            Console.WriteLine("3. Показать подробно одно блюдо");
            Console.WriteLine("4. Редактировать блюдо");
            Console.WriteLine("5. Добавить блюдо");
            Console.WriteLine("6. Удалить блюдо");
            Console.WriteLine("7. Анализ меню");
            Console.WriteLine("0. Выход");
        }

        private static void ShowShortMenuAction()
        {
            _manager.LoadFromFile();
            Console.WriteLine("\n--- Вывод меню (короткий) ---");
            _manager.ShowShortMenu();
        }

        private static void ShowFullMenuAction()
        {
            _manager.LoadFromFile();
            _manager.ShowFullMenu();
        }

        private static void ShowSingleDishAction()
        {
            if (_manager.GetMenuCount() == 0)
            {
                Console.WriteLine("Меню пусто.");
                return;
            }

            Console.WriteLine("\n--- Вывод меню (короткий) ---");
            _manager.ShowShortMenu();
            int dishNumber = InputValidator.GetValidDishNumber(
                "\nВведите номер блюда для подробного просмотра (0 - назад): ",
                _manager.GetMenuCount()
            );

            if (dishNumber == 0)
            {
                Console.WriteLine("Возврат в меню действий.");
                return;
            }

            _manager.ShowSingleFullMenu(dishNumber - 1);
        }

        private static void EditDishAction()
        {
            if (_manager.GetMenuCount() == 0)
            {
                Console.WriteLine("Меню пусто. Нечего редактировать.");
                return;
            }

            Console.WriteLine("\n--- Вывод меню (короткий) ---");
            _manager.ShowShortMenu();
            int dishNumber = InputValidator.GetValidDishNumber(
                "\nВведите номер блюда для редактирования (0 - назад): ",
                _manager.GetMenuCount()
            );

            if (dishNumber == 0)
            {
                Console.WriteLine("Возврат в меню действий.");
                return;
            }

            _manager.EditDishByIndex(dishNumber - 1);
            Console.WriteLine("\n--- Вывод меню (короткий) ---");
            _manager.ShowShortMenu();
        }

        private static void AddDishAction()
        {
            Console.Write("\nВы действительно хотите добавить новое блюдо? (1 - Да, 0 - Нет): ");
            string confirm = Console.ReadLine();
            if (confirm != "1")
            {
                Console.WriteLine("Добавление отменено.");
                return;
            }

            if (!InputValidator.TryGetValidName("Название: ", out string name))
                return;
            if (!InputValidator.TryGetValidPrice("Цена (руб): ", out float price))
                return;
            if (!InputValidator.TryGetValidCalories("Калории: ", out int calories))
                return;
            if (!InputValidator.TryGetValidSpiciness("Острота (0-10): ", out int spiciness))
                return;
            if (!InputValidator.TryGetValidAvailability("В наличии (true/false): ", out bool isAvailable))
                return;

            _manager.AddDishWithParams(name, price, calories, spiciness, isAvailable);
            Console.WriteLine("\n--- Вывод меню (короткий) ---");
            _manager.ShowShortMenu();
        }

        private static void DeleteDishAction()
        {
            if (_manager.GetMenuCount() == 0)
            {
                Console.WriteLine("Меню пусто. Нечего удалять.");
                return;
            }

            Console.WriteLine("\n--- Вывод меню (короткий) ---");
            _manager.ShowShortMenu();
            int dishNumber = InputValidator.GetValidDishNumber(
                "\nВведите номер блюда для удаления (0 - назад): ",
                _manager.GetMenuCount()
            );

            if (dishNumber == 0)
            {
                Console.WriteLine("Возврат в меню действий.");
                return;
            }

            _manager.DeleteDishByIndex(dishNumber - 1);
            Console.WriteLine("\n--- Вывод меню (короткий) ---");
            _manager.ShowShortMenu();
        }

        private static void AnalysisMenuAction()
        {
            if (_manager.GetMenuCount() == 0)
            {
                Console.WriteLine("Меню пусто. Анализ невозможен.");
                return;
            }

            bool inAnalysis = true;
            while (inAnalysis)
            {
                Console.WriteLine("\n--- Анализ меню ---");
                Console.WriteLine("1. Блюда в наличии (по алфавиту)");
                Console.WriteLine("2. Блюда с калорийностью выше средней");
                Console.WriteLine("3. Средняя цена блюда в меню");
                Console.WriteLine("4. Самое острое блюдо");
                Console.WriteLine("0. Назад");
                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAvailableDishes();
                        break;
                    case "2":
                        ShowDishesAboveAverageCalories();
                        break;
                    case "3":
                        ShowAveragePrice();
                        break;
                    case "4":
                        ShowSpiciestDishes();
                        break;
                    case "0":
                        inAnalysis = false;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        private static void ShowAvailableDishes()
        {
            List<Dish> availableDishes = _manager.GetAvailableDishesSortedByName();

            if (availableDishes.Count == 0)
            {
                Console.WriteLine("Нет блюд в наличии.");
                return;
            }

            Console.WriteLine("\n--- Блюда в наличии (по алфавиту) ---");
            for (int i = 0; i < availableDishes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableDishes[i].GetShortInfo()}");
            }
        }

        private static void ShowDishesAboveAverageCalories()
        {
            List<Dish> dishes = _manager.GetDishesAboveAverageCalories();
            double averageCalories = _manager.GetAverageCalories();

            if (dishes.Count == 0)
            {
                Console.WriteLine("Нет блюд с калорийностью выше средней.");
                return;
            }

            Console.WriteLine("\n--- Блюда с калорийностью выше средней ---");
            for (int i = 0; i < dishes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dishes[i].ToString()}");
            }

            Console.WriteLine($"\n* Средняя калорийность блюд: {averageCalories:F2} ккал");
        }

        private static void ShowAveragePrice()
        {
            double averagePrice = _manager.GetAveragePrice();
            Console.WriteLine($"\n* Средняя цена блюда в меню: {averagePrice:F2} руб.");
        }

        private static void ShowSpiciestDishes()
        {
            List<Dish> spiciestDishes = _manager.GetSpiciestDishes();

            if (spiciestDishes.Count == 0)
            {
                Console.WriteLine("\n* Нет острых блюд в меню.");
                return;
            }

            int maxSpiciness = spiciestDishes[0].Spiciness;
            string dishNames = string.Join(", ", spiciestDishes.Select(d => d.Name));

            Console.WriteLine($"\n* Самое острое блюдо: {dishNames} (острота {maxSpiciness})");
        }

        private static void ExitAction()
        {
            Console.WriteLine("Закрытие...");
            Console.ReadKey();
            _isRunning = false;
        }
    }
}