using System;
using System.Collections.Generic;
using System.IO;

namespace CafeApp
{
    public class CafeManager
    {
        private List<Dish> _menu;
        private readonly string _filePath;

        public CafeManager(string filePath)
        {
            _filePath = filePath;
            _menu = new List<Dish>();
        }

        public bool CheckFileExists()
        {
            return File.Exists(_filePath);
        }

        public void CreateNewDatabase()
        {
            _menu = new List<Dish>();
            SaveToFile();
        }

        public void LoadFromFile()
        {
            using (BinaryReader reader = new BinaryReader(File.Open(_filePath, FileMode.Open)))
            {
                int count = reader.ReadInt32();
                _menu = new List<Dish>(count);

                for (int i = 0; i < count; i++)
                {
                    int id = reader.ReadInt32();
                    string name = reader.ReadString();
                    float price = reader.ReadSingle();
                    int calories = reader.ReadInt32();
                    int spiciness = reader.ReadInt32();
                    bool isAvailable = reader.ReadBoolean();

                    _menu.Add(new Dish(id, name, price, calories, spiciness, isAvailable));
                }
            }
        }

        public void SaveToFile()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(_filePath, FileMode.Create)))
            {
                writer.Write(_menu.Count);

                foreach (Dish dish in _menu)
                {
                    writer.Write(dish.Id);
                    writer.Write(dish.Name);
                    writer.Write(dish.Price);
                    writer.Write(dish.Calories);
                    writer.Write(dish.Spiciness);
                    writer.Write(dish.IsAvailable);
                }
            }
        }

        public int GetMenuCount()
        {
            return _menu.Count;
        }

        public void ShowShortMenu()
        {
            if (_menu.Count == 0)
            {
                Console.WriteLine("Меню пусто.");
                return;
            }

            for (int i = 0; i < _menu.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_menu[i].GetShortInfo()}");
            }
        }

        public void ShowFullMenu()
        {
            if (_menu.Count == 0)
            {
                Console.WriteLine("Меню пусто.");
                return;
            }

            Console.WriteLine("\n--- Расширенное меню ---");
            for (int i = 0; i < _menu.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_menu[i].ToString()}");
            }
        }

        public void ShowSingleFullMenu(int index)
        {
            if (index < 0 || index >= _menu.Count)
            {
                throw new ArgumentOutOfRangeException("Неверный номер блюда.");
            }

            Console.WriteLine($"\n--- Подробная информация о блюде \"{_menu[index].GetShortInfo()}\" ---");
            Console.WriteLine(_menu[index].ToString());
        }

        public void AddDishWithParams(string name, float price, int calories, int spiciness, bool isAvailable)
        {
            Dish newDish = new Dish(name, price, calories, spiciness, isAvailable);
            _menu.Add(newDish);
            SaveToFile();
            Console.WriteLine($"Блюдо \"{name}\" добавлено с ID = {newDish.Id}");
        }

        public void DeleteDishByIndex(int index)
        {
            if (index < 0 || index >= _menu.Count)
            {
                throw new ArgumentOutOfRangeException("Неверный номер блюда.");
            }

            Dish dish = _menu[index];
            Console.Write($"Вы действительно хотите удалить \"{dish.Name}\"? (1 - Да, 0 - Нет): ");
            string answer = Console.ReadLine();

            if (answer == "1")
            {
                _menu.RemoveAt(index);
                SaveToFile();
                Console.WriteLine($"Блюдо \"{dish.Name}\" удалено.");
            }
            else
            {
                Console.WriteLine("Удаление отменено.");
            }
        }

        public void EditDishByIndex(int index)
        {
            if (index < 0 || index >= _menu.Count)
            {
                throw new ArgumentOutOfRangeException("Неверный номер блюда.");
            }

            Dish dish = _menu[index];
            bool editing = true;

            while (editing)
            {
                Console.WriteLine($"\n--- Редактирование блюда \"{dish.Name}\" ---");
                Console.WriteLine("1. Название: " + dish.Name);
                Console.WriteLine("2. Цена: " + dish.Price);
                Console.WriteLine("3. Калории: " + dish.Calories);
                Console.WriteLine("4. Острота (0-10): " + dish.Spiciness);
                Console.WriteLine("5. В наличии (true/false): " + dish.IsAvailable);
                Console.WriteLine("0. Завершить редактирование");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Новое название: ");
                        dish.Name = Console.ReadLine();
                        break;
                    case "2":
                        Console.Write("Новая цена: ");
                        dish.Price = float.Parse(Console.ReadLine());
                        break;
                    case "3":
                        Console.Write("Новые калории: ");
                        dish.Calories = int.Parse(Console.ReadLine());
                        break;
                    case "4":
                        Console.Write("Новая острота (0-10): ");
                        dish.Spiciness = int.Parse(Console.ReadLine());
                        break;
                    case "5":
                        Console.Write("В наличии (true/false): ");
                        dish.IsAvailable = bool.Parse(Console.ReadLine());
                        break;
                    case "0":
                        editing = false;
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
            SaveToFile();
            Console.WriteLine($"Блюдо \"{dish.Name}\" обновлено.");
        }
        public List<Dish> GetAvailableDishesSortedByName()
        {
            return _menu
                .Where(d => d.IsAvailable)
                .OrderBy(d => d.Name)
                .ToList();
        }
        public List<Dish> GetDishesAboveAverageCalories()
        {
            double averageCalories = _menu.Average(d => d.Calories);
            return _menu
                .Where(d => d.Calories > averageCalories)
                .ToList();
        }
        public double GetAveragePrice()
        {
            return _menu.Average(d => d.Price);
        }
        public List<Dish> GetSpiciestDishes()
        {
            int maxSpiciness = _menu.Max(d => d.Spiciness);

            if (maxSpiciness == 0)
            {
                return new List<Dish>();
            }

            return _menu
                .Where(d => d.Spiciness == maxSpiciness)
                .ToList();
        }
        public double GetAverageCalories()
        {
            return _menu.Average(d => d.Calories);
        }
    }
}