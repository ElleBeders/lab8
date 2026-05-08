using System;

namespace CafeApp
{
    public class Dish
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Calories { get; set; }
        public int Spiciness { get; set; }
        public bool IsAvailable { get; set; }

        public Dish(string name, float price, int calories, int spiciness, bool isAvailable)
        {
            Id = _nextId++;
            Name = name;
            Price = price;
            Calories = calories;
            Spiciness = spiciness;
            IsAvailable = isAvailable;
        }

        public Dish(int id, string name, float price, int calories, int spiciness, bool isAvailable)
        {
            Id = id;
            Name = name;
            Price = price;
            Calories = calories;
            Spiciness = spiciness;
            IsAvailable = isAvailable;
            if (id >= _nextId) _nextId = id + 1;
        }

        public override string ToString()
        {
            int maxNameWidth = 20;
            string shortName = Name;

            if (shortName.Length > maxNameWidth)
            {
                shortName = shortName.Substring(0, maxNameWidth - 3) + "...";
            }

            return $" {shortName,-20} {Price,6} руб.  {Calories,4} ккал  Острота: {Spiciness,2}   {(IsAvailable ? "В наличии" : "Нет в наличии"),-12}";
        }//{Id,-4}//

        public string GetShortInfo()
        {
            return Name;
        }
    }
}