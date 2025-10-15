using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Lab678.Models;

namespace Lab678.Services
{
    public class DataService
    {
        private readonly string _dataFilePath;
        private PawnShopDatabase _database;

        public DataService(string dataFilePath = "pawnshop_data.json")
        {
            _dataFilePath = dataFilePath;
            LoadData();
        }

        // Загрузка данных из файла
        public void LoadData()
        {
            if (File.Exists(_dataFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_dataFilePath);
                    _database = JsonSerializer.Deserialize<PawnShopDatabase>(json);
                }
                catch
                {
                    _database = new PawnShopDatabase();
                    InitializeTestData();
                }
            }
            else
            {
                _database = new PawnShopDatabase();
                InitializeTestData();
            }
        }

        // Сохранение данных в файл
        public void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_database, options);
            File.WriteAllText(_dataFilePath, json);
        }

        // Получение всех клиентов
        public List<Client> GetAllClients()
        {
            return _database.Clients;
        }

        // Получение всех залоговых предметов
        public List<PledgeItem> GetAllPledgeItems()
        {
            return _database.PledgeItems;
        }

        // Получение всех займов
        public List<Loan> GetAllLoans()
        {
            return _database.Loans;
        }

        // Получение всех платежей
        public List<Payment> GetAllPayments()
        {
            return _database.Payments;
        }

        // Получение клиента по ID
        public Client GetClientById(int id)
        {
            return _database.Clients.Find(c => c.Id == id);
        }

        // Получение залогового предмета по ID
        public PledgeItem GetPledgeItemById(int id)
        {
            return _database.PledgeItems.Find(p => p.Id == id);
        }

        // Инициализация тестовых данных
        private void InitializeTestData()
        {
            // Клиенты
            _database.Clients = new List<Client>
            {
                new Client
                {
                    Id = 1,
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    PassportNumber = "4512 123456",
                    Phone = "+7 (912) 345-67-89",
                    Address = "г. Москва, ул. Ленина, д. 10, кв. 5",
                    RegistrationDate = new DateTime(2023, 1, 15)
                },
                new Client
                {
                    Id = 2,
                    LastName = "Петрова",
                    FirstName = "Мария",
                    MiddleName = "Сергеевна",
                    PassportNumber = "4513 234567",
                    Phone = "+7 (912) 456-78-90",
                    Address = "г. Москва, ул. Пушкина, д. 20, кв. 15",
                    RegistrationDate = new DateTime(2023, 2, 20)
                },
                new Client
                {
                    Id = 3,
                    LastName = "Сидоров",
                    FirstName = "Петр",
                    MiddleName = "Алексеевич",
                    PassportNumber = "4514 345678",
                    Phone = "+7 (912) 567-89-01",
                    Address = "г. Москва, ул. Гагарина, д. 30, кв. 25",
                    RegistrationDate = new DateTime(2023, 3, 10)
                },
                new Client
                {
                    Id = 4,
                    LastName = "Смирнова",
                    FirstName = "Елена",
                    MiddleName = "Дмитриевна",
                    PassportNumber = "4515 456789",
                    Phone = "+7 (912) 678-90-12",
                    Address = "г. Москва, ул. Кирова, д. 40, кв. 35",
                    RegistrationDate = new DateTime(2023, 4, 5)
                },
                new Client
                {
                    Id = 5,
                    LastName = "Козлов",
                    FirstName = "Дмитрий",
                    MiddleName = "Викторович",
                    PassportNumber = "4516 567890",
                    Phone = "+7 (912) 789-01-23",
                    Address = "г. Москва, ул. Мира, д. 50, кв. 45",
                    RegistrationDate = new DateTime(2023, 5, 12)
                }
            };

            // Залоговое имущество
            _database.PledgeItems = new List<PledgeItem>
            {
                new PledgeItem
                {
                    Id = 1,
                    Name = "Золотое кольцо 585 пробы",
                    Category = "Ювелирные изделия",
                    Description = "Обручальное кольцо, вес 3.5г",
                    EstimatedValue = 15000,
                    Condition = "Отличное"
                },
                new PledgeItem
                {
                    Id = 2,
                    Name = "Ноутбук ASUS ROG",
                    Category = "Электроника",
                    Description = "Gaming ноутбук, RTX 3060, 16GB RAM",
                    EstimatedValue = 80000,
                    Condition = "Хорошее"
                },
                new PledgeItem
                {
                    Id = 3,
                    Name = "iPhone 13 Pro Max",
                    Category = "Электроника",
                    Description = "256GB, черный, полный комплект",
                    EstimatedValue = 65000,
                    Condition = "Отличное"
                },
                new PledgeItem
                {
                    Id = 4,
                    Name = "Золотая цепочка 750 пробы",
                    Category = "Ювелирные изделия",
                    Description = "Плетение бисмарк, вес 25г",
                    EstimatedValue = 120000,
                    Condition = "Отличное"
                },
                new PledgeItem
                {
                    Id = 5,
                    Name = "Часы Rolex Submariner",
                    Category = "Часы",
                    Description = "Швейцарские механические часы",
                    EstimatedValue = 450000,
                    Condition = "Отличное"
                },
                new PledgeItem
                {
                    Id = 6,
                    Name = "Серебряные серьги с бриллиантами",
                    Category = "Ювелирные изделия",
                    Description = "925 проба, бриллианты 0.5 карат",
                    EstimatedValue = 35000,
                    Condition = "Хорошее"
                },
                new PledgeItem
                {
                    Id = 7,
                    Name = "PlayStation 5",
                    Category = "Электроника",
                    Description = "Игровая консоль, 825GB, 2 джойстика",
                    EstimatedValue = 45000,
                    Condition = "Отличное"
                }
            };

            // Займы
            _database.Loans = new List<Loan>
            {
                new Loan
                {
                    Id = 1,
                    ClientId = 1,
                    PledgeItemId = 1,
                    LoanAmount = 10000,
                    InterestRate = 5.0m,
                    IssueDate = new DateTime(2024, 1, 10),
                    DueDate = new DateTime(2024, 4, 10),
                    Status = "Активный",
                    PaidAmount = 0
                },
                new Loan
                {
                    Id = 2,
                    ClientId = 2,
                    PledgeItemId = 2,
                    LoanAmount = 50000,
                    InterestRate = 4.5m,
                    IssueDate = new DateTime(2024, 2, 15),
                    DueDate = new DateTime(2024, 5, 15),
                    Status = "Активный",
                    PaidAmount = 15000
                },
                new Loan
                {
                    Id = 3,
                    ClientId = 3,
                    PledgeItemId = 3,
                    LoanAmount = 40000,
                    InterestRate = 4.0m,
                    IssueDate = new DateTime(2023, 12, 1),
                    DueDate = new DateTime(2024, 3, 1),
                    Status = "Погашен",
                    PaidAmount = 44800
                },
                new Loan
                {
                    Id = 4,
                    ClientId = 4,
                    PledgeItemId = 4,
                    LoanAmount = 80000,
                    InterestRate = 3.5m,
                    IssueDate = new DateTime(2024, 1, 20),
                    DueDate = new DateTime(2024, 4, 20),
                    Status = "Активный",
                    PaidAmount = 25000
                },
                new Loan
                {
                    Id = 5,
                    ClientId = 5,
                    PledgeItemId = 5,
                    LoanAmount = 300000,
                    InterestRate = 3.0m,
                    IssueDate = new DateTime(2024, 2, 1),
                    DueDate = new DateTime(2024, 8, 1),
                    Status = "Активный",
                    PaidAmount = 50000
                },
                new Loan
                {
                    Id = 6,
                    ClientId = 1,
                    PledgeItemId = 6,
                    LoanAmount = 20000,
                    InterestRate = 4.5m,
                    IssueDate = new DateTime(2023, 11, 15),
                    DueDate = new DateTime(2024, 2, 15),
                    Status = "Просрочен",
                    PaidAmount = 10000
                }
            };

            // Платежи
            _database.Payments = new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    LoanId = 2,
                    PaymentDate = new DateTime(2024, 3, 1),
                    Amount = 15000,
                    PaymentType = "Основной долг"
                },
                new Payment
                {
                    Id = 2,
                    LoanId = 3,
                    PaymentDate = new DateTime(2024, 2, 15),
                    Amount = 40000,
                    PaymentType = "Основной долг"
                },
                new Payment
                {
                    Id = 3,
                    LoanId = 3,
                    PaymentDate = new DateTime(2024, 2, 15),
                    Amount = 4800,
                    PaymentType = "Проценты"
                },
                new Payment
                {
                    Id = 4,
                    LoanId = 4,
                    PaymentDate = new DateTime(2024, 2, 10),
                    Amount = 25000,
                    PaymentType = "Основной долг"
                },
                new Payment
                {
                    Id = 5,
                    LoanId = 5,
                    PaymentDate = new DateTime(2024, 3, 1),
                    Amount = 50000,
                    PaymentType = "Основной долг"
                },
                new Payment
                {
                    Id = 6,
                    LoanId = 6,
                    PaymentDate = new DateTime(2023, 12, 20),
                    Amount = 10000,
                    PaymentType = "Основной долг"
                }
            };

            SaveData();
        }
    }
}
