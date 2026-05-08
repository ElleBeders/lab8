using System;
using System.IO;

namespace CafeApp
{
    public static class InputValidator
    {
        private const int MAX_ATTEMPTS = 3;

        public static bool CheckFileExists(string filePath, out CafeManager manager)
        {
            manager = new CafeManager(filePath);

            if (manager.CheckFileExists())
            {
                manager.LoadFromFile();
                return true;
            }

            Console.WriteLine("Файл базы данных не найден.");

            for (int attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                Console.WriteLine("1 - Создать новый файл");
                Console.WriteLine("0 - Выйти");
                Console.Write("Ваш выбор: ");
                string answer = Console.ReadLine();

                if (answer == "1")
                {
                    manager.CreateNewDatabase();
                    Console.WriteLine("Новая база данных создана.");
                    return true;
                }
                else if (answer == "0")
                {
                    Console.WriteLine("Программа завершает работу.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine($"Неверный ввод. Осталось попыток: {MAX_ATTEMPTS - attempt}");
                }
            }

            Console.WriteLine("Превышено количество попыток. Программа завершает работу.");
            Console.ReadKey();
            Environment.Exit(0);
            return false;
        }

        public static int GetValidAction(string prompt, int min, int max, bool endless)
        {
            int attempts = 0;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int result) && result >= min && result <= max)
                {
                    return result;
                }

                attempts++;
                if (!endless && attempts >= MAX_ATTEMPTS)
                {
                    Console.WriteLine("Превышено количество попыток. Операция отменена.");
                    return -1;
                }

                Console.WriteLine($"Неверный ввод. Ожидается число от {min} до {max}.");
                if (!endless)
                {
                    Console.WriteLine($"Осталось попыток: {MAX_ATTEMPTS - attempts}");
                }
            }
        }

        public static int GetValidDishNumber(string prompt, int maxDishCount)
        {
            for (int attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int number) && number >= 0 && number <= maxDishCount)
                {
                    return number;
                }

                Console.WriteLine($"Неверный ввод. Ожидается число от 0 до {maxDishCount}.");
                Console.WriteLine($"Осталось попыток: {MAX_ATTEMPTS - attempt}");
            }

            Console.WriteLine("Превышено количество попыток. Возврат в меню действий.");
            return 0;
        }

        public static bool TryGetValidName(string prompt, out string result)
        {
            result = "";

            for (int attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    result = input.Trim();
                    return true;
                }

                Console.WriteLine("Название не может быть пустым.");
                Console.WriteLine($"Осталось попыток: {MAX_ATTEMPTS - attempt}");
            }

            Console.WriteLine("Превышено количество попыток. Добавление отменено.");
            return false;
        }

        public static bool TryGetValidPrice(string prompt, out float result)
        {
            result = 0;

            for (int attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (float.TryParse(input, out result) && result > 0)
                {
                    return true;
                }

                Console.WriteLine("Цена должна быть положительным числом.");
                Console.WriteLine($"Осталось попыток: {MAX_ATTEMPTS - attempt}");
            }

            Console.WriteLine("Превышено количество попыток. Добавление отменено.");
            return false;
        }

        public static bool TryGetValidCalories(string prompt, out int result)
        {
            result = 0;

            for (int attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out result) && result >= 0)
                {
                    return true;
                }

                Console.WriteLine("Калории должны быть неотрицательным целым числом.");
                Console.WriteLine($"Осталось попыток: {MAX_ATTEMPTS - attempt}");
            }

            Console.WriteLine("Превышено количество попыток. Добавление отменено.");
            return false;
        }

        public static bool TryGetValidSpiciness(string prompt, out int result)
        {
            result = 0;

            for (int attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out result) && result >= 0 && result <= 10)
                {
                    return true;
                }

                Console.WriteLine("Острота должна быть целым числом от 0 до 10.");
                Console.WriteLine($"Осталось попыток: {MAX_ATTEMPTS - attempt}");
            }

            Console.WriteLine("Превышено количество попыток. Добавление отменено.");
            return false;
        }

        public static bool TryGetValidAvailability(string prompt, out bool result)
        {
            result = false;

            for (int attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                Console.Write(prompt);
                string input = Console.ReadLine().ToLower().Trim();

                if (input == "true" || input == "1")
                {
                    result = true;
                    return true;
                }
                else if (input == "false" || input == "0")
                {
                    result = false;
                    return true;
                }

                Console.WriteLine("Введите true/false или 1/0.");
                Console.WriteLine($"Осталось попыток: {MAX_ATTEMPTS - attempt}");
            }

            Console.WriteLine("Превышено количество попыток. Добавление отменено.");
            return false;
        }
    }
}