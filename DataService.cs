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
        private ServiceCenterDatabase _database;

        public DataService(string dataFilePath = null)
        {
            // Если путь не указан, используем папку проекта
            _dataFilePath = dataFilePath ?? GetProjectDataFilePath();
            LoadData();
        }

        private string GetProjectDataFilePath()
        {
            // Получаем текущую директорию (обычно bin/Debug или bin/Release)
            string currentDir = Directory.GetCurrentDirectory();
            
            // Поднимаемся на 2 уровня вверх до папки проекта
            string projectDir = Directory.GetParent(currentDir).Parent.Parent.FullName;
            
            // Создаем путь к файлу в папке проекта
            string dataFile = Path.Combine(projectDir, "servicecenter_data.json");
            
            return dataFile;
        }

        public void LoadData()
        {
            if (File.Exists(_dataFilePath))
            {
                try
                {
                    string json = File.ReadAllText(_dataFilePath);
                    _database = JsonSerializer.Deserialize<ServiceCenterDatabase>(json);
                }
                catch (Exception ex)
                {
                    // В случае ошибки создаем новую базу с тестовыми данными
                    _database = new ServiceCenterDatabase();
                    InitializeTestData();
                    Console.WriteLine($"Ошибка загрузки данных: {ex.Message}. Созданы тестовые данные.");
                }
            }
            else
            {
                _database = new ServiceCenterDatabase();
                InitializeTestData();
            }
        }

        public void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_database, options);
                
                // Создаем директорию, если она не существует
                string directory = Path.GetDirectoryName(_dataFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                File.WriteAllText(_dataFilePath, json);
                Console.WriteLine($"Данные сохранены в: {_dataFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения данных: {ex.Message}");
                throw;
            }
        }

        // Остальные методы остаются без изменений
        public List<Client> GetAllClients() => _database.Clients;
        public List<RepairOrder> GetAllRepairOrders() => _database.RepairOrders;
        public List<SparePart> GetAllSpareParts() => _database.SpareParts;
        public List<RepairWork> GetAllRepairWorks() => _database.RepairWorks;
        public List<Payment> GetAllPayments() => _database.Payments;

        public Client GetClientById(int id) => _database.Clients.Find(c => c.Id == id);
        public RepairOrder GetRepairOrderById(int id) => _database.RepairOrders.Find(r => r.Id == id);
        public SparePart GetSparePartById(int id) => _database.SpareParts.Find(s => s.Id == id);

        public void AddClient(Client client) => _database.Clients.Add(client);
        public void AddRepairOrder(RepairOrder order) => _database.RepairOrders.Add(order);
        public void AddSparePart(SparePart part) => _database.SpareParts.Add(part);
        public void AddRepairWork(RepairWork work) => _database.RepairWorks.Add(work);
        public void AddPayment(Payment payment) => _database.Payments.Add(payment);

        public void RemoveClient(int id) => _database.Clients.RemoveAll(c => c.Id == id);
        public void RemoveRepairOrder(int id) => _database.RepairOrders.RemoveAll(r => r.Id == id);
        public void RemoveSparePart(int id) => _database.SpareParts.RemoveAll(s => s.Id == id);
        public void RemoveRepairWork(int id) => _database.RepairWorks.RemoveAll(w => w.Id == id);
        public void RemovePayment(int id) => _database.Payments.RemoveAll(p => p.Id == id);

        private void InitializeTestData()
        {
            // Тестовые данные остаются без изменений
            _database.Clients = new List<Client>
            {
                new Client { Id = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Phone = "+7 (912) 345-67-89", Email = "ivanov@example.com", Address = "г. Москва, ул. Ленина, д. 10", RegistrationDate = new DateTime(2023, 1, 15) },
                new Client { Id = 2, LastName = "Петрова", FirstName = "Мария", MiddleName = "Сергеевна", Phone = "+7 (912) 456-78-90", Email = "petrova@example.com", Address = "г. Москва, ул. Пушкина, д. 20", RegistrationDate = new DateTime(2023, 2, 20) },
                new Client { Id = 3, LastName = "Сидоров", FirstName = "Петр", MiddleName = "Алексеевич", Phone = "+7 (912) 567-89-01", Email = "sidorov@example.com", Address = "г. Москва, ул. Гагарина, д. 30", RegistrationDate = new DateTime(2023, 3, 10) }
            };

            _database.RepairOrders = new List<RepairOrder>
            {
                new RepairOrder { Id = 1, ClientId = 1, DeviceType = "Смартфон", DeviceModel = "iPhone 13", ProblemDescription = "Разбит экран", EstimatedCost = 15000, OrderDate = new DateTime(2024, 1, 10), CompletionDate = new DateTime(2024, 1, 12), Status = "Завершен", PaidAmount = 15000 },
                new RepairOrder { Id = 2, ClientId = 2, DeviceType = "Ноутбук", DeviceModel = "ASUS ROG", ProblemDescription = "Не включается", EstimatedCost = 8000, OrderDate = new DateTime(2024, 2, 15), Status = "В работе", PaidAmount = 4000 }
            };

            _database.SpareParts = new List<SparePart>
            {
                new SparePart { Id = 1, Name = "Экран iPhone 13", Category = "Электроника", Description = "Оригинальный экран", Cost = 10000, StockQuantity = 5, Supplier = "Apple" },
                new SparePart { Id = 2, Name = "Батарея ASUS", Category = "Электроника", Description = "Литий-ионная батарея", Cost = 3000, StockQuantity = 10, Supplier = "ASUS" }
            };

            _database.RepairWorks = new List<RepairWork>
            {
                new RepairWork { Id = 1, RepairOrderId = 1, MasterName = "Петров А.А.", WorkDescription = "Замена экрана", LaborCost = 2000, WorkTime = TimeSpan.FromHours(2), WorkDate = new DateTime(2024, 1, 12) },
                new RepairWork { Id = 2, RepairOrderId = 2, MasterName = "Сидоров Б.Б.", WorkDescription = "Диагностика", LaborCost = 1000, WorkTime = TimeSpan.FromHours(1), WorkDate = new DateTime(2024, 2, 16) }
            };

            _database.Payments = new List<Payment>
            {
                new Payment { Id = 1, RepairOrderId = 1, PaymentDate = new DateTime(2024, 1, 12), Amount = 15000, PaymentType = "Оплата по факту" },
                new Payment { Id = 2, RepairOrderId = 2, PaymentDate = new DateTime(2024, 2, 15), Amount = 4000, PaymentType = "Предоплата" }
            };

            SaveData();
        }
    }
}