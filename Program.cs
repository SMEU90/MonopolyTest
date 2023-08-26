using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MonopolyTest.Services;

namespace MonopolyTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int number = 0;
            while (true)
            {
                number = 0;
                // Получение данных
                try
                {
                    Task.Run(async () => await MyMethods.GetDataFromDB()).Wait();
                }
                catch 
                {
                    Console.WriteLine("Ошибка получения данных с БД\n");

                } 
                Console.Clear();
                Console.WriteLine("Тестовое задание для \"Монополия\" Дегтерев С.О.\n");
                Console.WriteLine("1. Добавить паллет.");
                Console.WriteLine("2. Добавить коробку.");
                Console.WriteLine("3. Показать паллеты.");
                Console.WriteLine("4. Показать коробки.");
                Console.WriteLine("5. Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию " +
                    "срока годности и в каждой группе отсортировать паллеты по весу.");
                Console.WriteLine("6. Три паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.");
                Console.WriteLine("7. Получить данные из БД.");
                Console.WriteLine("0. Выход.");
                Console.WriteLine("\nВведите цифру:");

                if (!Int32.TryParse(Console.ReadLine(), out number))
                    number = 0;

                switch (number)
                {
                    case 0:
                        return;
                    case 1:
                        Task.Run(async () => await MyMethods.AddPallet()).Wait();
                        break; 
                    case 2:
                        Task.Run(async () => await MyMethods.AddBox()).Wait();
                        break;
                    case 3:
                        MyMethods.ShowPallet();
                        break;
                    case 4:
                        MyMethods.ShowBox();
                        break;
                    case 5:
                        MyMethods.GroupByDateSortDateSortWeight();
                        break;
                    case 6:
                        MyMethods.LongestShelfLifeSortVolume();
                        break;
                    case 7:
                        Task.Run(async () => await MyMethods.GetDataFromDB()).Wait();
                        break;
                    default:
                        Console.WriteLine("Ошибка ввода.");
                        Console.ReadKey();
                        break;
                }


            }
        }
    }
}
