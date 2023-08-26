using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MonopolyTest.Models;
using MonopolyTest.Models.Base;
using CromulentBisgetti.ContainerPacking;
using CromulentBisgetti.ContainerPacking.Entities;
using CromulentBisgetti.ContainerPacking.Algorithms;
using System.Threading.Tasks;
using System.Globalization;

namespace MonopolyTest.Services
{
    public class MyMethods
    {

        private static List<Box> Boxes;
        private static List<Pallet> Pallets;

        // Number 1
        //  Добавить паллет
        public static async Task AddPallet()
        {
            Console.Clear();
            Pallet pallet = new Pallet();
            double check;
            Console.WriteLine("Введите ширину:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                pallet.Width = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите высоту:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                pallet.Length = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите глубину:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                pallet.Depth = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }

            try
            {
                pallet.Id = 0;
                var result = await ContextDB.GetContext().Pallets.AddAsync(pallet);
                await ContextDB.GetContext().SaveChangesAsync();
                if (Pallets == null)
                    Pallets = new List<Pallet>();
                Pallets.Add(result.Entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine("Ошибка при добавлении в БД!");
                Console.ReadKey();
                return;
            }
        }
        // Number 2
        // Добавить коробку
        public static async Task AddBox()
        {
            Console.Clear();
            Box box = new Box();
            double check;
            DateTime checkDateTime;
            Console.WriteLine("Введите ширину:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                box.Width = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите высоту:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                box.Length = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите глубину:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                box.Depth = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите вес:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                box.Weight = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите дату производства по типу ДД/ММ/ГГГГ:");
            if (DateTime.TryParse(Console.ReadLine(), out checkDateTime))
                box.ProductionDate = checkDateTime;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }

            try
            {
                box.Id = 0;
                var result = await ContextDB.GetContext().Boxes.AddAsync(box);
                await ContextDB.GetContext().SaveChangesAsync();
                if (Boxes == null)
                    Boxes = new List<Box>();
                Boxes.Add(result.Entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine("Ошибка при добавлении в БД!");
                Console.ReadKey();
                return;
            }

        }
        // Number 3
        // Показать паллеты
        public static void ShowPallet()
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("Выберите паллет по его Id для редактирования или удаления. Для возвращения в меню введите цифру 0 ");
                if (Pallets != null && Pallets.Count > 0)
                {
                    foreach (Pallet el in Pallets)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Паллет:");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Id: {el.Id} Ширина: {el.Width} Длина: {el.Length} Глубина: {el.Depth} Вес: {el.Weight} Объем: {el.Volume}");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Коробки:");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (el.Boxes != null && el.Boxes.Count > 0)
                            foreach (Box box in el.Boxes)
                                Console.WriteLine($"    Id: {box.Id} Ширина: {box.Width} Длина: {box.Length} Глубина: " +
                                    $"{box.Depth} Вес: {box.Weight} Объем: {box.Volume} Дата производства: {box.ProductionDate.ToShortDateString()}");
                        else
                            Console.WriteLine("    На паллете нет коробок.");
                    }
                }
                else
                {
                    Console.WriteLine("Паллетов нет!");
                    Console.ReadKey();
                    return;
                }
                int check = 0;
                if (int.TryParse(Console.ReadLine(), NumberStyles.Number, null, out check) && check>=0)
                {
                    if (check == 0)
                        return;
                    if (Pallets.Any(p => p.Id == check))
                    {
                        EditOrDeletePallet(Pallets.First(p => p.Id == check));
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода!");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка ввода!");
                    Console.ReadKey();
                    return;
                }
            }
        }
        // Number 4
        // Показать коробки
        public static void ShowBox()
        {
            Console.Clear();
            Console.WriteLine("Выберите паллет по его Id для редактирования или удаления. Для возвращения в меню введите цифру 0 ");
            if (Boxes != null && Boxes.Count > 0)
            {
                foreach (Box box in Boxes)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Коробка:");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Id: {box.Id} Ширина: {box.Width} Длина: {box.Length} Глубина: " +
                                $"{box.Depth} Вес: {box.Weight} Объем: {box.Volume} Дата производства: {box.ProductionDate.ToShortDateString()}");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Паллет:");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (box.Pallet != null)
                        Console.WriteLine($"    Id: {box.Pallet.Id} Ширина: {box.Pallet.Width} Длина: {box.Pallet.Length} " +
                            $"Глубина: {box.Pallet.Depth} Вес: {box.Pallet.Weight} Объем: {box.Pallet.Volume}");
                    else
                        Console.WriteLine("    Данная коробка не лежит на паллете.");
                }
            }
            else
            {
                Console.WriteLine("Коробок нет!");
                Console.ReadKey();
                return;
            }
            int check;
            if (Int32.TryParse(Console.ReadLine(), out check) && check >= 0)
            {
                if (check == 0)
                    return;
                if (Boxes.Any(p => p.Id == check))
                {
                    EditOrDeleteBox(Boxes.First(p => p.Id == check));
                }
                else
                {
                    Console.WriteLine("Ошибка ввода!");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
        }
        // Number 5
        // Сгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.
        public static void GroupByDateSortDateSortWeight()
        {
            Console.Clear();
            IOrderedEnumerable<IGrouping<DateTime, Pallet>> result = Pallets.OrderBy(p => p.Weight).GroupBy(p => p.ProductionDate).OrderBy(p => p.Key);
            var fff = Pallets.GroupBy(p => p.ProductionDate).Select(el => new { el.Key, Items = el.OrderBy(x => x.ProductionDate) }).OrderBy(p => p.Key);
            if(result != null)
            foreach (var el in result)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Сгруппированные паллеты по сроку годности: {el.Key.ToShortDateString()}");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var pallet in el)
                    Console.WriteLine($"Id: {pallet.Id} Ширина: {pallet.Width} Длина: {pallet.Length} " +
                            $"Глубина: {pallet.Depth} Вес: {pallet.Weight} Объем: {pallet.Volume}");
            }
            Console.ReadKey();
        }
        // Number 6
        // Три паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.
        public static void LongestShelfLifeSortVolume()
        {
            Console.Clear();
            IEnumerable<Pallet> result = Pallets.OrderByDescending(p => p.ProductionDate).ThenBy(p => p.Volume).Take(3);
            foreach (Pallet pallet in result)
            {
                Console.WriteLine($"Id: {pallet.Id} Ширина: {pallet.Width} Длина: {pallet.Length} " +
                            $"Глубина: {pallet.Depth} Вес: {pallet.Weight} Объем: {pallet.Volume}");
            }
            Console.ReadKey();
        }
        // Number 7
        // Получить данные из БД
        public static async Task GetDataFromDB()
        {

            // Получение всех паллетов
            Pallets = new List<Pallet>(await ContextDB.GetContext().Pallets.AsQueryable().ToListAsync());
            // Получение всех коробок
            Boxes = new List<Box>(await ContextDB.GetContext().Boxes.AsQueryable().ToListAsync());
        }
        // Добавить коробку на паллет
        private static void AddBoxOnPallet(Pallet pallet)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Добавьте коробки на паллет по их Id. Если такой необходимости нет, то введите цифру 0");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Коробки:");
                Console.ForegroundColor = ConsoleColor.White;
                if (Boxes != null && Boxes.Count > 0)
                    foreach (Box box in Boxes)
                    {
                        if(box.Pallet==null)
                        Console.WriteLine($"    Id: {box.Id} Ширина: {box.Width} Длина: {box.Length} " +
                                $"Глубина: {box.Depth} Вес: {box.Weight} Объем: {box.Volume}");
                    }
                else
                    Console.WriteLine("Коробок нет!");
                int check;
                if (Int32.TryParse(Console.ReadLine(), out check) && check >= 0)
                {
                    if (check == 0)
                        return;
                    if(Boxes.Any(p => p.Id == check) && Boxes.First(p => p.Id == check).Pallet == null)
                    {
                        Box box = Boxes.First(p => p.Id == check);
                        AlgorithmAddBoxOnPallet(box, pallet);
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка ввода!");
                    Console.ReadKey();
                    return;
                }
            }
        }
        // Алгоритм добавления коробок на паллет
        private static bool AlgorithmAddBoxOnPallet(List<Box> box, Pallet pallet)
        {
            List<Container> container = new List<Container>();
            container.Add(new Container(pallet.Id, (decimal)pallet.Length, (decimal)pallet.Width, (decimal)pallet.Depth));
            List<Item> itemsToPack = new List<Item>();
            foreach(Box el in box)
                itemsToPack.Add(new Item(el.Id, (decimal)el.Length, (decimal)el.Width, (decimal)el.Depth, 1));
            if( pallet.Boxes == null )
                pallet.Boxes = new List<Box>();
            List<int> algorithms = new List<int>();
            algorithms.Add((int)AlgorithmType.EB_AFIT);
            List<ContainerPackingResult> result = PackingService.Pack(container, itemsToPack, algorithms);
            if(result[0].AlgorithmPackingResults[0].UnpackedItems.Count == 0)
            {
                foreach (Box el in box)
                {
                    pallet.Boxes.Add(el);
                    el.PalletId = pallet.Id;
                    el.Pallet = pallet;
                }
                return true;
            }
            else
            {
                Console.WriteLine("Не удалось разместить на паллете");
                Console.ReadKey();
                return false;
            }
        }
        // Алгоритм добавления короби на паллет
        private static bool AlgorithmAddBoxOnPallet(Box box, Pallet pallet)
        {
            List<Container> container = new List<Container>();
            container.Add(new Container(pallet.Id, (decimal)pallet.Length, (decimal)pallet.Width, (decimal)pallet.Depth));
            List<Item> itemsToPack = new List<Item>();
            itemsToPack.Add(new Item(box.Id, (decimal)box.Length, (decimal)box.Width, (decimal)box.Depth, 1));
            if (pallet.Boxes == null)
                pallet.Boxes = new List<Box>();
            else
                foreach (Box el in pallet.Boxes)
                    itemsToPack.Add(new Item(el.Id, (decimal)el.Length, (decimal)el.Width, (decimal)el.Depth, 1));
            List<int> algorithms = new List<int>();
            algorithms.Add((int)AlgorithmType.EB_AFIT);
            List<ContainerPackingResult> result = PackingService.Pack(container, itemsToPack, algorithms);
            if (result[0].AlgorithmPackingResults[0].UnpackedItems.Count == 0)
            {
                pallet.Boxes.Add(box);
                box.PalletId = pallet.Id;
                box.Pallet = pallet;
                return true;
            }
            else
            {
                Console.WriteLine("Не удалось разместить на паллете");
                Console.ReadKey();
                return false;
            }
        }
        // Алгоритм изменения коробоки на паллете
        private static bool AlgorithmEditBoxOnPallet(Box box, Pallet pallet)
        {
            List<Container> container = new List<Container>();
            container.Add(new Container(pallet.Id, (decimal)pallet.Length, (decimal)pallet.Width, (decimal)pallet.Depth));
            List<Item> itemsToPack = new List<Item>();
            itemsToPack.Add(new Item(box.Id, (decimal)box.Length, (decimal)box.Width, (decimal)box.Depth, 1));
            if (pallet.Boxes == null)
                pallet.Boxes = new List<Box>();
            else
                foreach (Box el in pallet.Boxes)
                    if(el.Id != box.Id)
                        itemsToPack.Add(new Item(el.Id, (decimal)el.Length, (decimal)el.Width, (decimal)el.Depth, 1));
            List<int> algorithms = new List<int>();
            algorithms.Add((int)AlgorithmType.EB_AFIT);
            List<ContainerPackingResult> result = PackingService.Pack(container, itemsToPack, algorithms);
            if (result[0].AlgorithmPackingResults[0].UnpackedItems.Count == 0)
                return true;
            else
            {
                Console.WriteLine("Не удалось разместить на паллете");
                Console.ReadKey();
                return false;
            }
        }
        // Меню для редактирования или удаления паллета
        private static void EditOrDeletePallet(Pallet pallet)
        {
            int number = -1;
            while(true)
            {
                number = -1;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Паллет:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Id: {pallet.Id} Ширина: {pallet.Width} Длина: {pallet.Length} Глубина: {pallet.Depth} Вес: {pallet.Weight} Объем: {pallet.Volume}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Коробки:");
                Console.ForegroundColor = ConsoleColor.White;
                if (pallet.Boxes != null && pallet.Boxes.Count > 0)
                    foreach (Box box in pallet.Boxes)
                        Console.WriteLine($"    Id: {box.Id} Ширина: {box.Width} Длина: {box.Length} Глубина: " +
                            $"{box.Depth} Вес: {box.Weight} Объем: {box.Volume} Дата производства: {box.ProductionDate.ToShortDateString()}");
                else
                    Console.WriteLine("    На паллете нет коробок.");
                Console.WriteLine();
                Console.WriteLine("1. Редактировать");
                Console.WriteLine("2. Удалить");
                Console.WriteLine("0. Выход\n");
                if (!Int32.TryParse(Console.ReadLine(), out number))
                    number = -1;
                switch (number)
                {
                    case 0:
                        return;
                    case 1:
                        EditPalletMenu(pallet);
                        break;
                    case 2:
                        Task.Run(async () => await MyMethods.DeletePallet(pallet)).Wait();
                        return;
                }
            }

        }
        // Меню редактирования паллета
        private static void EditPalletMenu(Pallet pallet)
        {
            Console.WriteLine();
            int number = -1;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Редактировать поля паллета");
                Console.WriteLine("2. Добавить коробку");
                Console.WriteLine("3. Удалить коробку");
                Console.WriteLine("0. Выход\n");
                if (!Int32.TryParse(Console.ReadLine(), out number))
                    number = -1;
                switch (number)
                {
                    case 0:
                        return;
                    case 1:
                        Task.Run(async () => await MyMethods.EditPallet(pallet)).Wait();
                        break;
                    case 2:
                        AddBoxOnPallet(pallet);
                        break;
                    case 3:
                        Task.Run(async () => await MyMethods.DeleteBoxOnPallet(pallet)).Wait();
                        break;
                }
            }
        }
        // Редактирования паллета
        private static async Task EditPallet(Pallet palletInitial)
        {
            Console.Clear();
            Pallet pallet = new Pallet
            {
                Weight = 30.0
            };
            double check;
            Console.WriteLine("Введите ширину:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                pallet.Width = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите высоту:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                pallet.Length = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите глубину:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                pallet.Depth = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            if(pallet.Length == palletInitial.Length && pallet.Width == palletInitial.Width && pallet.Depth == palletInitial.Depth)
            {
                Console.WriteLine("Вы не изменили ни одного поля!");
                Console.ReadKey();
                return;
            }

            try
            {
                var palletFromDB = await ContextDB.GetContext().Pallets.AsQueryable().FirstAsync(p => p.Id == palletInitial.Id);
                palletFromDB.Depth = pallet.Depth;
                palletFromDB.Length = pallet.Length;
                palletFromDB.Width = pallet.Width;
                var result = ContextDB.GetContext().Pallets.Update(palletFromDB);
                await ContextDB.GetContext().SaveChangesAsync();
                Pallets.Remove(palletInitial);
                if (Pallets == null)
                    Pallets = new List<Pallet>();
                Pallets.Add(result.Entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine("Ошибка при обновлении в БД!");
                Console.ReadKey();
                return;
            }
        }
        // Удаление паллета
        private static async Task DeletePallet(Pallet pallet)
        {
            try
            {
                var palletData = await ContextDB.GetContext().Pallets.AsQueryable().FirstAsync(el => el.Id == pallet.Id);
                ContextDB.GetContext().Pallets.Remove(palletData);
                if(pallet.Boxes!= null)
                foreach (Box box in pallet.Boxes)
                {
                    box.Pallet = null;
                    box.PalletId = null;
                    var boxData = await ContextDB.GetContext().Boxes.AsQueryable().FirstAsync(el => el.Id == box.Id);
                    boxData.PalletId = null;
                    boxData.Pallet = null;
                    ContextDB.GetContext().Boxes.Update(boxData);
                }
                await ContextDB.GetContext().SaveChangesAsync();
                Pallets.Remove(pallet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine("Ошибка при удалении паллета из БД");
            }
        }
        // Удаление коробки с паллета
        private static async Task DeleteBoxOnPallet(Pallet pallet)
        {
            Console.Clear();
            if (pallet.Boxes != null && pallet.Boxes.Count > 0)
            {
                foreach (Box box in pallet.Boxes)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Коробка:");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Id: {box.Id} Ширина: {box.Width} Длина: {box.Length} Глубина: " +
                                $"{box.Depth} Вес: {box.Weight} Объем: {box.Volume} Дата производства: {box.ProductionDate.ToShortDateString()}");
                }
                Console.WriteLine("\nВыберите коробку по ее Id. Если такой необходимости нет, то введите цифру 0");
                int number = -1;
                if (!Int32.TryParse(Console.ReadLine(), out number))
                    number = -1;
                if (number == 0)
                    return;
                // Проверка наличия коробки на паллете
                if(pallet.Boxes.Any(p => p.Id == number))
                {
                    try
                    {
                        Box boxData = await ContextDB.GetContext().Boxes.AsQueryable().FirstAsync(el => el.Id == number);
                        Box box = Boxes.First(el => el.Id == number);
                        Pallet palletData = await ContextDB.GetContext().Pallets.AsQueryable().FirstAsync(el => el.Id == pallet.Id);
                        palletData.Boxes.Remove(boxData);
                        box.Pallet = null;
                        box.PalletId = null;
                        boxData.PalletId = null;
                        boxData.Pallet = null;
                        ContextDB.GetContext().Boxes.Update(boxData);
                        ContextDB.GetContext().Pallets.Update(palletData);
                        await ContextDB.GetContext().SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        Console.WriteLine("Ошибка при удалении коробки с паллета!");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Такой коробки нет на паллете");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Коробок нет!");
                Console.ReadKey();
                return;
            }
        }
        // Меню для редактирования или удаления коробки
        private static void EditOrDeleteBox(Box box)
        {
            int number = -1;
            while (true)
            {
                number = -1;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Коробка:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Id: {box.Id} Ширина: {box.Width} Длина: {box.Length} Глубина: " +
                            $"{box.Depth} Вес: {box.Weight} Объем: {box.Volume} Дата производства: {box.ProductionDate.ToShortDateString()}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Паллет:");
                Console.ForegroundColor = ConsoleColor.White;
                if(box.Pallet!=null)
                    Console.WriteLine($"Id: {box.Pallet.Id} Ширина: {box.Pallet.Width} Длина: {box.Pallet.Length} Глубина: {box.Pallet.Depth}" +
                        $" Вес: {box.Pallet.Weight} Объем: {box.Pallet.Volume}");
                else
                    Console.WriteLine("Коробка не лежит на паллете");

                Console.WriteLine();
                Console.WriteLine("1. Редактировать");
                Console.WriteLine("2. Удалить");
                Console.WriteLine("0. Выход\n");
                if (!Int32.TryParse(Console.ReadLine(), out number))
                    number = -1;
                switch (number)
                {
                    case 0:
                        return;
                    case 1:
                        EditBoxMenu(box);
                        break;
                    case 2:
                        Task.Run(async () => await MyMethods.DeleteBox(box)).Wait();
                        return;

                }
            }

        }
        // Меню для редактирования коробки
        private static void EditBoxMenu(Box box)
        {
            Console.WriteLine();
            int number = -1;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Редактировать поля коробки");
                Console.WriteLine("2. Изменить паллет, на котором лежит коробка");
                Console.WriteLine("3. Снять коробку с паллета");
                Console.WriteLine("0. Выход\n");
                if (!Int32.TryParse(Console.ReadLine(), out number))
                    number = -1;
                switch (number)
                {
                    case 0:
                        return;
                    case 1:
                        Task.Run(async () => await MyMethods.EditBox(box)).Wait();
                        break;
                    case 2:
                        Task.Run(async () => await MyMethods.EditBoxOnPallet(box)).Wait();
                        break;
                    case 3:
                        Task.Run(async () => await MyMethods.TakeBoxOffPallet(box)).Wait();
                        break;
                }
            }
        }
        // Редактирование полей коробки
        private static async Task EditBox(Box boxInitial)
        {
            Console.Clear();
            Box box = new Box();
            double check;
            DateTime checkDateTime;
            Console.WriteLine("Введите ширину:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                box.Width = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите высоту:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                box.Length = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите глубину:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                box.Depth = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите вес:");
            if (Double.TryParse(Console.ReadLine(), out check) && check > 0)
                box.Weight = check;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Введите дату производства по типу ДД/ММ/ГГГГ:");
            if (DateTime.TryParse(Console.ReadLine(), out checkDateTime))
                box.ProductionDate = checkDateTime;
            else
            {
                Console.WriteLine("Ошибка ввода!");
                Console.ReadKey();
                return;
            }
            if (box.Length == boxInitial.Length && box.Width == boxInitial.Width && box.Depth == boxInitial.Depth && box.Weight == boxInitial.Weight &&
                box.ProductionDate == boxInitial.ProductionDate)
            {
                Console.WriteLine("Вы не изменили ни одного поля!");
                Console.ReadKey();
                return;
            }

            try
            {
                if (boxInitial.Pallet != null)
                {
                    box.Id= boxInitial.Id;
                    if(AlgorithmEditBoxOnPallet(box, boxInitial.Pallet))
                    {
                        Pallet pallet = Pallets.First(p => p.Id == boxInitial.PalletId);
                        var boxFromDB = await ContextDB.GetContext().Boxes.AsQueryable().FirstAsync(p => p.Id == boxInitial.Id);
                        boxFromDB.Depth = box.Depth;
                        boxFromDB.Length = box.Length;
                        boxFromDB.Width = box.Width;
                        boxFromDB.Weight = box.Weight;
                        boxFromDB.ProductionDate = box.ProductionDate;
                        var result = ContextDB.GetContext().Boxes.Update(boxFromDB);
                        await ContextDB.GetContext().SaveChangesAsync();
                        pallet.Boxes.Remove(boxInitial);
                        Boxes.Remove(boxInitial);
                        if (Boxes == null)
                            Boxes = new List<Box>();
                        Boxes.Add(result.Entity);
                        pallet.Boxes.Add(result.Entity);
                    }
                }
                else
                {
                    var boxFromDB = await ContextDB.GetContext().Boxes.AsQueryable().FirstAsync(p => p.Id == boxInitial.Id);
                    boxFromDB.Depth = box.Depth;
                    boxFromDB.Length = box.Length;
                    boxFromDB.Width = box.Width;
                    boxFromDB.Weight = box.Weight;
                    boxFromDB.ProductionDate = box.ProductionDate;
                    var result = ContextDB.GetContext().Boxes.Update(boxFromDB);
                    await ContextDB.GetContext().SaveChangesAsync();
                    Boxes.Remove(boxInitial);
                    if (Boxes == null)
                        Boxes = new List<Box>();
                    Boxes.Add(result.Entity);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine("Ошибка при обновлении в БД!");
                Console.ReadKey();
                return;
            }
        }
        // Удаление коробки
        private static async Task DeleteBox(Box box)
        {
            try
            {
                var boxData = await ContextDB.GetContext().Boxes.AsQueryable().FirstAsync(el => el.Id == box.Id);
                if (box.Pallet != null)
                {
                    var palletData = await ContextDB.GetContext().Pallets.AsQueryable().FirstAsync(el => el.Id == box.PalletId);
                    palletData.Boxes.Remove(boxData);
                    ContextDB.GetContext().Pallets.Update(palletData);
                    box.Pallet.Boxes.Remove(box);
                }
                ContextDB.GetContext().Boxes.Remove(boxData);
                await ContextDB.GetContext().SaveChangesAsync();
                Boxes.Remove(box);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine("Ошибка при удалении коробки из БД");
            }
        }
        // Изменение паллета, на котором лежит коробка
        private static async Task EditBoxOnPallet(Box box)
        {
            Console.Clear();
            if (Pallets!=null && Pallets.Count>0)
            {
                foreach (Pallet pallet in Pallets)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Паллет:");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Id: {pallet.Id} Ширина: {pallet.Width} Длина: {pallet.Length} Глубина: {pallet.Depth} Вес: {pallet.Weight} Объем: {pallet.Volume}");
                }
                int number = -1;
                Console.WriteLine("\nВыберите паллет по еге Id. Если такой необходимости нет, то введите цифру 0");
                if (!Int32.TryParse(Console.ReadLine(), out number) && number >= 0)
                {
                    Console.WriteLine("Ошибка ввода!");
                    Console.ReadKey();
                    return;
                }
                if (number == 0)
                    return;
                // Проверка наличия паллета
                if (Pallets.Any(p => p.Id == number))
                {
                    try
                    {
                        Pallet palletData = await ContextDB.GetContext().Pallets.AsQueryable().FirstAsync(el => el.Id == number);
                        if (AlgorithmEditBoxOnPallet(box, palletData))
                        {
                            if(box.Pallet == null)
                            {
                                Box boxData = await ContextDB.GetContext().Boxes.AsQueryable().FirstAsync(el => el.Id == box.Id);
                                boxData.Pallet = palletData;
                                boxData.PalletId = palletData.Id;
                                box.Pallet = palletData;
                                box.PalletId = palletData.Id;
                                ContextDB.GetContext().Boxes.Update(boxData);
                                ContextDB.GetContext().Pallets.Update(palletData);
                                await ContextDB.GetContext().SaveChangesAsync();
                            }
                            else
                            {
                                Box boxData = await ContextDB.GetContext().Boxes.AsQueryable().FirstAsync(el => el.Id == box.Id);
                                Pallet pallet = Pallets.First(p => p.Id == box.PalletId);
                                palletData.Boxes.Remove(boxData);
                                boxData.Pallet = pallet;
                                boxData.PalletId = pallet.Id;
                                box.Pallet.Boxes.Remove(box);
                                box.Pallet = pallet;
                                box.PalletId = pallet.Id;
                                Pallet palletToDB = Pallets.First(p => p.Id == number);
                                palletToDB.Boxes.Add(boxData);
                                ContextDB.GetContext().Boxes.Update(boxData);
                                ContextDB.GetContext().Pallets.Update(palletData);
                                ContextDB.GetContext().Pallets.Update(palletToDB);
                                await ContextDB.GetContext().SaveChangesAsync();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Паллет не вмещает коробку!");
                            Console.ReadKey();
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        Console.WriteLine("Ошибка при изменении паллета!");
                        Console.ReadKey();
                    }

                }
                else
                {
                    Console.WriteLine("Паллета с таким Id нет!");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("Паллетов нет!");
                Console.ReadKey();
                return;
            }
        }
        // Снять коробку с паллета
        private static async Task TakeBoxOffPallet(Box box)
        {
            try
            {
                if(box.Pallet != null)
                {
                    Box boxData = await ContextDB.GetContext().Boxes.AsQueryable().FirstAsync(el => el.Id == box.Id);
                    Pallet palletData = await ContextDB.GetContext().Pallets.AsQueryable().FirstAsync(el => el.Id == box.Pallet.Id);
                    palletData.Boxes.Remove(boxData);
                    Pallet palletLocal = Pallets.First(p => p.Id == box.Pallet.Id);
                    box.Pallet = null;
                    box.PalletId = null;
                    boxData.PalletId = null;
                    boxData.Pallet = null;
                    palletLocal.Boxes.Remove(box);
                    ContextDB.GetContext().Boxes.Update(boxData);
                    ContextDB.GetContext().Pallets.Update(palletData);
                    await ContextDB.GetContext().SaveChangesAsync();
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Console.WriteLine("Ошибка при удалении коробки с паллета!");
                Console.ReadKey();
            }
        }
    }
}
