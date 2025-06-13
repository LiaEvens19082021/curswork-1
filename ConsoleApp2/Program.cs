using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static List<Product> products = new List<Product>();
        static List<Category> categories = new List<Category>();
        static List<Provider> providers = new List<Provider>();
        static List<Sale> sales = new List<Sale>();
        static List<Client> clients = new List<Client>();

        const string productFile = "C:\\программированиеС#\\ConsoleApp2\\txt\\Product.txt";
        const string categoryFile = "C:\\программированиеС#\\ConsoleApp2\\txt\\Category.txt";
        const string providerFile = "C:\\программированиеС#\\ConsoleApp2\\txt\\Provider.txt";
        const string saleFile = "C:\\программированиеС#\\ConsoleApp2\\txt\\Sale.txt";
        const string clientFile = "C:\\программированиеС#\\ConsoleApp2\\txt\\Client.txt";

       static void Main(string[] args)
        {
            LoadData();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== Магазин электроники ===");
                Console.WriteLine("1. Добавить категорию");
                Console.WriteLine("2. Добавить / удалить поставщика");
                Console.WriteLine("3. Добавить товар");
                Console.WriteLine("4. Добавить клиента");
                Console.WriteLine("5. Совершить продажу");
                Console.WriteLine("6. Товары с низким остатком");
                Console.WriteLine("7. Продажи за последний месяц");
                Console.WriteLine("8. Поставщики по периоду");
                Console.WriteLine("9. Популярные категории");
                Console.WriteLine("10. Выручка за месяц");
                Console.WriteLine("11. Средний чек");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                switch (Console.ReadLine())
                {
                    case "1": AddCategory(); break;
                    case "2": ManageProvider(); break;
                    case "3": AddProduct(); break;
                    case "4": AddClient(); break;
                    case "5": MakeSale(); break;
                    case "6": ShowLowStock(); break;
                    case "7": ShowRecentSales(); break;
                    case "8": ShowProvidersByDate(); break;
                    case "9": ShowPopularCategories(); break;
                    case "10": ShowMonthlyRevenue(); break;
                    case "11": ShowAverageReceipt(); break;
                    case "0": 
                        SaveData();
                        return;
                    default: Console.WriteLine("Неверный выбор."); break;
                }
            }
              Console.ReadKey();
        }

        static void AddCategory()
        {
            int id = ReadInt("ID: ");
            string name;
            while (true)
            {
                Console.Write("Название: ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) break;
                Console.WriteLine("Ошибка: название не может быть пустым.");
            }
            categories.Add(new Category { Id = id, Name = name });
            Console.WriteLine("Категория добавлена");
        }

        static int ReadInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out result))
                    return result;
                Console.WriteLine("Ошибка: введите целое число.");
            }
        }

        static decimal ReadDecimal(string prompt)
        {
            decimal result;
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out result))
                    return result;
                Console.WriteLine("Ошибка: введите число.");
            }
        }

        static DateTime ReadDate(string prompt)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out result))
                    return result;
                Console.WriteLine("Ошибка: введите дату (например, 01.01.2025).");
            }
        }

        static void ManageProvider()
        {
            Console.Write("1 - Добавить, 2 - Удалить: ");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                int id = ReadInt("ID: ");

                string name;
                while (true)
                {
                    Console.Write("Название: ");
                    name = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(name)) break;
                    Console.WriteLine("Ошибка: название не может быть пустым.");
                }

                string contact;
                while (true)
                {
                    Console.Write("Контакт: ");
                    contact = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(contact)) break;
                    Console.WriteLine("Ошибка: контакт не может быть пустым.");
                }

                providers.Add(new Provider { Id = id, Name = name, Contact = contact });
                Console.WriteLine("Поставщик добавлен");
            }
            else if (choice == "2")
            {
                int id = ReadInt("ID для удаления: ");
                providers.RemoveAll(p => p.Id == id);
                Console.WriteLine("Поставщик удален");
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }

        static void AddProduct()
        {
            int id = ReadInt("ID: ");

            string name;
            while (true)
            {
                Console.Write("Название: ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) break;
                Console.WriteLine("Ошибка: название не может быть пустым.");
            }

            int cat = ReadInt("ID категории: ");
            decimal price = ReadDecimal("Цена: ");
            int stock = ReadInt("Склад: ");
            int prov = ReadInt("ID поставщика: ");

            products.Add(new Product
            {
                Id = id,
                Name = name,
                CategoryId = cat,
                Price = price,
                Stock = stock,
                ProviderId = prov
            });

            Console.WriteLine("Товар добавлен");
        }

        static void AddClient()
        {
            int id = ReadInt("ID: ");

            string name;
            while (true)
            {
                Console.Write("ФИО (только буквы): ");
                name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name) && name.All(c => char.IsLetter(c) || c == ' ' || c == '-'))
                    break;
                Console.WriteLine("Ошибка: ФИО должно содержать только буквы, пробелы или дефисы.");
            }

            string contact;
            while (true)
            {
                Console.Write("Контакт (только цифры): ");
                contact = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(contact) && contact.All(char.IsDigit))
                    break;
                Console.WriteLine("Ошибка: контакт должен содержать только цифры.");
            }

            clients.Add(new Client { Id = id, FullName = name, Contact = contact });
            Console.WriteLine("Клиент добавлен");
        }

        static void MakeSale()
        {
            int saleId = ReadInt("ID продажи: ");
            int clientId = ReadInt("ID клиента: ");
            int productId = ReadInt("ID товара: ");
            int qty = ReadInt("Количество: ");

            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                Console.WriteLine("Ошибка: товар не найден");
                return;
            }

            if (product.Stock < qty)
            {
                Console.WriteLine("Ошибка: недостаточно товара на складе");
                return;
            }

            product.Stock -= qty;
            decimal total = qty * product.Price;

            sales.Add(new Sale
            {
                Id = saleId,
                ClientId = clientId,
                ProductId = productId,
                Date = DateTime.Now,
                Quantity = qty,
                Total = total
            });

            Console.WriteLine("Продажа завершена. Сумма: " + total);
        }

        static void ShowLowStock()
        {
            var low = products.Where(p => p.Stock <= 2);
            Console.WriteLine("\n Низкий остаток:");
            foreach (var p in low)
                Console.WriteLine($"{p.Name} — {p.Stock} шт.");
        }

        static void ShowRecentSales()
        {
            var from = DateTime.Now.AddMonths(-1);
            var recents = sales.Where(s => s.Date >= from);
            Console.WriteLine("\n Продажи за месяц:");
            foreach (var s in recents)
                Console.WriteLine($"{products.First(p => p.Id == s.ProductId).Name} - {s.Date.ToShortDateString()} - {s.Total} руб.");
        }

        static void ShowProvidersByDate()
        {
            DateTime from = ReadDate("С: ");
            DateTime to = ReadDate("По: ");

            var provs = sales
                .Where(s => s.Date >= from && s.Date <= to)
                .Select(s => products.First(p => p.Id == s.ProductId).ProviderId)
                .Distinct();

            Console.WriteLine("\nПоставщики за указанный период:");
            foreach (var id in provs)
                Console.WriteLine(providers.First(p => p.Id == id).Name);
        }

        static void ShowPopularCategories()
        {
            var result = sales.GroupBy(s => products.First(p => p.Id == s.ProductId).CategoryId)
                .OrderByDescending(g => g.Count());
            Console.WriteLine("\n Популярные категории:");
            foreach (var g in result)
                Console.WriteLine($"{categories.First(c => c.Id == g.Key).Name}: {g.Count()} продаж");
        }

        static void ShowMonthlyRevenue()
        {
            int month = ReadInt("Месяц (1-12): ");
            int year = ReadInt("Год: ");

            var sum = sales
                .Where(s => s.Date.Month == month && s.Date.Year == year)
                .Sum(s => s.Total);

            Console.WriteLine("\nВыручка за указанный месяц: " + sum);
        }

        static void ShowAverageReceipt()
        {
            Console.WriteLine("\n Средний чек:");
            Console.WriteLine(sales.Count > 0 ? (sales.Average(s => s.Total).ToString("F2")) : "Нет данных");
        }

        static void SaveData()
        {
            File.WriteAllLines(categoryFile, categories.Select(c => c.Id + "," + c.Name));
            File.WriteAllLines(providerFile, providers.Select(p => p.Id + "," + p.Name + "," + p.Contact));
            File.WriteAllLines(productFile, products.Select(p => string.Join(",", p.Id, p.Name, p.CategoryId, p.Price, p.Stock, p.ProviderId)));
            File.WriteAllLines(clientFile, clients.Select(c => c.Id + "," + c.FullName + "," + c.Contact));
            File.WriteAllLines(saleFile, sales.Select(s => string.Join(",", s.Id, s.ClientId, s.ProductId, s.Date, s.Quantity, s.Total)));
        }

        static void LoadData()
        {
            if (File.Exists(categoryFile))
                categories = File.ReadAllLines(categoryFile)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line =>
                    {
                        var p = line.Split(',');
                        return new Category
                        {
                            Id = int.Parse(p[0]),
                            Name = p[1]
                        };
                    }).ToList();

            if (File.Exists(providerFile))
                providers = File.ReadAllLines(providerFile)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line =>
                    {
                        var p = line.Split(',');
                        return new Provider
                        {
                            Id = int.Parse(p[0]),
                            Name = p[1],
                            Contact = p[2]
                        };
                    }).ToList();

            if (File.Exists(productFile))
                products = File.ReadAllLines(productFile)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line =>
                    {
                        var p = line.Split(',');
                        return new Product
                        {
                            Id = int.Parse(p[0]),
                            Name = p[1],
                            CategoryId = int.Parse(p[2]),
                            Price = decimal.Parse(p[3]),
                            Stock = int.Parse(p[4]),
                            ProviderId = int.Parse(p[5])
                        };
                    }).ToList();

            if (File.Exists(clientFile))
                clients = File.ReadAllLines(clientFile)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line =>
                    {
                        var p = line.Split(',');
                        return new Client
                        {
                            Id = int.Parse(p[0]),
                            FullName = p[1],
                            Contact = p[2]
                        };
                    }).ToList();

            if (File.Exists(saleFile))
                sales = File.ReadAllLines(saleFile)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line =>
                    {
                        var p = line.Split(',');
                        return new Sale
                        {
                            Id = int.Parse(p[0]),
                            ClientId = int.Parse(p[1]),
                            ProductId = int.Parse(p[2]),
                            Date = DateTime.Parse(p[3]),
                            Quantity = int.Parse(p[4]),
                            Total = decimal.Parse(p[5])
                        };
                    }).ToList();
        }

    }
}